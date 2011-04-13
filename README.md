AutoREST -- A WCF based REST engine
====================================

## DESCRIPTION

AutoREST is a WCF based REST engine that supports both JSON and XML.

AutoREST is built on the shoulders of giants, such as 
[Steve Michelotti](http://geekswithblogs.net/michelotti/archive/2010/08/21/restful-wcf-services-with-no-svc-file-and-no-config.aspx)
[StackOverflow](http://www.stackoverflow.com)


There is a sample project called HowToConsumeAutoREST that shows... wait for it... How to Consume AutoREST.

## IMPLEMENTATION STEPS

It is as simple as this:

* Add a reference to the AutoREST project
* Implement IRESTable on the items that you want to expose via REST
* Add [DataContract(Namespace = "")] to your classes that implement IRESTable
* Add [DataMember] to members that you want exposed via REST
* Implement IRepositoryTypeMap.  The class that implements IRepositoryTypeMap is responsible for mapping REST requests to the code that will process the request
	* Note that IRepositoryTypeMap may soon be renamed to ITypeToUnitOfWorkMap, as UnitOfWork is more generic than Repository.  
* Add the following code to your Global application class (similar but slightly different for MVC projects):

		protected void Application_Start()
        {
            var entityRoutePopulator = new EntityRouteRegistrar
            {
                Routes = RouteTable.Routes,
                TypeMap = new RepositoryTypeMapper() // implements IRepositoryTypeMap
            };

            entityRoutePopulator.RegisterRoutes();
        }



## SAMPLE IMPLEMENTATION

The sample implementation is the simplest thing that will work.  It does not follow the best practices.  Best Practices are laid out below this section.
To test the sample implementation perform the following:
		POST to http://localhost/HowToConsumeAutoREST/RESTable/Create with an empty JSON payload like {} 
This should result a response with a JSON body similar to:

		{"APIkey":"1K4q7f9HAEO3cXs6idYYgg=="}
		
The RESTable type is exposed for access according to this interface:
		[ServiceContract]
		public interface IService<T> where T : IRESTable
		{
			[OperationContract]
			T Create(T payload);

			[OperationContract]
			T Read(string id);

			[OperationContract]
			T Update(T payload);

			[OperationContract]
			void Delete(string id);
		}