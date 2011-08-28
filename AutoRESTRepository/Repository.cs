using System;
using System.Collections.Generic;
using AutoREST;

namespace AutoRESTRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> where T : class
    {
        // Architecture issue here.  Repository should know nothing about persistence, etc, but currently need to give RestURI.  What to do?  
        public static string BaseRestUri { private get; set; }

        private readonly JSONBasedRESTProcessor<T> _processor;

        public Repository(string baseRestUrl)
        {
            _processor = new JSONBasedRESTProcessor<T>(_verbToUrlMapping);
            BaseRestUri = baseRestUrl;
        }

        private readonly Dictionary<RESTVerb, Func<int, string>> _verbToUrlMapping = new Dictionary<RESTVerb, Func<int, string>>
                                                                                         {
                                                                                             { RESTVerb.Create,  id => RESTUrl("Create")   },
                                                                                             { RESTVerb.Update,  id => RESTUrl("Update")   },
                                                                                             { RESTVerb.Retrieve,id => RESTUrl(id) },
                                                                                             { RESTVerb.Delete,  id => RESTUrl(id) }
                                                                                         };
       
        private static string RESTUrl(object param)
        {
            var routeToken = EntityRouteRegistrar<IWantRESTExposure>.RouteTokenOfType(typeof (T));
            return String.Format("{0}/{1}/{2}", BaseRestUri, routeToken, param);
        }

        public T Create(T item)
        {
            return _processor.ProcessItem(RESTVerb.Create, item);
        }

        public T GetById(int id)
        {
            return _processor.ProcessItem(RESTVerb.Retrieve, null, id);
        }

        public T Update(T item)
        {
            return _processor.ProcessItem(RESTVerb.Update, item);
        }

        public T Delete(int id)
        {
            return _processor.ProcessItem(RESTVerb.Delete, null, id);
        }
    }
}