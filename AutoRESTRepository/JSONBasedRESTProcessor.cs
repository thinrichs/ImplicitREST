﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using AutoREST;

public class JSONBasedRESTProcessor<T> where T : class
{
    private static Dictionary<RESTVerb, Func<int, string>> _verbToUrlMapping;

    /// <summary>
    /// For the given generic type, proxies requests over to an AutoREST implemented REST resource point
    /// </summary>
    /// <param name="verbToUrlMapping"></param>
    /// <param name="verbToRequestMethodMapping"></param>
    public JSONBasedRESTProcessor(Dictionary<RESTVerb, Func<int, string>> verbToUrlMapping)
    {
        _verbToUrlMapping = verbToUrlMapping;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="verb"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private static WebRequest GetWebRequest(RESTVerb verb, int id)
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
    public static void AddItemToRequest(T item, WebRequest request)
    {
        if (item == null) return;
        var json = JSONSerializer.Serialize(item);
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
    public static T SendRequest(WebRequest request) 
    {
        try
        {
            using (var response = request.GetResponse() as HttpWebResponse)
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var resultingJson = reader.ReadToEnd();
                var resultingItem = JSONSerializer.Deserialize<T>(resultingJson);
                if (resultingItem == null)
                {
                    throw new ArgumentNullException("collection");
                }
                return resultingItem;
            }
        }
        catch (WebException ex)
        {
            var webResponse = (HttpWebResponse)ex.Response;
            // nothing to get so return null;
            if (webResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            throw;
        }
    }

    /// <summary>
    /// Changes the given item doing the given verb on it.  Send the item to a REST url
    /// </summary>
    /// <param name="verb"></param>
    /// <param name="item"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public T ProcessItem(RESTVerb verb, T item, int id = int.MinValue)
    {
        var request = GetWebRequest(verb, id);
        AddItemToRequest(item, request);
        var result = SendRequest(request);
        return result;
    }
}