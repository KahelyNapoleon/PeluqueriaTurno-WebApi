using EfCoreAccessDataDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreAccessDataDemo.Data
{
    public class InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : DbContext(options)
    {
        public DbSet<Invoice> Invoices => Set<Invoice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id = Guid.NewGuid(),
                    InvoiceNumber = "INV-001",
                    ContactName = "IronMan",
                    Description = "Invoice for the first month",
                    Amount = 100.0m,
                    InvoiceDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    DueDate = new DateTimeOffset(2023, 1, 15, 0, 0, 0, TimeSpan.Zero),
                    Status = InvoiceStatus.AwaitPayment
                });
        }


    }
}
