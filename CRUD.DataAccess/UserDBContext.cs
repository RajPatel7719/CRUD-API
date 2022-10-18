using CRUD.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CRUD.DataAccess
{
    public partial class UserDBContext : IdentityDbContext<AppUser>
    {
        public UserDBContext()
        {
        }

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {
        }

        public virtual DbSet<User1> Users1 { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User1>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("First Name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Last Name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .HasColumnName("Phone Number")
                    .IsFixedLength();
            });

            base.OnModelCreating(builder);
        }
    }
}
