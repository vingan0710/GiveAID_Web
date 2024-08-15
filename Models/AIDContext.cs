using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models
{
    public class AIDContext : DbContext
    {
        public AIDContext() { }
        public AIDContext(DbContextOptions<AIDContext> options) : base(options) { }
        public AIDContext(string connectionString) : base(GetOptions(connectionString)) { }

        private static DbContextOptions GetOptions(string connectionString)
        {
            throw new NotImplementedException();
        }

        //#region model
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<Member>? Members { get; set; }
        public DbSet<Topic>? Topics { get; set; }
        public DbSet<Donation>? Donations { get; set; }
        public DbSet<Organization_program>? Organization_Programs { get; set; }
        public DbSet<Gallery>? Galleries { get; set; }
        public DbSet<Partnership>? Partnerships { get; set; }
        public DbSet<Payment>? Payment { get; set; }
        public DbSet<Contact>? Contacts { get; set; }
        //#endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-Q9E21TFO\\MEOMEO;Initial Catalog=GiveAID;Persist Security Info=True;User ID=sa;Password=1234;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contact>()
                .Property(s => s.created_at)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
