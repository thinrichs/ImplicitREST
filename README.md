AutoREST -- A .NET WCF based REST engine
====================================

## DESCRIPTION

AutoREST is a .NET 4 WCF based REST engine that supports both JSON and XML.

AutoREST has a few requirements:
* AspNetCompatibilityRequirementsMode has to be set to allowed.  
	* This can be done in configuration with this node
	
			<serviceHostingEnvironment aspNetCompatibilityEnabled="true"/>
			
	* Automatic format selection and automatic help requires the following configuration:
	
	<webHttpEndpoint>
        <standardEndpoint name="" helpEnabled="true" automaticFormatSelectionEnabled="true"/>
    </webHttpEndpoint>
	
AutoREST is built on the shoulders of giants, such as:

* [Steve Michelotti](http://geekswithblogs.net/michelotti/archive/2010/08/21/restful-wcf-services-with-no-svc-file-and-no-config.aspx)
* [StackOverflow](http://www.stackoverflow.com)
* [Andras Zoltan](http://stackoverflow.com/users/157701/andras-zoltan) specifically for his answer to [this stackoverflow question](http://stackoverflow.com/questions/3021613/how-to-pre-load-all-deployed-assemblies-for-an-appdomain)
* I'm sure there are others that I can't remember at this time

There is a sample project called HowToConsumeAutoREST that shows... wait for it... How to Consume AutoREST.

## SAMPLE IMPLEMENTATION

The sample implementation is the simplest thing that will work.  It does not follow the best practices.  Best Practices are laid out below this section.
This and subsequent sections will assume the base URL for the sample is http://localhost/HowToConsumeAutoREST
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
		
WCF gives you a help page for each type also.  The URL looks like this http://localhost/HowToConsumeAutoREST/Restable/help
		
		Operations at http://localhost/HowToConsumeAutoREST/Restable
		This page describes the service operations at this endpoint.

		Uri		Method	Description
		{id}	GET		Service at http://localhost/HowToConsumeAutoREST/Restable/{ID}
				DELETE	Service at http://localhost/HowToConsumeAutoREST/Restable/{ID}
		Create	POST	Service at http://localhost/HowToConsumeAutoREST/Restable/Create
		Update	PUT		Service at http://localhost/HowToConsumeAutoREST/Restable/Update
		
## SAMPLE IMPLEMENTATION

The sample implementation is the simplest thing that will work.  It does not follow the best practices.  Best Practices are laid out below this section.
This and subsequent sections will assume the base URL for the sample is http://localhost/HowToConsumeAutoREST
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
		
WCF gives you a help page for each type also.  The URL looks like this http://localhost/HowToConsumeAutoREST/Restable/help:
		
		Operations at http://localhost/HowToConsumeAutoREST/Restable
		This page describes the service operations at this endpoint.

		Uri		Method	Description
		{id}	GET		Service at http://localhost/HowToConsumeAutoREST/Restable/{ID}
				DELETE	Service at http://localhost/HowToConsumeAutoREST/Restable/{ID}
		Create	POST	Service at http://localhost/HowToConsumeAutoREST/Restable/Create
		Update	PUT		Service at http://localhost/HowToConsumeAutoREST/Restable/Update
		
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
		
		