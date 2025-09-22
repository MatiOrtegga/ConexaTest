using ConexaTest.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Tests
{
    public static class DbContextFactory
    {
        public static AppDbContext CreateInMemoryDbContext(string dbName = "TestDatabase")
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }
    }

}
