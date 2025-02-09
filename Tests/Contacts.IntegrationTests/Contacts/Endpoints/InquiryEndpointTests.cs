using FIAP.Contacts.IntegrationTests.Base;
using FIAP.Contacts.IntegrationTests.Mock;
using FIAP.SharedKernel.DTO;
using System.Net;
using System.Net.Http.Json;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Contacts.IntegrationTests.Contacts.Endpoints
{
    public class InquiryEndpointTests : IClassFixture<WebClientFixture<InquiryProgram>>
    {
        private readonly WebClientFixture<InquiryProgram> _fixture;

        public InquiryEndpointTests(WebClientFixture<InquiryProgram> fixture)
        {
            _fixture = fixture;
        }


        [Fact(DisplayName = "Get All Contacts")]
        [Trait("Integration", "Get")]
        public async Task Get_All_Contacts_Returns_Ok()
        {
            //Arrange
            await _fixture.InsertContactsInDatabase(2);
            var client = _fixture.Client;

            //Act
            var response = await client.GetAsync("/api/contacts");

            //Assert
            response.EnsureSuccessStatusCode();

            var contacts = await response.Content.ReadFromJsonAsync<List<ContactDTO>>();
            var contactsInDbAmount = await _fixture.CountContactsInDatabaseAsync();

            Assert.NotNull(contacts);
            Assert.Equal(contactsInDbAmount, contacts.Count);
        }

        [Fact(DisplayName = "Should Not Get All Contacts")]
        [Trait("Integration", "Get")]
        public async Task Get_No_Contacts_Returns_No_Content()
        {
            //Arrange
            await _fixture.ClearDatabase();
            var client = _fixture.Client;

            //Act
            var response = await client.GetAsync("/api/contacts");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory(DisplayName = "Get Contact By Phone Code")]
        [Trait("Integration", "Get")]
        [InlineData(11)]
        [InlineData(19)]
        [InlineData(21)]
        [InlineData(44)]
        public async Task Get_Contact_By_PhoneCode_Returns_Ok(int phoneCode)
        {
            //Arrange
            var client = _fixture.Client;
            var contactInDatabase = ContactMock
                    .GenerateContactByPhoneCode(phoneCode)
                    .Generate(ContactMock.CUSTOM_PHONE_VALID_ENTITY);

            await _fixture
                    .InsertContactsInDatabase(contactInDatabase);

            //Act
            var response = await client.GetAsync($"/api/contacts/{phoneCode}");

            //Assert
            response.EnsureSuccessStatusCode();

            var contacts = await response.Content.ReadFromJsonAsync<List<ContactDTO>>();

            Assert.NotNull(contacts);
            Assert.NotNull(contacts.FirstOrDefault(x => x.Id == contactInDatabase.Id));
            Assert.True(contacts.All(x => x.PhoneCode == phoneCode));
        }

        [Fact(DisplayName = "Should Not Get Contact By Phone Code ")]
        [Trait("Integration", "Get")]
        public async Task Get_Contact_By_PhoneCode_Returns_No_Content()
        {
            //Arrange
            var client = _fixture.Client;
            await _fixture.ClearDatabase();

            //Act
            var response = await client.GetAsync($"/api/contacts/11");

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

    }
}
