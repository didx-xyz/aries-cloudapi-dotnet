REM this will generate the client models in /Clients/Models/nswag.cs
REM nswag openapi2csclient /input:http://localhost:8000/openapi.json  /namespace:AriesCloudDotnet.Clients.Models /output:../src/AriesCloudDotnet/Clients/Models/nswag.cs /GenerateClientClasses:false

nswag openapi2csclient /input:http://ssi-beta.didx.co.za:8100/openapi.json  /namespace:AriesCloudDotnet.Clients.Models /output:C:/Users/User/source/repos/aries-cloudapi-dotnet/src/AriesCloudDotnet/Clients/Models/nswag01.cs /GenerateClientClasses:false

REM C:/Users/User/source/repos/aries-cloudapi-dotnet/src/AriesCloudDotnet/Clients/Models