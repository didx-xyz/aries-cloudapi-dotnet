using AriesCloudAPI.Services;
using AriesCloudAPI.Stores;
using AriesCloudAPI.WebClient.Clients;
using AriesCloudAPI.WebClient.Commands;
using AriesCloudAPI.WebClient.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AriesCloudAPI.WebClient.UnitTests.v1
{
    public class AgentTests : ServiceAgentBaseTest
    {
        [Fact]
        public async Task ShouldGetAllTenantsAsync()
        {
            // Arrange
            var inMemoryPersistedTenantStore = new InMemoryPersistedTenantStore();

            // Act
            var content = new List<Tenant>
            {
               new Tenant(),
               new Tenant(),
               new Tenant()
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(content))
            };

            var mockHandler = new MockHttpMessageHandler(httpResponse);
            var httpclient = new HttpClient(mockHandler);

            var tenantAdminServiceAgent = new TenantAdminServiceAgent(httpclient, Options.Value, inMemoryPersistedTenantStore);

            using (new AssertionScope())
            {
                // create issuer
                var command = new CreateTenantCommand
                {
                    Name = "Issuer1",
                    ImageUrl = "",
                    Roles = new TenantRole[] { TenantRole.Issuer }
                };

                var tenantKey = await tenantAdminServiceAgent.CreateAsync(command);

                // assert
                tenantKey.Should().NotBeEmpty();

                // get connections as issuer
                var connectionServiceAgent = new ConnectionServiceAgent(httpclient, Options.Value, entityKeyResolver);
                EntityKeyResolver entityKeyResolver = new EntityKeyResolver(inMemoryPersistedTenantStore);


             
                    tenantKey.Should().BeEquivalentTo(content);
                    mockHandler.LastRequest.Method.Should().Be(HttpMethod.Get);
                    mockHandler.LastRequest.RequestUri.ToString().Should().BeEquivalentTo($"{Options.Value.BaseUri}/admin/tenants/");
               
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