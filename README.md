# assistantsService
Implementation of the OpenAI Assistants API.

The idea here is to create an implementation of the OpenAI Assistants API that implements the stateful aspects of the service, and just calling the stateless OpenAI chat completion endpoint.

There is a basic pass-through "Proxy" style implementation of the protocol, just there to verify things. And then, using the same Controllers but swapping out the Models, an implementation that uses Azure blob storage for the state.

An Azure Storage Queue is used to facilitate the asynchronous Run exection. Work items being queued when you create a Run and again if the client needs to Submit tool outputs.

Almost all the protocol is there, certainly the "interesting" parts, with the exception of:
- Metadata
- Some updates: MessageUpdateParams, RunUpdateParams
- ThreadCreateAndRunParams
- Paging on the collections
- Last Error

Otherwise we have:
- need to add ILogger to the models (note the Steps logging is implemented)
- better error handling in the proxy implementation (it's basically test code)
- better validation and therefore error messages on some of the calls
- retrival and code_interpreter tools
- code in the hosted service needs some TLC

