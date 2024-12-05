using FIAP.DatabaseManagement.Contacts.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FIAP.DatabaseManagement.Context
{
    public class FIAPContext : DbContext
    {
        public FIAPContext(DbContextOptions options) : base(options)
        {
        }

        public DbConnection DbConnection
        {
            get
            {
                return Database.GetDbConnection();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ContactMapping());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("FiapContacts");
            }
        }

    }
}
