namespace FIAP.Contacts.Application.Contacts.Models
{
    public record UpdateContactInput
    {
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string PhoneNumber { get; init; }
        public required int PhoneCode { get; init; }
    }
}
