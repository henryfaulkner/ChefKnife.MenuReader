using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ChefKnife.MenuReader.Data.Entities;

namespace ChefKnife.MenuReader.Data;

public class MenuReaderDbContext : DbContext
{
    public MenuReaderDbContext(DbContextOptions<MenuReaderDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*
         * Create objects like Sql Sequence / Functions, if any
         */

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Need to store all DateTimeOffsets as UTC offset 0 for PostgreSQL compatibility
        // See:
        // https://stackoverflow.com/questions/29127128/how-to-store-datetimeoffset-in-postresql
        // https://www.npgsql.org/doc/types/datetime.html
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetConverter>();
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}