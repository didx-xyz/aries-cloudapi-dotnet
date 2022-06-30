@ECHO OFF
SET /p migration="Enter migration name: " 
dotnet ef migrations add PersistedGrantDb_%migration% -c AriesCloudAPI.EntityFramework.DbContexts.PersistedGrantDbContext -o Migrations/PersistedGrantDb