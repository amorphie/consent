using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.consent.core.Model;

namespace amorphie.consent.data;

public static class DbInitializer
{
    public static void Initialize(ConsentDbContext context)
    {
        context.Database.EnsureCreated();

        // Look for any students.
        if (context.Consents.Any())
        {
            return; // DB has been seeded
        }

    }
}
