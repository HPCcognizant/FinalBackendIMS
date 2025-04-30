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

        public DatabaseDbContext(DbContextOptions<DatabaseDbContext> options): base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasKey(c => c.Customer_ID);
            modelBuilder.Entity<Agent>().HasKey(c => c.AgentID );
            modelBuilder.Entity<Policy>().HasKey(c => c.PolicyID );
            modelBuilder.Entity<Claim>().HasKey(c => c.ClaimID);
            modelBuilder.Entity<Notification>().HasKey(c => c.NotificationID );

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Customer)
                .WithMany(cu => cu.Claims)
                .HasForeignKey(c => c.Customer_ID);

            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Policy)
                .WithMany(p => p.Claims)
                .HasForeignKey(sc => sc.PolicyID);

            modelBuilder.Entity<CustomerPolicy>()
               .HasOne(c => c.Customer)
               .WithMany(cu => cu.CustomerPolicies)
               .HasForeignKey(c => c.Customer_ID);

            modelBuilder.Entity<CustomerPolicy>()
               .HasOne(c => c.Policy)
               .WithMany(cu => cu.CustomerPolicies)
               .HasForeignKey(c => c.PolicyID);

            modelBuilder.Entity<Notification>().HasOne(n => n.Customer)
                .WithMany(cu => cu.Notifications)
                .HasForeignKey(n => n.Customer_ID);

            modelBuilder.Entity<Policy>().HasOne(p => p.Agent)
                .WithMany(a => a.AssignedPolicies)
                .HasForeignKey(p => p.AgentID); 

        }
    }
}
