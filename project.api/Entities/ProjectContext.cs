using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace project.api.Entities
{
    public class ProjectContext : IdentityDbContext<
        User,
        Role,
        Guid,
        IdentityUserClaim<Guid>,
        UserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (modelBuilder == null) { throw new ArgumentNullException(nameof(modelBuilder)); }

            modelBuilder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();
            });

            modelBuilder.Entity<Order>(x =>
            {
                x.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
            });

            modelBuilder.Entity<RefreshToken>(x =>
            {
                x.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
            });

            modelBuilder.Entity<OrderProduct>(x =>
            {
                x.HasOne(x => x.Order)
                .WithMany(x => x.OrderProducts)
                .HasForeignKey(x => x.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

                x.HasOne(x => x.Product)
                .WithMany(x => x.OrderProducts)
                .HasForeignKey(x => x.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(x =>
            {
                x.HasOne(x => x.Company)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CompanyId)
                .IsRequired();

                x.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired();

            });

            modelBuilder.Entity<Image>(x =>
            {
                x.HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .IsRequired(false);
            });

            modelBuilder.Entity<Category>(x =>
            {
                x.HasMany(x => x.SubCategories)
                .WithOne(x => x.ParentCategory)
                .HasForeignKey(x => x.ParentId);
            });

            modelBuilder.Entity<Address>(o =>
            {
                o.HasOne(o => o.User)
                .WithOne(o => o.Address)
                .HasForeignKey<User>(o => o.AddressId)
                .IsRequired(false);

                o.HasOne(o => o.Company)
                .WithOne(o => o.Address)
                .HasForeignKey<Company>(o => o.AddressId)
                .IsRequired(false);
            });

            modelBuilder.Entity<Category>(x =>
             {
                 x.HasIndex(x => new { x.Name, x.ParentId })
                 .IsUnique(true)
                 .HasName("Un_Category_Name_ParentId");
             });

            modelBuilder.Entity<Company>(x =>
            {
                x.HasIndex(x => new { x.Name, x.Email, x.AccountNumber })
                .IsUnique(true)
                .HasName("Un_Company_Name_Email_AccountNumber");
            });

            modelBuilder.Entity<Product>(x =>
            {
                x.HasIndex(x => new { x.Name, x.Description })
                .IsUnique(true)
                .HasName("Un_Product_Name_Description");
            });

            modelBuilder.Entity<Product>(x =>
            {
                x.HasIndex(x => new { x.Name, x.Description })
                .IsUnique(true)
                .HasName("Un_Product_Name_Description");
            });

            modelBuilder.Entity<OrderProduct>(x =>
            {
                x.HasIndex(x => new { x.ProductId, x.OrderId })
                .IsUnique(true)
                .HasName("Un_OrderProduct_ProductId_OrderId");
            });

            modelBuilder.Entity<Image>(x =>
            {
                x.HasIndex(x => new {  x.ProductId, x.Name, x.Extension })
                .IsUnique(true)
                .HasName("Un_Image_BoekId_Name_Extension");
            });
        }
    }
}
