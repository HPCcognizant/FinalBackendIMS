using InsuranceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementSystem.Data
{
    public class DatabaseDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CustomerPolicy> CustomerPolicies { get; set; } 
        public DbSet<User> Users { get; set; }

        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options): base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Customer>().HasKey(c => c.Customer_ID);
            modelBuilder.Entity<Agent>().HasKey(c => c.AgentID);
            modelBuilder.Entity<Policy>().HasKey(c => c.PolicyID);
            modelBuilder.Entity<Claim>().HasKey(c => c.ClaimID);
            modelBuilder.Entity<Notification>().HasKey(c => c.NotificationID);
            modelBuilder.Entity<CustomerPolicy>().HasKey(c => c.CustomerPolicy_ID);

            // User Relationships
            modelBuilder.Entity<Customer>()
             .HasOne(c => c.User)
             .WithOne(u => u.Customer)
             .HasForeignKey<Customer>(c => c.UserId);

            modelBuilder.Entity<Agent>()
             .HasOne(a => a.User)
             .WithOne(u => u.Agent)
             .HasForeignKey<Agent>(a => a.UserId);

            modelBuilder.Entity<Admin>()
             .HasOne(a => a.User)
             .WithOne(u => u.Admin)
             .HasForeignKey<Admin>(a => a.UserId);

            // Claim Relationships

            modelBuilder.Entity<Claim>()
             .HasOne(c => c.Customer)
             .WithMany(cu => cu.Claims)
             .HasForeignKey(c => c.Customer_ID)
             .OnDelete(DeleteBehavior.Restrict); // Specify ON DELETE NO ACTION

            modelBuilder.Entity<Claim>()
             .HasOne(c => c.Policy)
             .WithMany(p => p.Claims)
             .HasForeignKey(c => c.PolicyID)
             .OnDelete(DeleteBehavior.Restrict);


            // CustomerPolicy Relationships
            modelBuilder.Entity<CustomerPolicy>()
             .HasOne(cp => cp.Customer)
             .WithMany(cu => cu.CustomerPolicies)
             .HasForeignKey(cp => cp.Customer_ID);

            modelBuilder.Entity<CustomerPolicy>()
             .HasOne(cp => cp.Policy)
             .WithMany(p => p.CustomerPolicies)
             .HasForeignKey(cp => cp.PolicyID);

            // Notification Relationships
            modelBuilder.Entity<Notification>()
             .HasOne(n => n.Customer)
             .WithMany(cu => cu.Notifications)
             .HasForeignKey(n => n.Customer_ID);

            // Policy Relationships
            modelBuilder.Entity<Policy>()
             .HasOne(p => p.Agent)
             .WithMany(a => a.AssignedPolicies)
             .HasForeignKey(p => p.AgentID)
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
