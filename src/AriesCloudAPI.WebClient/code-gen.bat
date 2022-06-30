REM To install Microsoft.dotnet-openapi, run the following command:
REM dotnet tool install -g Microsoft.dotnet-openapi

REM dotnet openapi add url http://localhost:8000/openapi.json -c NSwagCSharp


REM this will generate the client models in Models/nswag.cs
nswag openapi2csclient /input:http://localhost:8000/openapi.json  /namespace:AriesCloudAPI.WebClient /output:Models/nswag.cs /GenerateClientClasses:false
