# assistantsService
Implementation of the OpenAI Assistants API.

The idea here is to create an implementation of the OpenAI Assistants API that implements the stateful aspects of the service. But runs the regular functions loop against OpenAIs chat completion endpoint.

There is a basic pass throught "Proxy" implementation of the protocol and then the same models but this time partially implemented against Azure blob storage.

So far I've done the Assistant and Thread models. You'll need a blob storage accound and create the assistants and threads containers (I guess I could create those on demand in the code fwiw.)

Parts of the protocol are incomplete:
- pagination
- most of the updates
- thread create-and-run
- metadata
- then all the 404s
- and duplicate errors
- and apparently the timestamp should be unix in seconds, which seems like an odd choice in this day and age
- anything to do with file uploading

  

 

