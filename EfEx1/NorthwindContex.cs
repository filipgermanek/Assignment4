using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace EfEx1
{
    public class NorthwindContex : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("host=localhost;db=northwind;uid=filipgermanek;pwd=GRuby123");
            // you only need this if you want to see the SQL statments created by EF
            optionsBuilder.UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            // Define composite key.
            modelBuilder.Entity<OrderDetails>()
                .HasKey(x => new { x.OrderId, x.ProductId });
        }

        // you only need this if you want to see the SQL statments created
        // by EF. See https://docs.microsoft.com/en-us/ef/core/miscellaneous/logging
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information, true)
            });
    }



    //CATEGORY CONFIG
    class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.Property(x => x.Id).HasColumnName("categoryid");
            builder.Property(x => x.CategoryName).HasColumnName("categoryname");
            builder.Property(x => x.CategoryDescription).HasColumnName("description");
    }
    }

    //PRODUCT CONFIG
    class ProductConfiguration : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.ToTable("products");
            builder.Property(x => x.Id).HasColumnName("productid");
            builder.Property(x => x.Name).HasColumnName("productname");
            builder.Property(x => x.CategoryId).HasColumnName("categoryid");
            builder.Property(x => x.UnitPrice).HasColumnName("unitprice");
            builder.Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
            builder.Property(x => x.UnitsInStock).HasColumnName("unitsinstock");
    }
    }

    //ORDERDETAILS CONFIG
    class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.ToTable("orderdetails");
            builder.Property(x => x.OrderId).HasColumnName("orderid");
            builder.Property(x => x.ProductId).HasColumnName("productid");
            builder.Property(x => x.UnitPrice).HasColumnName("unitprice");
            builder.Property(x => x.OrderQuantity).HasColumnName("quantity");
            builder.Property(x => x.OrderDiscount).HasColumnName("discount");
        }
    }

    //ORDER CONFIG
    class OrderConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.ToTable("orders");
            builder.Property(x => x.Id).HasColumnName("orderid");
            builder.Property(x => x.OrderDate).HasColumnName("orderdate");
            builder.Property(x => x.ShippedDate).HasColumnName("shippeddate");
            builder.Property(x => x.RequiredDate).HasColumnName("requireddate");
            builder.Property(x => x.Freight).HasColumnName("freight");
            builder.Property(x => x.ShipName).HasColumnName("shipname");
            builder.Property(x => x.ShipCity).HasColumnName("shipcity");
        }
    }

}
