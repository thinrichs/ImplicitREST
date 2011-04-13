using AutoREST;
using Model.Domain;
using Repositories;

namespace HowToConsumeAutoREST
{
    public class RepositoryTypeMapper : IRepositoryTypeMap
    {
        #region Implementation of IRepositoryTypeMap

        public EntityToEntityFuncMap CreateEntityMap
        {
            get // type -> unit of create work
            {
                return new EntityToEntityFuncMap
                            {/*General Idea here is 
                              *{ TypeToPerformUnitOfWorkOn,  LamdaSpecifyingUnitOfWork } */  
                               { typeof(Restable), r => { return new RestableRepository().Create(); }}
                            };
            }
        }

        public EntityToEntityFuncMap UpdateEntityMap
        {   // type -> unit of update work
            get { return new EntityToEntityFuncMap(); }
        }

        public StringToEntityFuncMap RetrieveEntityMap
        {  // type -> unit of retrieval work
            get { return new StringToEntityFuncMap(); }
        }

        public StringToEntityFuncMap DeleteEntityMap
        {   // type -> unit of delete work
            get { return new StringToEntityFuncMap(); }
        }

        #endregion
    }
}
