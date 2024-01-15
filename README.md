# assistantsService
Implementation of the OpenAI Assistants API.

The idea here is to create an implementation of the OpenAI Assistants API that implements the stateful aspects of the service, and just calling the stateless OpenAI chat completion endpoint.

There is a basic pass-through "Proxy" style implementation of the protocol, just there to verify things. And then, using the same Controllers but swapping out the Models, an implementation that uses Azure blob storage for the state.

An Azure Storage Queue is used to facilitate the asynchronous Run exection. Work items being queued when you create a Run and again if the client needs to Submit tool outputs.

Almost all the protocol is there, certainly the "interesting" parts, with the exception of:
- Metadata and then the related implementation of updates: MessageUpdateParams, RunUpdateParams
- Short cut method ThreadCreateAndRunParams
- Paging on the collections
- Last Error
- Run Failed state

Otherwise we have - roughly in priority order:
- use C# 'required' in schema to remove some of the null programming (this also gives better input validation - and at least the right http error code)
- better validation and therefore error messages on some of the REST calls - basically there is lots of 400 and 404 missing
- the specifics of the factoring around the OpenAI call could be improved 
- the schema definition could be tighter - specifically there are nulls everywhere and that could be better
- the use of constants in the code could be improved
- the Swagger could be improved with descriptions and constraints on values for various string constants
- better error handling in the proxy implementation (it's basically test code)
- retrival and code_interpreter tools

# Setup Instructions

In Azure portal create 5 blob containers:
- assistants
- messages
- runs
- steps
- threads

And a single Azure Queue called "work"

Then on config you need:
- OpenAIKey - which you get from the OpenAI https://platform.openai.com/api-keys
- BlobConnectionString - Access Keys in the Azure Portal

(note I use "dotnet user-secrets" for local testing)
  
Then run the project - and you should see the Swagger page open

I have been using various JavaScript node programs. For example https://github.com/johnataylor/assistants/blob/main/steps/functions.js though I'm sure you could do better.




  
