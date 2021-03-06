using System;
using System.Text.RegularExpressions;
using WebAPI.Common.Abstractions;
using WebAPI.Common.Models.Raven.Keys;
using WebAPI.Dashboard.Areas.secure.Models.ViewModels;

namespace WebAPI.Dashboard.Commands.Key
{
    using Common.Exceptions;

    public class ValidateAndGetKeyTypeCommand : Command<ApiKey.ApplicationType>
    {
        private static readonly Regex IpValidate = new Regex(
            @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");

        private static readonly Regex UrlValidate =
            new Regex(@"\*?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\,\'/\\\~])*\*?$");

        public ValidateAndGetKeyTypeCommand(ApiKeyData data)
        {
            Data = data;
        }

        public ApiKeyData Data { get; set; }

        protected override void Execute()
        {
            if (!string.IsNullOrEmpty(Data.UrlPattern))
            {
                Result = ValidateUrl();
                return;
            }

            if (!string.IsNullOrEmpty(Data.Ip))
            {
                Result = ValidateIp();
                return;
            }

            throw new CommandValidationException("UrlPattern and IP are empty. Please fill out one when trying to create an API key.");
        }

        private ApiKey.ApplicationType ValidateIp()
        {
            if (!IpValidate.IsMatch(Data.Ip))
            {
                throw new CommandValidationException("IP is not in the correct format.");
            }

            return ApiKey.ApplicationType.Server;
        }

        private ApiKey.ApplicationType ValidateUrl()
        {
            var validUrl = UrlValidate.IsMatch(Data.UrlPattern);
            var validIp = IpValidate.IsMatch(Data.UrlPattern);

            if (Data.AppStatus == ApiKey.ApplicationStatus.Development)
            {
                return ApiKey.ApplicationType.Browser;
            }

            if (!validUrl && !validIp)
            {
                throw new ArgumentException("Url pattern is not in the correct format.");
            }

            if (Data.AppStatus == ApiKey.ApplicationStatus.Production && validIp)
            {
                if (Data.UrlPattern.Contains("127.0.0.1"))
                {
                    throw new ArgumentException("Url pattern is not in the correct format.");
                }
            }

            return ApiKey.ApplicationType.Browser;
        }

        public override string ToString()
        {
            return string.Format("{0}, Data: {1}", "ValidateAndGetKeyTypeCommand", Data);
        }
    }
}