namespace ImplicitREST
{
    class Repository<T> where T : class, IWantRESTExposure
    {
        private readonly IVerbToTypeMap _typeMap;
        public Repository(IVerbToTypeMap typeMap)
        {
            _typeMap = typeMap;
        }

        // TODO:  These static methods are so similar... some way to refactor to common code even though type maps are different types?
        public T Create(T payload)
        {
            var typeMap = _typeMap.CreateTypeMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return null;
            var result = typeMap[type](payload);
            return result as T;
        }

        public T GetById(string id)
        {

            var typeMap = _typeMap.RetrieveTypeMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return null;
            var result = typeMap[type](id);
            return result as T;
        }

        public T Update(T payload)
        {
            var typeMap = _typeMap.UpdateTypeMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return null;
            var result = typeMap[type](payload);
            return result as T;
        }

        public void RemoveById(string id)
        {
            var typeMap = _typeMap.DeleteTypeMap;
            var type = typeof(T);
            if (!typeMap.ContainsKey(type)) return;
            typeMap[type](id);
        }
    }
}