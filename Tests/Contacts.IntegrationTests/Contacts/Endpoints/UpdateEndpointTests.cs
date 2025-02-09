using FIAP.Contacts.Application.Contacts.Models;
using FIAP.Contacts.IntegrationTests.Base;
using FIAP.Contacts.IntegrationTests.Mock;
using Shouldly;
using System.Net;
using System.Net.Http.Json;


namespace Contacts.IntegrationTests.Contacts.Endpoints
{
    public class UpdateEndpointTests : IClassFixture<WebClientFixture<UpdateProgram>>
    {
        private readonly WebClientFixture<UpdateProgram> _fixture;

        public UpdateEndpointTests(WebClientFixture<UpdateProgram> fixture)
        {
            _fixture = fixture;
        }

        [Theory(DisplayName = "Update Contact")]
        [Trait("Integration", "Update")]
        [InlineData("Gustavo Koz0noe")]
        public async Task Should_Update_Contact_Returns_Ok(string expectedName)
        {
            //Arrange
            var client = _fixture.Client;
            var contactToUpdate = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            await _fixture.InsertContactsInDatabase(contactToUpdate);

            var input = new UpdateContactInput
            {
                Name = expectedName,
                Email = contactToUpdate.Email.Address,
                PhoneCode = contactToUpdate.PhoneNumber.Code,
                PhoneNumber = contactToUpdate.PhoneNumber.Number
            };

            //Act
            var response = await client.PutAsJsonAsync($"/api/contacts/{contactToUpdate.Id}", input);

            var message = await response.Content.ReadFromJsonAsync<string>();

            //Assert
            response.EnsureSuccessStatusCode();


            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            message.ShouldBe("Contact is being updated...");
        }

    }
}
