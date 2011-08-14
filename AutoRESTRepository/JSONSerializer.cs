using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

// modified from http://pietschsoft.com/post/2008/02/NET-35-JSON-Serialization-using-the-DataContractJsonSerializer.aspx
/// <summary>
/// Serialize and Deserialize instances encoded as json objects
/// </summary>
public class JSONSerializer
{
    /// <summary>
    /// Takes an object of type T and turns it into a json string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize<T>(T obj)
    {
        if (obj == null) return string.Empty;
        var serializer = new DataContractJsonSerializer(obj.GetType());
        using (var ms = new MemoryStream())
        {
            serializer.WriteObject(ms, obj);
            return Encoding.Default.GetString(ms.ToArray());
        }
    }
    /// <summary>
    /// Takes a json string and turns it into an object of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string json)
    {
        var obj = Activator.CreateInstance<T>();
        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            return (T)serializer.ReadObject(ms);
        }
    }
}