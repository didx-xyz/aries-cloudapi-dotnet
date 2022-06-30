@ECHO OFF
SET /p migration="Enter migration name: " 
dotnet ef migrations add PersistedTenantDb_%migration% -c AriesCloudAPI.EntityFramework.Contexts.PersistedStateDbContext -o Migrations/PersistedStateDb