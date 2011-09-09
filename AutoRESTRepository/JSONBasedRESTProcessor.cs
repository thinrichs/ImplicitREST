using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using AutoREST;
namespace AutoRESTRepository
{
    public class JSONBasedRESTProcessor<TType, TKey> where TType : class
    {
        private static Dictionary<RESTVerb, Func<TKey, string>> _verbToUrlMapping;
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer(); // was using JSON.NET, but was getting wierd
        // exceptions related to security.
        // see http://stackoverflow.com/questions/5511171/json-net-says-operation-may-destabilize-the-runtime-under-net-4-but-not-under
        /// <summary>
        /// For the given generic type, proxies requests over to an AutoREST implemented REST resource point
        /// </summary>
        /// <param name="verbToUrlMapping"></param>
        public JSONBasedRESTProcessor(Dictionary<RESTVerb, Func<TKey, string>> verbToUrlMapping)
        {
            _verbToUrlMapping = verbToUrlMapping;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="verb"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static WebRequest GetWebRequest(RESTVerb verb, TKey id)
        {
            // setup request    
            var url = _verbToUrlMapping[verb](id);
            var request = BaseWebRequest(url);
            request.Method = VerbToRequestMethod.Mapping[verb];
            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="request"></param>
        public static void AddItemToRequest(TType item, WebRequest request)
        {
            if (item == null) return;
            var json = Serializer.Serialize(item);
            var byteData = Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteData.Length;

            // Write data      
            using (var postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestTo"></param>
        /// <returns></returns>
        public static WebRequest BaseWebRequest(string requestTo)
        { // would the below webRequest related code be better off if it used WebClient?
            var request = WebRequest.Create(requestTo);
            request.ContentType = "application/json";
            request.Credentials = CredentialCache.DefaultCredentials;
            return request;
        }

        /// <summary>
        /// Send Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static TType SendRequest(WebRequest request)
        {
            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var resultingJson = reader.ReadToEnd();
                    var resultingItem = Serializer.Deserialize<TType>(resultingJson);
                    return resultingItem;
                }
            }
            catch (WebException ex)
            {
                using (var webResponse = (HttpWebResponse)ex.Response)
                using (var stream = webResponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    throw new WebException(ex.Message + Environment.NewLine + content, ex, ex.Status, ex.Response);
                }
            }
        }

        /// <summary>
        /// Changes the given item doing the given verb on it.  Send the item to a REST url
        /// </summary>
        /// <param name="verb"></param>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public TType ProcessItem(RESTVerb verb, TType item, TKey id)
        {
            var request = GetWebRequest(verb, id);
            AddItemToRequest(item, request);
            var result = SendRequest(request);
            return result;
        }
    }
}