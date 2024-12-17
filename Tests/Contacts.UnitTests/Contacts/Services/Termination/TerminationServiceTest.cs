using FIAP.MessageBus;
using FIAP.Modification.Application.Commands;
using FIAP.Modification.Application.Handlers;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using FIAP.Termination.Application.Commands;
using FIAP.Termination.Application.Handlers;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.UnitTests.Contacts.Services.Termination
{
    public class TerminationServiceTest
    {
        private IMessageBus Bus = Substitute.For<IMessageBus>();

        [Fact(DisplayName = "Should Not Delete If Request Is Invalid")]
        [Trait("Unit", "Delete")]
        public async Task Should_Not_Delete_If_Request_Is_Invalid()
        {
            //Arrange
            var handler = CreateHandler();

            //Act
            var result = await handler.Handle(new DeleteContactCommand
            {
                ContactId = Guid.Empty
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
        }

        [Fact(DisplayName = "Should Not Delete If Contact was not found")]
        [Trait("Unit", "Delete")]
        public async Task Should_Not_Delete_If_Contact_Is_Not_Found()
        {
            //Arrange
            var handler = CreateHandler();
            var contact = ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY);
            Bus.RequestAsync<QueryContactByIdIntegrationEvent, QueryContactResponse>(Arg.Any<QueryContactByIdIntegrationEvent>())
                .Returns(Task.FromResult(new QueryContactResponse() { Contact = null }));

            //Act
            var result = await handler.Handle(new DeleteContactCommand
            {
                ContactId= contact.Id,
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldAllBe(x => x.ErrorMessage.Equals("Contact could not be found to be deleted", StringComparison.OrdinalIgnoreCase));
        }

        [Fact(DisplayName = "Should Delete")]
        [Trait("Unit", "Delete")]
        public async Task Should_Delete()
        {
            //Arrange
            var handler = CreateHandler();
            var contact = ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY);
            Bus.RequestAsync<QueryContactByIdIntegrationEvent, QueryContactResponse>(Arg.Any<QueryContactByIdIntegrationEvent>())
                .Returns(Task.FromResult(new QueryContactResponse() { Contact = contact }));

            //Act
            var result = await handler.Handle(new DeleteContactCommand
            {
                ContactId = contact.Id,
            }, CancellationToken.None);

            //Assert
            result.IsValid.ShouldBeTrue();
            await Bus.Received(1).PublishAsync(Arg.Any<DeleteContactIntegrationEvent>());
        }

        private DeleteContactCommandHandler CreateHandler()
        {
            Bus = Substitute.For<IMessageBus>();
            return new DeleteContactCommandHandler(Bus);

        }
    }
}
