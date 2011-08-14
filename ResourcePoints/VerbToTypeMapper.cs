using AutoREST;
using RESTModels;

namespace ResourcePoints
{
    public class VerbToTypeMapper : IVerbToTypeMap
    {
        #region Implementation of IVerbToTypeMap

        public EntityToEntityFuncMap CreateTypeMap
        {
            get // type -> unit of create work
            {
                return new EntityToEntityFuncMap
                            {/*General Idea here is 
                              *{ TypeToPerformUnitOfWorkOn,  LamdaSpecifyingUnitOfWork } */  
                               { typeof(Sample), r => { return new SampleRepository().Create(); }}
                            };
            }
        }

        public EntityToEntityFuncMap UpdateTypeMap
        {   // type -> unit of update work
            get { return new EntityToEntityFuncMap(); }
        }

        public StringToEntityFuncMap RetrieveTypeMap
        {  // type -> unit of retrieval work
            get { return new StringToEntityFuncMap(); }
        }

        public StringToEntityFuncMap DeleteTypeMap
        {   // type -> unit of delete work
            get { return new StringToEntityFuncMap(); }
        }

        #endregion
    }
}
