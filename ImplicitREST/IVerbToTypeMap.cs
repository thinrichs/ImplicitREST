namespace ImplicitREST
{
    public interface IVerbToTypeMap
    {
        EntityFuncMap<IWantRESTExposure> CreateTypeMap   { get; }
        EntityFuncMap<IWantRESTExposure> UpdateTypeMap { get; }
        EntityFuncMap<string> RetrieveTypeMap { get; }
        EntityFuncMap<string> DeleteTypeMap   { get; }
    }
}