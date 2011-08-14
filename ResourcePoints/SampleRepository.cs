using System;
using RESTModels;

namespace ResourcePoints
{
    /// <summary>
    /// Would encapsulate actions against Sample model objects
    /// </summary>
    public class SampleRepository
    {
        /// <summary>
        /// Creates an instance of the Sample Model class
        /// </summary>
        /// <returns></returns>
        public Sample Create()
        {
            return new Sample { ApiKey = GetRandomAPIKey() };
        }

        /// <summary>
        /// Returns a random string
        /// </summary>
        /// <returns></returns>
        private static string GetRandomAPIKey()
        {
            var guid = Guid.NewGuid();
            var guidAsBytes = guid.ToByteArray();
            var guidAsBase64 = Convert.ToBase64String(guidAsBytes);
            return guidAsBase64;
        }
    }
}