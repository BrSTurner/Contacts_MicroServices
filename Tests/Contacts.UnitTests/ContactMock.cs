using Bogus;
using FIAP.SharedKernel.Constants;
using FIAP.SharedKernel.DomainObjects;
using FIAP.SharedKernel.Entities;

namespace Contacts.UnitTests
{
    public static class ContactMock
    {
        public const string VALID_ENTITY = "Valid";
        public const string VALID_SPECIFIC_PHONECODE_ENTITY = "Valid_Specific_PhoneCode";
        public const int SPECIFIC_PHONE_CODE = 11;

        public static Faker<Contact> ContactFaker = new Faker<Contact>()
            .RuleSet(VALID_ENTITY, r =>
            {
                r.CustomInstantiator(c => new Contact(
                    c.Name.FullName(),
                    new Email(c.Internet.Email()),
                    new PhoneNumber(
                    c.PickRandom(PhoneCodes.ValidCodes.Values.SelectMany(x => x).ToList()),
                    c.Random.Number(900000000, 999999999).ToString())))
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.CreatedAt, f => f.Date.Recent());
            });
    }
}