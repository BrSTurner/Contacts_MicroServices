using Bogus;
using FIAP.Inquiry.Application.Commands;
using FIAP.Inquiry.Application.Handlers;
using FIAP.MessageBus;
using FIAP.Modification.Application.Commands;
using FIAP.Modification.Application.Handlers;
using FIAP.SharedKernel.DTO;
using FIAP.SharedKernel.Messages.Integration.Events;
using FIAP.SharedKernel.Messages.Integration.Responses;
using MassTransit;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.UnitTests.Contacts.Services.Inquiry
{
    public class InquiryServiceTest
    {

        private IMessageBus Bus = Substitute.For<IMessageBus>();


        [Fact(DisplayName = "Get All Contacts")]
        [Trait("Unit", "Get")]
        public async Task Get_All_Contacts()
        {
            //Arrange
            var handler = CreateAllContactsHandler();
            var contacts = Enumerable.Repeat(ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY), 5);
            Bus.RequestAsync<QueryAllContactsIntegrationEvent, QueryContactsResponse>(Arg.Any<QueryAllContactsIntegrationEvent>())
               .Returns(Task.FromResult(new QueryContactsResponse { Contacts = contacts.ToList() }));


            //Act
            var results = await handler.Handle(new InquiryAllContactsCommand(), CancellationToken.None);

            //Assert
            results.ShouldNotBeEmpty();
            results.Count.ShouldBe(5);
        }

        [Fact(DisplayName = "Get All Contacts Returns Empty")]
        [Trait("Unit", "Get")]
        public async Task Get_All_Contacts_Returns_Empty()
        {
            //Arrange
            var handler = CreateAllContactsHandler();
            Bus.RequestAsync<QueryAllContactsIntegrationEvent, QueryContactsResponse>(Arg.Any<QueryAllContactsIntegrationEvent>())
               .Returns(Task.FromResult(new QueryContactsResponse { Contacts = [] }));


            //Act
            var results = await handler.Handle(new InquiryAllContactsCommand(), CancellationToken.None);

            //Assert
            results.ShouldBeEmpty();
        }


        [Fact(DisplayName = "Get Contact by phone code")]
        [Trait("Unit", "Get")]
        public async Task Get_Contact_By_Phone_Code()
        {
            //Arrange
            var handler = CreateContactByPhoneCodeHandler();
            var contacts = Enumerable.Repeat(ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY), 5);
            Bus.RequestAsync<QueryContactByPhoneCodeIntegrationEvent, QueryContactByPhoneCodeResponse>(Arg.Any<QueryContactByPhoneCodeIntegrationEvent>())
               .Returns(Task.FromResult(new QueryContactByPhoneCodeResponse
               {
                   Contacts = contacts.Select(x => new ContactDTO
                   {
                       Email = x.Email.Address,
                       Name = x.Name,
                       PhoneNumber = x.PhoneNumber.Number,
                       PhoneCode = x.PhoneNumber.Code,
                       Id = x.Id
                   })
                   .ToList()
               }));


            //Act
            var results = await handler.Handle(new InquiryContactByPhoneCodeCommand { PhoneCode = contacts.First().PhoneNumber.Code }, CancellationToken.None);

            //Assert
            results.ShouldNotBeEmpty();
            results.Count.ShouldBeGreaterThanOrEqualTo(1);
        }

        [Fact(DisplayName = "Get Contact by phone code when request is invalid")]
        [Trait("Unit", "Get")]
        public async Task Get_Contact_By_Phone_Code_Returns_Empty()
        {
            //Arrange
            var handler = CreateContactByPhoneCodeHandler();

            //Act
            var results = await handler.Handle(new InquiryContactByPhoneCodeCommand { PhoneCode = 100 }, CancellationToken.None);

            //Assert
            results.ShouldBeEmpty();
        }

        private InquiryAllContactsCommandHandler CreateAllContactsHandler()
        {
            Bus = Substitute.For<IMessageBus>();
            return new InquiryAllContactsCommandHandler(Bus);

        }

        private InquiryContactByPhoneCodeCommandHandler CreateContactByPhoneCodeHandler()
        {
            Bus = Substitute.For<IMessageBus>();
            return new InquiryContactByPhoneCodeCommandHandler(Bus);

        }
    }
}
