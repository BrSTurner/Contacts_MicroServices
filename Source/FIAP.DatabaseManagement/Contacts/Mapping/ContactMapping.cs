using FIAP.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.DatabaseManagement.Contacts.Mapping
{
    public class ContactMapping : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.UpdatedAt);

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
