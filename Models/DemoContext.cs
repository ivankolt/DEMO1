using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Den.Models;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Manufacture> Manufactures { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrdersProduct> OrdersProducts { get; set; }

    public virtual DbSet<OrdersStatus> OrdersStatuses { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsType> ProductsTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=DEMO;Username=postgres;Password=12345;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("gender_pkey");

            entity.ToTable("gender");

            entity.HasIndex(e => e.Name, "gender_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Manufacture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufactures_pkey");

            entity.ToTable("manufactures");

            entity.HasIndex(e => e.Name, "manufactures_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasColumnType("character varying")
                .HasColumnName("code");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.OrdersDate).HasColumnName("orders_date");
            entity.Property(e => e.PickuppointId).HasColumnName("pickuppoint_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UsersId).HasColumnName("users_id");

            entity.HasOne(d => d.Pickuppoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickuppointId)
                .HasConstraintName("orders_pickuppoint_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("orders_status_id_fkey");

            entity.HasOne(d => d.Users).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("orders_users_id_fkey");
        });

        modelBuilder.Entity<OrdersProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_products_pkey");

            entity.ToTable("orders_products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrdersId).HasColumnName("orders_id");
            entity.Property(e => e.ProductsId)
                .HasColumnType("character varying")
                .HasColumnName("products_id");
            entity.Property(e => e.Qty).HasColumnName("qty");

            entity.HasOne(d => d.Orders).WithMany(p => p.OrdersProducts)
                .HasForeignKey(d => d.OrdersId)
                .HasConstraintName("orders_products_orders_id_fkey");

            entity.HasOne(d => d.Products).WithMany(p => p.OrdersProducts)
                .HasForeignKey(d => d.ProductsId)
                .HasConstraintName("orders_products_products_id_fkey");
        });

        modelBuilder.Entity<OrdersStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_status_pkey");

            entity.ToTable("orders_status");

            entity.HasIndex(e => e.Name, "orders_status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("persons_pkey");

            entity.ToTable("persons");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasColumnType("character varying")
                .HasColumnName("middle_name");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pickuppoint_pkey");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Articule).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Articule)
                .HasMaxLength(100)
                .HasColumnName("articule");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.ImgPth)
                .HasMaxLength(100)
                .HasColumnName("img_pth");
            entity.Property(e => e.ManufacturesId).HasColumnName("manufactures_id");
            entity.Property(e => e.Price)
                .HasPrecision(12, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductsTypeId).HasColumnName("products_type_id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.SuppliersId).HasColumnName("suppliers_id");
            entity.Property(e => e.Unit)
                .HasMaxLength(100)
                .HasColumnName("unit");

            entity.HasOne(d => d.Gender).WithMany(p => p.Products)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("products_gender_id_fkey");

            entity.HasOne(d => d.Manufactures).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturesId)
                .HasConstraintName("products_manufactures_id_fkey");

            entity.HasOne(d => d.Suppliers).WithMany(p => p.Products)
                .HasForeignKey(d => d.SuppliersId)
                .HasConstraintName("products_suppliers_id_fkey");
        });

        modelBuilder.Entity<ProductsType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_type_pkey");

            entity.ToTable("products_type");

            entity.HasIndex(e => e.Name, "products_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.HasIndex(e => e.Name, "suppliers_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.PersonsId).HasColumnName("persons_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Persons).WithMany(p => p.Users)
                .HasForeignKey(d => d.PersonsId)
                .HasConstraintName("users_persons_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
