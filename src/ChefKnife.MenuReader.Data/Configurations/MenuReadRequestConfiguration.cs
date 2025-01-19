using ChefKnife.MenuReader.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.MenuReader.Data.Configurations;

public class ReadMenuRequestConfiguration : IEntityTypeConfiguration<ReadMenuRequest>
{
    public void Configure(EntityTypeBuilder<ReadMenuRequest> builder)
    {
        builder.ToContainer("ReadMenuRequests");

        builder.HasKey(x => x.id);

        builder.HasPartitionKey(x => x.MenuUri);

        builder.Property(x => x.id)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedDate);
        builder.Property(x => x.MenuUri)
            .IsRequired();
        builder.Property(x => x.StorageUri);
        builder.Property(x => x.ModelResult);
    }
}
