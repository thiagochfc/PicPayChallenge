using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PicPayChallenge.Models;

namespace PicPayChallenge.Infrastructure.EntityConfigurations;

public class AccountEntityTypeConfiguration
    : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("accounts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired().HasColumnType("uuid ").HasColumnName("id");

        builder.Property(x => x.FullName).IsRequired().HasColumnType("VARCHAR(150)").HasColumnName("fullName");

        builder.Property(x => x.CpfCnpj).IsRequired().HasColumnType("VARCHAR(14)").HasColumnName("cpfCnpj");

        builder.Property(x => x.Email).IsRequired().HasColumnType("VARCHAR(150)").HasColumnName("email");

        builder.Property(x => x.Password).IsRequired().HasColumnType("VARCHAR(150)").HasColumnName("password");

        builder.Property(x => x.Type).IsRequired().HasColumnType("VARCHAR(11)").HasConversion<string>().HasColumnName("type");
        
        builder.Property(x => x.Balance).IsRequired().HasColumnType("NUMERIC(13, 2)").HasColumnName("balance");
    }
}
