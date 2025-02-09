using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.IntegrationTests.Base;
using FIAP.Contacts.IntegrationTests.Mock;
using Shouldly;
using System.Net;
using System.Net.Http.Json;


namespace Contacts.IntegrationTests.Contacts.Endpoints
{
    public class RegistrationEndpointTests : IClassFixture<WebClientFixture<RegistrationProgram>>
    {
        private readonly WebClientFixture<RegistrationProgram> _fixture;

        public RegistrationEndpointTests(WebClientFixture<RegistrationProgram> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Create New Contact")]
        [Trait("Integration", "Create")]
        public async Task Should_Create_New_Contact_Returns_Created()
        {
            //Arrange
            var client = _fixture.Client;
            var newContact = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();
            var name = Guid.NewGuid();

            var input = new CreateContactInput
            {
                Email = newContact.Email.Address,
                Name = name.ToString(),
                PhoneCode = newContact.PhoneNumber.Code,
                PhoneNumber = newContact.PhoneNumber.Number
            };

            //Act
            var response = await client.PostAsJsonAsync("/api/contacts", input);
            var message = await response.Content.ReadFromJsonAsync<string>();

            //Assert
            response.EnsureSuccessStatusCode();


            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            message.ShouldBe("Contanct is being created...");
        }
    }
}
