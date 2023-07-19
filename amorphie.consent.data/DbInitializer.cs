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
        var consentDefinition = new ConsentDefinition[]{
            new ConsentDefinition{
                Id = Guid.NewGuid(),
                Name = "Consent 1",
                RoleAssignment = "Role 1",
                Scope = new string[]{"Scope 1"},
                ClientId = new string[]{"Client 1"}
            },
            new ConsentDefinition{
                Id = Guid.NewGuid(),
                Name = "Consent 2",
                RoleAssignment = "Role 2",
                Scope = new string[]{"Scope 2"},
                ClientId = new string[]{"Client 2"}
            },
        };
        foreach (ConsentDefinition c in consentDefinition)
        {
            context.ConsentDefinitions.Add(c);
        }
        context.SaveChanges();

        var consent = new Consent[]{
            new Consent{
                // Id = Guid.NewGuid(),
                ConsentDefinitionId = consentDefinition[0].Id,
                UserId = Guid.NewGuid(),
                State = 1,
                ConsentType = 1,
                AdditionalData = "Additional Data 1"
            },
            new Consent{
                // Id = Guid.NewGuid(),
                ConsentDefinitionId = consentDefinition[1].Id,
                UserId = Guid.NewGuid(),
                State = 2,
                ConsentType = 2,
                AdditionalData = "Additional Data 2"
            },
        };

        foreach (Consent c in consent)
        {
            context.Consents.Add(c);
        }
        context.SaveChanges();

        var consentPermission = new ConsentPermission[]{
            new ConsentPermission{
                ConsentId = consent[0].Id,
                Permission = "Permission 1"
            },
            new ConsentPermission{
                ConsentId = consent[1].Id,
                Permission = "Permission 2"
            },
        };

        foreach (ConsentPermission c in consentPermission)
        {
            context.ConsentPermissions.Add(c);
        }
        context.SaveChanges();

        var token = new Token[]{
            new Token{
                ConsentId = consent[0].Id,
                TokenValue = "Token Value 1",
                TokenType = 1,
                ExpireTime = 5

            }
        };

        foreach (Token t in token)
        {
            context.Tokens.Add(t);
        }
        context.SaveChanges();


    }
}
