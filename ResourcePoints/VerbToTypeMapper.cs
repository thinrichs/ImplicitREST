using AutoREST;
using RESTModels;

namespace ResourcePoints
{
    public class VerbToTypeMapper : IVerbToTypeMap
    {
        #region Implementation of IVerbToTypeMap

        public EntityFuncMap<IWantRESTExposure> CreateTypeMap
        {
            get // type -> unit of create work
            {
                return new EntityFuncMap<IWantRESTExposure>
                            {/*General Idea here is 
                              *{ TypeToPerformUnitOfWorkOn,  LamdaSpecifyingUnitOfWork } */  
                               { typeof(Sample), r => { return new SampleRepository().Create(); }}
                            };
            }
        }

        public EntityFuncMap<IWantRESTExposure> UpdateTypeMap
        {   // type -> unit of update work
            get { return new EntityFuncMap<IWantRESTExposure>(); }
        }

        public EntityFuncMap<string> RetrieveTypeMap
        {  // type -> unit of retrieval work
            get { return new EntityFuncMap<string>(); }
        }

        public EntityFuncMap<string> DeleteTypeMap
        {   // type -> unit of delete work
            get { return new EntityFuncMap<string>(); }
        }

        #endregion
    }
}
