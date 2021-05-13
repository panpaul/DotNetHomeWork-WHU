using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;

namespace OrderView
{
    public class OrderContext : DbContext
    {
        public OrderContext()
            : base("name=OrderModel")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<OrderContext, ContextMigrationConfiguration>(true));
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }

    internal sealed class ContextMigrationConfiguration : DbMigrationsConfiguration<OrderContext>
    {
        public ContextMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }
    }
}