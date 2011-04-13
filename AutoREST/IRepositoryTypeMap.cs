namespace AutoREST
{
    public interface IRepositoryTypeMap
    {
        EntityToEntityFuncMap CreateEntityMap { get; }
        EntityToEntityFuncMap UpdateEntityMap { get; }

        StringToEntityFuncMap RetrieveEntityMap { get; }
        StringToEntityFuncMap DeleteEntityMap { get; }
    }
}