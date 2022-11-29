using AriesCloudAPI.Clients.AriesCloud.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AriesCloudDotnet.IntegrationTests.v1
{
    public class ClientTests : ServiceAgentBaseTest
    {
        [Fact]
        public async Task ShouldCompleteAsync()
        {
            // Arrange 
            var clientGoverance = Factory.CreateGoveranceClient();
            var clientTenantAdmin = Factory.CreateTenantAdminClient();

            #region Schemas (Goveranance)

            var schemaId = "";
            var requestCreateSchema = new CreateSchema
            {
                Name = "TestSchema",
                Attribute_names = new string[] { "skill", "age" },
                Version = "4.2.0"
            };

            // Get Schema
            var schemas = await clientGoverance.GetSchemasAsync(schema_name: requestCreateSchema.Name);
            schemas.Should().NotBeNull();  // Assert
            schemas.Should().NotBeNull().And.Contain(x => x.Name == requestCreateSchema.Name); // Assert

            var schema = schemas.First(x => x.Name == requestCreateSchema.Name);
            if (schema == null)
            {
                // Create Schema
                var responseCreateSchema = await clientGoverance.CreateSchemaAsync(requestCreateSchema);
                responseCreateSchema.Should().NotBeNull(); // Assert

                schemaId = responseCreateSchema.Id;
            }
            else
                schemaId = schema.Id;

            #endregion Schemas (Goveranance)

            #region Tenants (Issuer)

            var tenantIdIssuer = "";
            var requestCreateTenantIssuer = new CreateTenantRequest
            {
                Name = "Test_Issuer",
                Roles = new Roles2[] { Roles2.Issuer },
                Image_url = new Uri("https://yoma.africa/images/sample.png"),
            };

            // Get Tenant
            var group_id = "TCM Group";
            var tenants = await clientTenantAdmin.GetTenantsAsync(group_id);
            tenants.Should().NotBeNull();  // Assert 

            var tenant = tenants.FirstOrDefault(x => x.Tenant_name == requestCreateTenantIssuer.Name);
            if (tenant == null)
            {
                // Create Tenant
                var responseCreateTenantIssuer = await clientTenantAdmin.CreateTenantAsync(requestCreateTenantIssuer);
                responseCreateTenantIssuer.Should().NotBeNull(); // Assert

                tenantIdIssuer = responseCreateTenantIssuer.Tenant_id;
            }
            else
                tenantIdIssuer = tenant.Tenant_id;

            #endregion Tenants (Issuer)

            #region Tenants (Member)

            var tenantIdMember = "";
            var requestCreateTenantMember = new CreateTenantRequest
            {
                Name = "Test_Member"
            };

            // Get Tenant
            tenants = await clientTenantAdmin.GetTenantsAsync(group_id);
            tenants.Should().NotBeNull();  // Assert 

            tenant = tenants.FirstOrDefault(x => x.Tenant_name == requestCreateTenantMember.Name);
            if (tenant == null)
            {
                // Create Tenant
                var responseCreateTenantMember = await clientTenantAdmin.CreateTenantAsync(requestCreateTenantMember);
                responseCreateTenantMember.Should().NotBeNull(); // Assert

                tenantIdMember = responseCreateTenantMember.Tenant_id;
            }
            else
                tenantIdMember = tenant.Tenant_id;

            #endregion Tenants (Member)

            #region Credential Definitions (Issuer)

            var clientTenantIssuer = Factory.CreateTenantClient(tenantIdIssuer);

            var credentialdefinitionId = "";
            var requestCreateCredentialDefinition = new CreateCredentialDefinition
            {
                Schema_id = schemaId,
                Tag = "default"
            };

            // Get Credential Definition
            var credentialdefinitions = await clientTenantIssuer.GetCredentialDefinitionsAsync(schema_id: schemaId);
            credentialdefinitions.Should().NotBeNull();  // Assert 

            var credentialdefinition = credentialdefinitions.FirstOrDefault(/*x => x.Schema_id == schemaId*/); // issue: Schema_id in response is invalid, so can't lookup
            if (credentialdefinition == null)
            {
                // Create CredentialDefinition
                var responseCreateCredentialDefinition = await clientTenantIssuer.CreateCredentialDefinitionAsync(requestCreateCredentialDefinition);
                responseCreateCredentialDefinition.Should().NotBeNull(); // Assert

                credentialdefinitionId = responseCreateCredentialDefinition.Id;
            }
            else
                credentialdefinitionId = credentialdefinition.Id;

            #endregion Credential Definitions

            #region Connections

            var connectionIdIssuer = string.Empty;
            var connectionIdMember = string.Empty;
            var clientTenantMember = Factory.CreateTenantClient(tenantIdMember);

            var connectionsIssuer = await clientTenantIssuer.GetConnectionsAsync();
            var connectionsMember = await clientTenantMember.GetConnectionsAsync();

            if (!(connectionsIssuer.Any() && connectionsMember.Any()))
            {
                // create invitation as issuer  
                var requestCreateInvitationInsuer = new CreateInvitation();
                var responseCreateInvitationInsuer = await clientTenantIssuer.CreateInvitationAsync(requestCreateInvitationInsuer);
                responseCreateInvitationInsuer.Should().NotBeNull(); // Assert

                // accept invitation as holder
                var requestAcceptInvitationMember = new AcceptInvitation
                {
                    Alias = $"{requestCreateTenantMember.Name}'s connection to {requestCreateTenantIssuer.Name}",
                    Use_existing_connection = true,
                    Invitation = new ReceiveInvitationRequest
                    {
                        AdditionalProperties = responseCreateInvitationInsuer.Invitation.AdditionalProperties,
                        Did = responseCreateInvitationInsuer.Invitation.Did,
                        Id = responseCreateInvitationInsuer.Invitation.Id,
                        ImageUrl = responseCreateInvitationInsuer.Invitation.ImageUrl,
                        Label = responseCreateInvitationInsuer.Invitation.Label,
                        RecipientKeys = responseCreateInvitationInsuer.Invitation.RecipientKeys,
                        RoutingKeys = responseCreateInvitationInsuer.Invitation.RoutingKeys,
                        ServiceEndpoint = responseCreateInvitationInsuer.Invitation.ServiceEndpoint,
                        Type = responseCreateInvitationInsuer.Invitation.Type
                    }
                };
                var responseAcceptInvitationMember = await clientTenantMember.AcceptInvitationAsync(requestAcceptInvitationMember);
                responseAcceptInvitationMember.Should().NotBeNull(); // Assert

                connectionIdIssuer = responseCreateInvitationInsuer.Connection_id;
                connectionIdMember = responseAcceptInvitationMember.Connection_id;
            }
            else
            {
                connectionIdIssuer = connectionsIssuer.Last().Connection_id;
                connectionIdMember = connectionsMember.Last().Connection_id;
            }

            #endregion Connections 

            #region Issue Credential

            // issue credential from issuer to member

            var responseSendCredential = await clientTenantIssuer.SendCredentialAsync(new SendCredential
            {
                Attributes = new Dictionary<string, string> { { "age", "age1" }, { "skill", "skill1" } },
                Connection_id = connectionIdIssuer,
                Credential_definition_id = credentialdefinitionId,
                Protocol_Version = IssueCredentialProtocolVersion.v1.ToString()
            });
            responseSendCredential.Should().NotBeNull(); // Assert

            var credentialId = responseSendCredential as string;

            // get credentials as member
            var credentials = await clientTenantMember.GetCredentialsAsync();
            credentials.Should().NotBeNull(); // Assert

            var credential = await clientTenantMember.GetCredentialAsync(credentialId);
            credential.Should().NotBeNull(); // Assert

            #endregion 
        }
    }
}