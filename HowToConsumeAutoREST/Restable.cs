using System.Runtime.Serialization;
using AutoREST;

namespace Model.Domain
{
    /// <summary>
    /// Sample class to show how AutoREST can work.  
    /// </summary>
    [DataContract(Namespace = "")]
    public class Restable : IRESTable
    {
        #region Implementation of IRESTable
        /// <summary>
        /// Perhaps each REST client has to register, get an APIKey and send it with each request
        /// </summary>
        [DataMember]
        public string APIkey { get; set; }
        #endregion
    }
}