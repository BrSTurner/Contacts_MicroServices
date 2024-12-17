using FIAP.Contacts.IntegrationTests.Mock;
using FIAP.DatabaseManagement.Context;
using FIAP.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.Contacts.IntegrationTests.Database.ContextTests
{
    public class FIAPContextIntegrationTest
    {
        private readonly DbContextOptions<FIAPContext> _options;

        public FIAPContextIntegrationTest()
        {
            _options = new DbContextOptionsBuilder<FIAPContext>()
                        .UseSqlite("Filename=:memory:")
                        .Options;
        }

        [Fact(DisplayName = "Should Test Database Connection")]
        [Trait("Integration", "Database")]
        public async Task Should_Test_Database_Connection()
        {           
            using (var context = new FIAPContext(_options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();

                var canConnect = await context.Database.CanConnectAsync();

                Assert.True(canConnect);
            }
        }


        [Fact(DisplayName = "Should Insert In Database")]
        [Trait("Integration", "Database")]
        public async Task Should_Insert_In_Database()
        {
            using (var context = new FIAPContext(_options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();

                await context.Set<Contact>().AddAsync(ContactMock.ContactFaker.Generate(ContactMock.VALID_ENTITY));
                var rowsAffected = await context.SaveChangesAsync();

                Assert.Equal(1, rowsAffected);
            }
        }

        [Fact(DisplayName = "Should Delete In Database")]
        [Trait("Integration", "Database")]
        public async Task Should_Delete_In_Database()
        {
            using (var context = new FIAPContext(_options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                await SeedContactsInDatabase(context);

                var contacts = await context.Set<Contact>().ToListAsync();

                context.Set<Contact>().RemoveRange(contacts);
                var rowsAffected = await context.SaveChangesAsync();

                Assert.Equal(contacts.Count, rowsAffected);
            }
        }

        [Fact(DisplayName = "Should Get From Database")]
        [Trait("Integration", "Database")]
        public async Task Should_Get_From_Database()
        {
            using (var context = new FIAPContext(_options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                await SeedContactsInDatabase(context, 4);

                var contacts = await context.Set<Contact>().ToListAsync();

                Assert.Equal(4, contacts.Count);
            }
        }

        private async Task SeedContactsInDatabase(FIAPContext context, int quantity = 1)
        {
            await context.Set<Contact>().AddRangeAsync(ContactMock.ContactFaker.Generate(quantity, ContactMock.VALID_ENTITY));
            await context.SaveChangesAsync();
        }
    }
}
