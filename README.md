# assistantsService
Implementation of the OpenAI Assistants API.

The idea here is to create an implementation of the OpenAI Assistants API that implements the stateful aspects of the service, running the regular functions loop against the OpenAI chat completion endpoint.

There is a basic pass-through "Proxy" style implementation of the protocol, just there to verify things. And then, using the same Controllers but swapping out the Models, an implementation that uses Azure blob storage for the state.

So far we have the Assistant, Thread and Message models and part of the Run Model. You'll need a blob storage account with "assistants," "threads," "messages" and "run" containers added.

Some parts of the protocol are a work in progress:
- pagination on the various "list" operations
- most of the updates, both the schema and the implementation
- metadata - apparantly up to 16 name, string value pairs
- thread create-and-run
- error handling is also missing, you'll get 500 where you should expect 404s
- anything to do with file uploading
