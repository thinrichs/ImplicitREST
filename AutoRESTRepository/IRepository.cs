using AutoREST;

namespace AutoRESTRepository
{
    public interface IRepository<TType, TKey> where TType : class, IWantRESTExposure
    {
        TType Create(TType item);
        TType GetById(TKey id);
        TType Update(TType item);
        TType Delete(TKey id);
    }
}