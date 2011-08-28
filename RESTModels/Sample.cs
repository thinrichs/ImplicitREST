using System.Runtime.Serialization;
using AutoREST;

namespace RESTModels
{
    /// <summary>
    /// Sample class to show how AutoREST can work.  
    /// </summary>
    [DataContract(Namespace = "")]
    [RouteName(Name = "SampleResource")]
    public class Sample : IWantRESTExposure
    {
        #region Implementation of IRESTable
        /// <summary>
        /// Perhaps each REST client has to register, get an APIKey and send it with each request
        /// </summary>
        [DataMember]
        public string ApiKey { get; set; }
        #endregion
    }
}
