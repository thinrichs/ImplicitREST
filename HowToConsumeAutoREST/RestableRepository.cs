using System;
using Model.Domain;

namespace Repositories
{
    public class RestableRepository
    {
        public Restable Create()
        {
            return new Restable { APIkey = GetRandomAPIKey() };
        }

        private static string GetRandomAPIKey()
        {
            var guid = Guid.NewGuid();
            var guidAsBytes = guid.ToByteArray();
            var guidAsBase64 = Convert.ToBase64String(guidAsBytes);
            return guidAsBase64;
        }
    }
}