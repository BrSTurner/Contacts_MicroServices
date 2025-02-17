using FIAP.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FIAP.DatabaseManagement.Contacts.Mapping
{
    public class ContactMapping : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
            );

            builder.ToTable("Contacts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.CreatedAt).HasConversion(dateTimeConverter);
            builder.Property(x => x.UpdatedAt).IsRequired(false).HasConversion(nullableDateTimeConverter);

            builder.OwnsOne(x => x.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Address)
                    .HasColumnName("Email")
                    .IsRequired();
            });

            builder.OwnsOne(x => x.PhoneNumber, phoneBuilder =>
            {
                phoneBuilder.Property(p => p.Code)
                    .HasColumnName("PhoneCode")
                    .IsRequired();

                phoneBuilder.Property(p => p.Number)
                    .HasColumnName("PhoneNumber")
                    .IsRequired();
            });
        }
    }
}
