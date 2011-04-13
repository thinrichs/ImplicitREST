namespace AutoREST
{
    public class Repository<T> where T : class, IRESTable
    {
        private readonly IRepositoryTypeMap _typeMap;
        public Repository(IRepositoryTypeMap typeMap)
        {
            _typeMap = typeMap;
        }

        // TODO:  These static methods are so similar... some way to refactor to common code even though type maps are different types?
        public T Create(T payload)
        {
            var typeMap = _typeMap.CreateEntityMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return null;
            var result = typeMap[type](payload);
            return result as T;
        }

        public T GetById(string id)
        {

            var typeMap = _typeMap.RetrieveEntityMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return null;
            var result = typeMap[type](id);
            return result as T;
        }

        public T Update(T payload)
        {
            var typeMap = _typeMap.UpdateEntityMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return null;
            var result = typeMap[type](payload);
            return result as T;
        }

        public void RemoveById(string id)
        {
            var typeMap = _typeMap.DeleteEntityMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return;
            typeMap[type](id);
        }
    }
}