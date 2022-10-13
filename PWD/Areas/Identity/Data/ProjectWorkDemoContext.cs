using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectWorkDemo.Areas.Identity.Data;

namespace ProjectWorkDemo.Data;

public class ProjectWorkDemoContext : IdentityDbContext<ProjectWorkDemoUser>
{
    public ProjectWorkDemoContext(DbContextOptions<ProjectWorkDemoContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ProjectWorkDemoUserEntityConfiguration());
    }
}

public class ProjectWorkDemoUserEntityConfiguration : IEntityTypeConfiguration<ProjectWorkDemoUser>
{
    public void Configure(EntityTypeBuilder<ProjectWorkDemoUser> builder)
    {
        builder.Property(u => u.Nome).IsRequired();
        builder.Property(u => u.Cognome).IsRequired();
        builder.Property(u => u.Telefono).IsRequired();
    }
}