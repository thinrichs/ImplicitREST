using System;
using System.Collections.Generic;
using System.Web;
using AutoREST;
using System.Text;

namespace AutoRESTRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TType"></typeparam>
    public class Repository<TType, TKey> where TType : class, IWantRESTExposure
    {
        // Architecture issue here.  Repository should know nothing about persistence, etc, but currently need to give RestURI.  What to do?  
        public static string BaseRestUri { private get; set; }

        private readonly JSONBasedRESTProcessor<TType, TKey> _processor;

        public Repository(string baseRestUrl)
        {
            _processor = new JSONBasedRESTProcessor<TType, TKey>(_verbToUrlMapping);
            BaseRestUri = baseRestUrl;
        }

        private readonly Dictionary<RESTVerb, Func<TKey, string>> _verbToUrlMapping = new Dictionary<RESTVerb, Func<TKey, string>>
        {
            { RESTVerb.Create,  id => RESTUrl("Create")   },
            { RESTVerb.Update,  id => RESTUrl("Update")   },
            { RESTVerb.Retrieve,id => RESTUrl(id) },
            { RESTVerb.Delete,  id => RESTUrl(id) }
        };
        
        private static string RESTUrl(object param)
        {
            var routeToken = EntityRouteRegistrar<IWantRESTExposure>.RouteTokenOfType(typeof (TType));

            return String.Format("{0}/{1}/{2}/", BaseRestUri, routeToken, param);
        }

        public TType Create(TType item)
        {
            return _processor.ProcessItem(RESTVerb.Create, item, default(TKey));
        }

        public TType GetById(TKey id)
        {
            return _processor.ProcessItem(RESTVerb.Retrieve, null, id);
        }

        public TType Update(TType item)
        {
            return _processor.ProcessItem(RESTVerb.Update, item, default(TKey));
        }

        public TType Delete(TKey id)
        {
            return _processor.ProcessItem(RESTVerb.Delete, null, id);
        }
    }
}