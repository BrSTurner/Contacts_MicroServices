using FIAP.Contacts.IntegrationTests.Base;
using FIAP.Contacts.IntegrationTests.Mock;
using Shouldly;
using System.Net;
using System.Net.Http.Json;


namespace Contacts.IntegrationTests.Contacts.Endpoints
{
    public class TerminationEndpointTests : IClassFixture<WebClientFixture<TerminationProgram>>
    {
        private readonly WebClientFixture<TerminationProgram> _fixture;

        public TerminationEndpointTests(WebClientFixture<TerminationProgram> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Delete Contact")]
        [Trait("Integration", "Delete")]
        public async Task Should_Delete_Contact_Returns_Ok()
        {
            //Arrange
            var client = _fixture.Client;
            var contactToUpdate = ContactMock.ContactFaker
                .Generate(1, ContactMock.VALID_ENTITY)
                .FirstOrDefault();

            await _fixture.InsertContactsInDatabase(contactToUpdate);

            //Act
            var response = await client.DeleteAsync($"/api/contacts/{contactToUpdate.Id}");
            var message = await response.Content.ReadFromJsonAsync<string>();

            //Assert
            response.EnsureSuccessStatusCode();


            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            message.ShouldBe("Contanct is being deleted...");
        }
    }
}
