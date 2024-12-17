using FIAP.MessageBus;
using FIAP.Registration.Application.Commands;
using FIAP.Registration.Application.Handlers;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using NSubstitute;
using Shouldly;

namespace Contacts.UnitTests.Contacts.Services.Registration
{
    public class RegistrationServiceTest
    {
        private IMessageBus Bus = Substitute.For<IMessageBus>();

        [Fact(DisplayName = "Should Not Create And Throw Null Exception")]
        [Trait("Unit", "Create")]
        public async Task Should_Not_Create_And_Throw_Null_Exception()
        {
            //Arrange
            var handler = CreateHandler();

            //Act && Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
            Assert.Contains("Contact must be correctly filled", exception.Message);
        }


        [Fact(DisplayName = "Should Not Create If Request Is Invalid")]
        [Trait("Unit", "Create")]
        public async Task Should_Not_Create_If_Request_Is_Invalid()
        {
            //Arrange
            var handler = CreateHandler();

            //Act
            var result = await handler.Handle(new CreateContactCommand
            {
                Name = "",
                Email = "",
                PhoneCode = 3,
                PhoneNumber = "879196273"
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(5);
        }

        [Fact(DisplayName = "Should Not Create If Contact Already")]
        [Trait("Unit", "Create")]
        public async Task Should_Not_Create_If_Already_Exists()
        {
            //Arrange
            var handler = CreateHandler();
            var contact = ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY);

            Bus.RequestAsync<QueryContactByEmailOrPhoneIntegrationEvent, QueryContactResponse>(Arg.Any<QueryContactByEmailOrPhoneIntegrationEvent>())
               .Returns(Task.FromResult(new QueryContactResponse { Contact = contact }));

            //Act
            var result = await handler.Handle(new CreateContactCommand
            {
                Name = contact.Name,
                Email = contact.Email.Address,
                PhoneCode = contact.PhoneNumber.Code,
                PhoneNumber = contact.PhoneNumber.Number
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldAllBe(x => x.ErrorMessage.Equals("A Contact with the same E-mail or Phone Number was already created", StringComparison.OrdinalIgnoreCase));
        }

        [Fact(DisplayName = "Should Create")]
        [Trait("Unit", "Create")]
        public async Task Should_Create()
        {
            //Arrange
            var handler = CreateHandler();
            var contact = ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY);

            Bus.RequestAsync<QueryContactByEmailOrPhoneIntegrationEvent, QueryContactResponse>(Arg.Any<QueryContactByEmailOrPhoneIntegrationEvent>())
               .Returns(Task.FromResult(new QueryContactResponse { Contact = null }));

            //Act
            var result = await handler.Handle(new CreateContactCommand
            {
                Name = contact.Name,
                Email = contact.Email.Address,
                PhoneCode = contact.PhoneNumber.Code,
                PhoneNumber = contact.PhoneNumber.Number
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeTrue();
            await Bus.Received(1).PublishAsync(Arg.Any<CreateContactIntegrationEvent>());
        }

        private CreateContactCommandHandler CreateHandler()
        {
            Bus = Substitute.For<IMessageBus>();
            return new CreateContactCommandHandler(Bus);

        }

    }
}
