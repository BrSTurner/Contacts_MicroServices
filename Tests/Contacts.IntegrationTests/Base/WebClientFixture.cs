using Dapper;
using FIAP.Contacts.IntegrationTests.Database.Dapper;
using FIAP.Contacts.IntegrationTests.Mock;
using FIAP.DatabaseManagement.Contacts.Queries;
using FIAP.DatabaseManagement.Contacts.Repositories;
using FIAP.DatabaseManagement.Context;
using FIAP.DatabaseManagement.UoW;
using FIAP.DatabaseManagement.WS.Contacts.Consumers;
using FIAP.SharedKernel.Entities;
using FIAP.SharedKernel.UoW;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace FIAP.Contacts.IntegrationTests.Base
{
    public class WebClientFixture<TProgram> : IDisposable where TProgram : class
    {
        public HttpClient Client { get; private set; }
        public IServiceScopeFactory ScopeFactory { get; private set; }

        private readonly WebApplicationFactory<TProgram> _factory;
        private readonly SqliteConnection _sqliteConnection;

        public WebClientFixture()
        {
            
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");
            _sqliteConnection.Open();

            _factory = new WebApplicationFactory<TProgram>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<FIAPContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    RegisterDatabase(services);
                    RegisterMassTransit(services);
                    RegisterDependencies(services);

                    var sp = services.BuildServiceProvider();
                    ScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<FIAPContext>();
                        db.Database.EnsureCreated();
                    }
                });
            });

            SqlMapper.AddTypeHandler(new GuidTypeHandler());

            Client = _factory.CreateClient();
        }

        private void RegisterDatabase(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<FIAPContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<DbConnection>(_ => _sqliteConnection);
            services.AddDbContext<FIAPContext>(options =>
            {
                options.UseSqlite(_sqliteConnection);
            });
        }

        private void RegisterMassTransit(IServiceCollection services)
        {
            var massTransitDescriptors = services
                .Where(d => d.ImplementationType?.Namespace?.Contains("MassTransit") == true)
                .ToList();

            foreach (var descriptor in massTransitDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<PersistContactConsumer>();
                x.AddConsumer<QueryByEmailOrPhoneConsumer>();
                x.AddConsumer<QueryByPhoneCodeConsumer>();
                x.AddConsumer<UpdateContactConsumer>();
                x.AddConsumer<QueryByIdConsumer>();
                x.AddConsumer<GetAllContactsConsumer>();
                x.AddConsumer<DeleteContactConsumer>();
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IContactQueries, ContactQueries>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public async Task<IEnumerable<Contact>> InsertContactsInDatabase(int quantity = 1)
        {
            if (quantity <= 0)
                return Enumerable.Empty<Contact>();

            var contacts = ContactMock.ContactFaker.Generate(quantity, ContactMock.VALID_ENTITY);

            return await InsertContactsInDatabase(contacts.ToArray());
        }

        public async Task<IEnumerable<Contact>> InsertContactsInDatabase(params Contact[] contacts)
        {
            if (contacts == null || !contacts.Any())
                return Enumerable.Empty<Contact>();


            using (var scope = ScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FIAPContext>();
                await db.AddRangeAsync(contacts);
                await db.SaveChangesAsync();
            }

            return contacts;
        }

        public async Task<int> CountContactsInDatabaseAsync()
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FIAPContext>();
                return await db.Set<Contact>().CountAsync();
            }
        }

        public async Task<Contact?> GetByIdInDatabaseAsync(Guid id)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FIAPContext>();
                return await db.Set<Contact>().FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<List<Contact>> GetAll()
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FIAPContext>();
                return await db.Set<Contact>().ToListAsync();
            }
        }


        public async Task ClearDatabase()
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<FIAPContext>();
                var allContacts = db.Set<Contact>().ToList();
                db.Set<Contact>().RemoveRange(allContacts); 
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            _sqliteConnection.Close();
            _factory?.Dispose();
        }
    }
}
