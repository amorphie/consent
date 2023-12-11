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

        var yosInfo = new OBYosInfo[]{
            new OBYosInfo{
                Id = Guid.NewGuid(),
                marka = "İş Bankası",
                unv = "unv 1",
                kod = "kod 1",
                acikAnahtar = "acikAnahtar 1",
                roller = new List<string> { "Rol 1", "Rol 2" },
                adresler = new List<string> { "Adres 1", "Adres 2" },
                logoBilgileri = new List<string> { "Logo 1", "Logo 2" }
            },
                new OBYosInfo{
                Id = Guid.NewGuid(),
                marka = "Ziraat Bankası",
                unv = "unv 1",
                kod = "kod 1",
                acikAnahtar = "acikAnahtar 1",
                roller = new List<string> { "Rol 1", "Rol 2" },
                adresler = new List<string> { "Adres 1", "Adres 2" },
                logoBilgileri = new List<string> { "Logo 1", "Logo 2" }
            }
        };
        foreach (OBYosInfo c in yosInfo)
        {
            context.OBYosInfos.Add(c);
        }
        context.SaveChanges();
        var consent = new Consent[]{
            new Consent{
                // Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                State = "Waiting",
                ConsentType = "Open Banking",
                AdditionalData = "Additional Data 1",
                Description = "Description 1",
                XGroupId = "xGroupId 1",
            },
            new Consent{
                // Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                State = "Approval",
                ConsentType = "BKM",
                AdditionalData = "Additional Data 2",
                Description = "Description 2",
                XGroupId = "xGroupId 2",
            },
        };

        foreach (Consent c in consent)
        {
            context.Consents.Add(c);
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
