﻿using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using WebAPI.API.ModelBindings.Providers;

namespace WebAPI.API
{
    public static class ModelBindingConfig
    {
        public static void RegisterModelBindings(ServicesContainer services)
        {
            services.Add(typeof (ModelBinderProvider), new GeocodeOptionsModelBindingProvider());
            services.Add(typeof (ModelBinderProvider), new ArcGisOnlineOptionsModelBindingProvide());
            services.Add(typeof (ModelBinderProvider), new RouteMilepostOptionsModelBindingProvider());
            services.Add(typeof (ModelBinderProvider), new SearchOptionsModelBindingProvider());
            services.Add(typeof(ModelBinderProvider), new ReverseMilepostOptionsModelBindingProvider());
            services.Add(typeof(ModelBinderProvider), new ReverseGeocodeOptionsModelBindingProvider());
        }
    }
}