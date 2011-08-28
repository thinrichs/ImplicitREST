namespace AutoREST
{
    public interface IVerbToTypeMap
    {
        EntityFuncMap<IRESTable> CreateTypeMap   { get; }
        EntityFuncMap<IRESTable> UpdateTypeMap { get; }
        EntityFuncMap<string> RetrieveTypeMap { get; }
        EntityFuncMap<string> DeleteTypeMap   { get; }
    }
}