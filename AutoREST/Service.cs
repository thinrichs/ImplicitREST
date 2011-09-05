using System;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace AutoREST
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service<T> : IService<T> where T : class, IWantRESTExposure
    {
        public static IVerbToTypeMap VerbTypeMap { private get; set; }

        private readonly Repository<T> _repository;

        public Service()
        {
            _repository = new Repository<T>(VerbTypeMap);
        }

        #region IService<T> Members

        [WebInvoke(Method = HttpVerb.Post, UriTemplate = "Create/")]
        public T Create(T payload)
        {
            if (payload == null) return null;
            Func<T> action = () => _repository.Create(payload);
            var result = PerformAction(action);
            return result;
        }

        [WebInvoke(Method = HttpVerb.Get, UriTemplate = "{id}/")]
        public T Read(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            Func<T> action = () => _repository.GetById(id);
            var result = PerformAction(action);
            return result;
        }

        [WebInvoke(Method = HttpVerb.Put, UriTemplate = "Update/")]
        public T Update(T payload)
        {
            if (payload == null) return null;
            Func<T> action = () => _repository.Update(payload);
            var result = PerformAction(action);
            return result;
        }

        [WebInvoke(Method = HttpVerb.Delete,UriTemplate = "{id}/")]
        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            Func<T> action = () =>
            {
                _repository.RemoveById(id);
                return null;
            };

            PerformAction(action);
        }
        #endregion

        private static T PerformAction(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                var stackTrace = ex.StackTrace;
                // Add Logging, Error handling etc here
                throw;
            }
        }
    }

}