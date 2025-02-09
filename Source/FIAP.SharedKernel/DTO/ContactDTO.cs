namespace FIAP.SharedKernel.DTO
{
    public record ContactDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public int PhoneCode { get; set; }

    }
}
