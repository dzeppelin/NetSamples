using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication5.DatabaseLayer;

namespace TestProject.Stubs;

public class PostgresContextStub : PostgresContext
{
    // We make a call to the base contructor
    // Constructor of the parent class
    // If we do that, base constructor is always first to execute
    public PostgresContextStub(DbContextOptions<PostgresContext> options) : base(options)
    {
        // We have a problem with our stubclass, when
        // a subclass gets instantiated, and the super class
        // already exists, it does not get instantiated twice
        // So our stub context has all the unneccessary objects from
        // tests above it, this ensures the db is empty
        base.Database.EnsureDeleted();
    }

    // The SaveChangesAsync method returns (in a non-stubbed scenario)
    // the number of entries written to the database 
    // We only write a single entry in tests so that's why it's 1
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Accessing pending AcademicClass object changes to the database
        IEnumerable<EntityEntry> pendingChanges = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);
        IEnumerable<AcademicClass> academicClasses = pendingChanges.Select(e => e.Entity).OfType<AcademicClass>();


        if (academicClasses.Any(a => a.Id != 1))
        {
            throw new Exception("Database error!");
        }

        // Here we call non-stubbed SaveChangesAsync
        await base.SaveChangesAsync(cancellationToken);
        return 1;
    }
}
