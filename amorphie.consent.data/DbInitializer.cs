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
                State = "Waiting",
                ConsentType = "Open Banking",
                AdditionalData = "Additional Data 1",
                Description = "Description 1",
                xGroupId = "xGroupId 1",
            },
            new Consent{
                // Id = Guid.NewGuid(),
                ConsentDefinitionId = consentDefinition[1].Id,
                UserId = Guid.NewGuid(),
                State = "Approval",
                ConsentType = "BKM",
                AdditionalData = "Additional Data 2",
                Description = "Description 2",
                xGroupId = "xGroupId 2",
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
                Permission = "Permission 1",
                PermissionLastDate = DateTime.Now.ToUniversalTime()
            },
            new ConsentPermission{
                ConsentId = consent[1].Id,
                Permission = "Permission 2",
                TransactionStartDate = DateTime.Now.ToUniversalTime(),
                TransactionEndDate = DateTime.Now.ToUniversalTime(),
                PermissionLastDate = DateTime.Now.ToUniversalTime()
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
                TokenType = "Access Token",
                ExpireTime = 5

            },
            new Token{
                ConsentId= consent[1].Id,
                TokenValue= "Token Value 2",
                TokenType= "Refresh Token",
                ExpireTime= 10
            }
        };

        foreach (Token t in token)
        {
            context.Tokens.Add(t);
        }
        context.SaveChanges();


    }
}
