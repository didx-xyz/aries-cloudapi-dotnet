using AriesCloudAPI.Services;
using AriesCloudAPI.Stores;
using AriesCloudAPI.WebClient.Clients;
using AriesCloudAPI.WebClient.Commands;
using FluentAssertions;
using FluentAssertions.Execution;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AriesCloudAPI.WebClient.IntegrationTests.v1
{
    public class AgentTests : ServiceAgentBaseTest
    {
        [Fact]
        public async Task ShouldGetAllTenantsAsync()
        {
            // Arrange
            var inMemoryStateStore = new InMemoryStateStore();
            var entityKeyResolver = new EntityKeyResolver(inMemoryStateStore);

            var tenantAdminServiceAgent = new TenantAdminServiceAgent(new HttpClient(), Options.Value, inMemoryStateStore, entityKeyResolver);
            var connectionServiceAgent = new ConnectionServiceAgent(new HttpClient(), Options.Value, inMemoryStateStore, entityKeyResolver);
            var schemaServiceAgent = new SchemaServiceAgent(new HttpClient(), Options.Value, inMemoryStateStore, entityKeyResolver);

            // Act  
            using (new AssertionScope())
            {
                //* create tenant (issuer)
                var command = new CreateTenantCommand
                {
                    Name = $"issuer_" + Guid.NewGuid().ToString(),
                    ImageUrl = "",
                    Roles = new TenantRole[] { TenantRole.Issuer }
                };

                var apiKeyIssuer = await tenantAdminServiceAgent.CreateAsync(command);

                apiKeyIssuer.Should().NotBeEmpty(); // assert

                //* create tenant (holder)
                command = new CreateTenantCommand
                {
                    Name = $"holder_" + Guid.NewGuid().ToString(),
                    ImageUrl = "",
                    Roles = null
                };

                var apiKeyHolder = await tenantAdminServiceAgent.CreateAsync(command);

                apiKeyHolder.Should().NotBeEmpty(); // assert

                //* create invitation (issuer) 
                var issuerInvitationKey = await connectionServiceAgent.CreateInvitationAsync(apiKeyIssuer);

                //* accept invitation (holder) 
                await connectionServiceAgent.AcceptInvitationAsync(apiKeyHolder, issuerInvitationKey);
                 
                //* get schemas

                //* create schema
                var commandCreateSchema = new CreateSchemaCommand
                {
                    Name = $"schema_" + Guid.NewGuid().ToString(),
                    Version = "4.2.0",
                    Attributes = new string[] { "skill", "age" }
                };

                var schemaId = await schemaServiceAgent.CreateSchemaAsync(commandCreateSchema);
                schemaId.Should().NotBeEmpty(); // assert

                //* create credential definition (issuer)
                var credentialDefinitionId = await schemaServiceAgent.CreateCredentialDefinitionAsync(apiKeyIssuer, schemaId);
                credentialDefinitionId.Should().NotBeEmpty(); // assert

            }

            //[Fact]
            //public async Task ShouldGetBeslotenVennootschapSummaryAsync()
            //{
            //	var instance = BeslotenVennootschapSummaryRequestFactory.ValidInstance();
            //	var content = new BeslotenVennootschapSummaryResponse();

            //	content.BeslotenVennootschapCalculatedOutput = new List<BeslotenVennootschapCategorieCalculatedOutput>
            //	{
            //		new BeslotenVennootschapCategorieCalculatedOutput
            //		{
            //			Categorie = BeslotenVennootschapCategorie.BalansActief,
            //			SummaryVectors = new List<SummaryVector>()
            //		}
            //	};

            //	var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            //	{
            //		Content = new StringContent(JsonSerializer.Serialize(content))
            //	};

            //	var mockHandler = new MockHttpMessageHandler(httpResponse);
            //	var httpclient = new HttpClient(mockHandler);

            //	var sut = new AlgemeenServiceAgent(httpclient, Options, AuthenticationProvider);

            //	var result = await sut.GetBeslotenVennootschapSummaryAsync(instance);

            //	using (new AssertionScope())
            //	{
            //		result.Should().BeEquivalentTo(content);
            //		mockHandler.LastRequest.Method.Should().Be(HttpMethod.Post);
            //		mockHandler.LastRequest.RequestUri.ToString().Should().BeEquivalentTo($"{BaseUrl}/algemeen/getbeslotenvennootschapsummary");
            //	}
            //}
        }
    }
}