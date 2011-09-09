﻿using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace AutoREST
{
    public class EntityRouteRegistrar<ToRoute> where ToRoute : class
    {
        public RouteCollection Routes { get; set; }

        public IVerbToTypeMap TypeMap { set; private get; }

        private static readonly WebServiceHostFactory Factory = new WebServiceHostFactory();

        /// <summary>
        /// Registers REST routes for everything that implements the IRESTable interface
        /// </summary>
        public void RegisterRoutes()
        {
            // greedy load all assemblies we can find
            DllPreLoader.PreLoadDeployedAssemblies();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies
                .SelectMany(s =>
                {
                    try { return s.GetTypes(); }
                    catch (ReflectionTypeLoadException)
                    {
                        return new Type[0];
                    }
                })
                .ToList();
            var typeToFind = typeof (ToRoute).Name;
            // filter to types implementing IRESTable
            var restableTypes = types
                .Where(t => t.GetInterface(typeToFind) != null)
                .ToList();
            // add route for each type
            restableTypes.ForEach(AddRouteForType);            
        }

        private void AddRouteForType(Type needsToBeRouted)
        {
            var routeName = RouteTokenOfType(needsToBeRouted);
            var route = ServiceRouteByRouteName(routeName);
            // dont add if we can already find it in the list of routes (is this possible?)
            if (route != null) return;
            route = RouteOfType(needsToBeRouted, routeName);
            Routes.Add(route);
        }

        private ServiceRoute ServiceRouteByRouteName(string routeName)
        {
            return Routes
               .Where(r => r is ServiceRoute)
               .Cast<ServiceRoute>()
               .Where(r => r.Url.Split('/')[0] == routeName)
               .FirstOrDefault();
        }

        private ServiceRoute RouteOfType(Type typeToRoute, string routeName)
        {
            // reflection for generic type from here http://stackoverflow.com/questions/67370/dynamically-create-a-generic-type-for-template
            var serviceType = typeof(Service<>);
            var genericServiceType = serviceType.MakeGenericType(typeToRoute);
            // Is there a better way to set the value on the static property of the generic class created above?
            var repositoryTypeMap = genericServiceType.GetProperty("VerbTypeMap");
            //var repositoryTypeMap = genericServiceType.GetProperties()[0]; // hack that only works because I know there is only one property!
            repositoryTypeMap.SetValue(null, TypeMap, null);
            // now that type is setup, add route
            var route = new ServiceRoute(routeName, Factory, genericServiceType);
            return route;
        }

        public static string RouteTokenOfType(Type typeToRoute)
        {
            var attributes = typeToRoute.GetCustomAttributes(true);
            var routeNameAttribute = attributes
                .Where(a => a is RouteNameAttribute)
                .Cast<RouteNameAttribute>()
                .FirstOrDefault();
            return (routeNameAttribute != default(RouteNameAttribute)) ? routeNameAttribute.Name : typeToRoute.Name;
        }
    }
}
