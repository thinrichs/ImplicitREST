AutoREST -- A WCF based REST engine
====================================

## DESCRIPTION

AutoREST is a WCF based REST engine that supports both JSON and XML.

There is a sample project called HowToConsumeAutoREST that shows... wait for it... How to Consume AutoREST.

It is as simple as this:

* Add a reference to the AutoREST project
* Implement IRESTable on the items that you want to expose via REST
* Add [DataContract(Namespace = "")] to your classes that implement IRESTable
* Add [DataMember] to members that you want exposed via REST

## SAMPLE IMPLEMENTATION

The sample implementation of consumption of AutoREST shows some common conventions

* POSTing to http://{your URL}/HowToConsumeAutoREST/RESTable/Create with an empty JSON payload {} will result in a JSON result similar to {"APIkey":"1K4q7f9HAEO3cXs6idYYgg=="}
	* This key can then be used to validate REST requests 
	



