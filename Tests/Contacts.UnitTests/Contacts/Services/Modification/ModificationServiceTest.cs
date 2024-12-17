using FIAP.MessageBus;
using FIAP.Modification.Application.Commands;
using FIAP.Modification.Application.Handlers;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using NSubstitute;
using Shouldly;

namespace Contacts.UnitTests.Contacts.Services.Modification
{
    public class ModificationServiceTest
    {

        private IMessageBus Bus = Substitute.For<IMessageBus>();

        [Fact(DisplayName = "Should Not Update If Request Is Invalid")]
        [Trait("Unit", "Update")]
        public async Task Should_Not_Update_If_Request_Is_Invalid()
        {
            //Arrange
            var handler = CreateHandler();

            //Act
            var result = await handler.Handle(new UpdateContactCommand
            {
                Id = Guid.Empty,
                Name = "",
                Email = "",
                PhoneCode = 3,
                PhoneNumber = "879196273"
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(6);
        }

        [Fact(DisplayName = "Should Not Update If Contact was not found")]
        [Trait("Unit", "Update")]
        public async Task Should_Not_Update_If_Contact_Is_Not_Found()
        {
            //Arrange
            var handler = CreateHandler();
            var contact = ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY);
            Bus.RequestAsync<QueryContactByIdIntegrationEvent, QueryContactResponse>(Arg.Any<QueryContactByIdIntegrationEvent>())
                .Returns(Task.FromResult(new QueryContactResponse() { Contact = null }));

            //Act
            var result = await handler.Handle(new UpdateContactCommand
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email.Address,
                PhoneCode = contact.PhoneNumber.Code,
                PhoneNumber = contact.PhoneNumber.Number
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldAllBe(x => x.ErrorMessage.Equals("Contact could not be found to be updated", StringComparison.OrdinalIgnoreCase));
        }

        [Fact(DisplayName = "Should Update")]
        [Trait("Unit", "Update")]
        public async Task Should_Update()
        {
            //Arrange
            var handler = CreateHandler();
            var contact = ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY);
            Bus.RequestAsync<QueryContactByIdIntegrationEvent, QueryContactResponse>(Arg.Any<QueryContactByIdIntegrationEvent>())
                .Returns(Task.FromResult(new QueryContactResponse() { Contact = contact }));

            //Act
            var result = await handler.Handle(new UpdateContactCommand
            {
                Id = contact.Id,
                Name = "Bruno",
                Email = contact.Email.Address,
                PhoneCode = contact.PhoneNumber.Code,
                PhoneNumber = contact.PhoneNumber.Number
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeTrue();
            await Bus.Received(1).PublishAsync(Arg.Any<UpdateContactIntegrationEvent>());
        }

        private UpdateContactCommandHandler CreateHandler()
        {
            Bus = Substitute.For<IMessageBus>();
            return new UpdateContactCommandHandler(Bus);

        }
    }
}
