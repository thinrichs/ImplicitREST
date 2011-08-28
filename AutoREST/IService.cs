// inspiration: http://geekswithblogs.net/michelotti/archive/2010/08/21/restful-wcf-services-with-no-svc-file-and-no-config.aspx

using System.ServiceModel;

namespace AutoREST
{
    [ServiceContract]
    public interface IService<T> where T : IWantRESTExposure
    {
        [OperationContract]
        T Create(T payload);

        [OperationContract]
        T Read(string id);

        [OperationContract]
        T Update(T payload);

        [OperationContract]
        void Delete(string id);
    }
}