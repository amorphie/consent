using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using amorphie.consent.core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace amorphie.consent.data;

class ConsentDbContextFactory : IDesignTimeDbContextFactory<ConsentDbContext>
{
    //lazy loading true
    //lazy loading false, eğer alt bileşenleri getirmek istiyorsak include kullanmamız lazım,eager loading
    private readonly IConfiguration _configuration;

    public ConsentDbContextFactory() { }

    public ConsentDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ConsentDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ConsentDbContext>();
        // var test = _configuration["STATE_STORE"];
        // System.Console.WriteLine("Test: " + test);
        // var connStr = _configuration["PostgreSql"];

        var connStr = "Host=localhost:5432;Database=ConsentDb;Username=postgres;Password=postgres";
        builder.UseNpgsql(connStr);
        builder.EnableSensitiveDataLogging();
        return new ConsentDbContext(builder.Options);
    }
}

public class ConsentDbContext : DbContext
{
    public ConsentDbContext(DbContextOptions<ConsentDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Consent>().HasIndex(c => c.SearchVector).HasMethod("GIN");
        modelBuilder.Entity<Consent>().Property(item => item.SearchVector)
        .HasComputedColumnSql(FullTextSearchHelper
        .GetTsVectorComputedColumnSql("english", new string[] { "State", "ConsentType", "AdditionalData" }), true);

        modelBuilder.Entity<Token>().HasIndex(c => c.SearchVector).HasMethod("GIN");
        modelBuilder.Entity<Token>().Property(item => item.SearchVector)
        .HasComputedColumnSql(FullTextSearchHelper
        .GetTsVectorComputedColumnSql("english", new string[] { "TokenValue", "TokenType" }), true);

modelBuilder.SeedOBErrorCodeDetailsCheckCustomer();
    }

    public DbSet<Consent> Consents { get; set; }
    public DbSet<Token> Tokens { get; set; }
    public DbSet<OBAccountConsentDetail> OBAccountConsentDetails { get; set; }
    public DbSet<OBPaymentConsentDetail> OBPaymentConsentDetails { get; set; }
    public DbSet<OBPaymentOrder> OBPaymentOrders { get; set; }
    public DbSet<OBYosInfo> OBYosInfos { get; set; }
    public DbSet<OBEvent> OBEvents { get; set; }
    public DbSet<OBSystemEvent> OBSystemEvents { get; set; }
    public DbSet<OBEventSubscription> OBEventSubscriptions { get; set; }
    public DbSet<OBEventSubscriptionType> OBEventSubscriptionTypes { get; set; }
    public DbSet<OBEventTypeSourceTypeRelation> OBEventTypeSourceTypeRelations { get; set; }
    public DbSet<OBHhsInfo> OBHhsInfos { get; set; }
    public DbSet<OBPermissionType> OBPermissionTypes { get; set; }
    public DbSet<OBErrorCodeDetail> OBErrorCodeDetails { get; set; }

}
