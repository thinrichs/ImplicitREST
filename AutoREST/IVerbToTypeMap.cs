namespace AutoREST
{
    public interface IVerbToTypeMap
    {
        EntityToEntityFuncMap CreateTypeMap   { get; }
        EntityToEntityFuncMap UpdateTypeMap   { get; }
        StringToEntityFuncMap RetrieveTypeMap { get; }
        StringToEntityFuncMap DeleteTypeMap   { get; }
    }
}