REM this will generate the client models in /Clients/Models/nswag.cs
nswag openapi2csclient /input:http://localhost:8000/openapi.json  /namespace:AriesCloudDotnet.Clients.Models /output:../src/AriesCloudDotnet/Clients/Models/nswag.cs /GenerateClientClasses:false
