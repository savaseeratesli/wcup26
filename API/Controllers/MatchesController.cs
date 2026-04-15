using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<MatchGroupDto>> GetMatches()
    {
        var groups = new List<MatchGroupDto>
        {
            new("A GRUBU", new[] {
                new MatchDto("ABD", "Kanada", "us", "ca", new DateTime(2026, 6, 12, 18, 0, 0)),
                new MatchDto("Meksika", "Kosta Rika", "mx", "cr", new DateTime(2026, 6, 12, 21, 0, 0)),
                new MatchDto("ABD", "Meksika", "us", "mx", new DateTime(2026, 6, 16, 19, 30, 0)),
                new MatchDto("Kanada", "Kosta Rika", "ca", "cr", new DateTime(2026, 6, 16, 22, 0, 0)),
                new MatchDto("Kosta Rika", "ABD", "cr", "us", new DateTime(2026, 6, 20, 18, 0, 0)),
                new MatchDto("Kanada", "Meksika", "ca", "mx", new DateTime(2026, 6, 20, 21, 0, 0))
            }),
            new("B GRUBU", new[] {
                new MatchDto("Brezilya", "Arjantin", "br", "ar", new DateTime(2026, 6, 13, 18, 0, 0)),
                new MatchDto("Uruguay", "Paraguay", "uy", "py", new DateTime(2026, 6, 13, 21, 0, 0)),
                new MatchDto("Brezilya", "Uruguay", "br", "uy", new DateTime(2026, 6, 17, 19, 30, 0)),
                new MatchDto("Arjantin", "Paraguay", "ar", "py", new DateTime(2026, 6, 17, 22, 0, 0)),
                new MatchDto("Paraguay", "Brezilya", "py", "br", new DateTime(2026, 6, 21, 18, 0, 0)),
                new MatchDto("Arjantin", "Uruguay", "ar", "uy", new DateTime(2026, 6, 21, 21, 0, 0))
            }),
            new("C GRUBU", new[] {
                new MatchDto("İngiltere", "Fransa", "gb", "fr", new DateTime(2026, 6, 14, 18, 0, 0)),
                new MatchDto("Almanya", "İspanya", "de", "es", new DateTime(2026, 6, 14, 21, 0, 0)),
                new MatchDto("İngiltere", "Almanya", "gb", "de", new DateTime(2026, 6, 18, 19, 30, 0)),
                new MatchDto("Fransa", "İspanya", "fr", "es", new DateTime(2026, 6, 18, 22, 0, 0)),
                new MatchDto("İspanya", "İngiltere", "es", "gb", new DateTime(2026, 6, 22, 18, 0, 0)),
                new MatchDto("Fransa", "Almanya", "fr", "de", new DateTime(2026, 6, 22, 21, 0, 0))
            }),
            new("D GRUBU", new[] {
                new MatchDto("Türkiye", "Avustralya", "tr", "au", new DateTime(2026, 6, 14, 7, 0, 0)),
                new MatchDto("Hollanda", "Senegal", "nl", "sn", new DateTime(2026, 6, 14, 10, 0, 0)),
                new MatchDto("Türkiye", "Hollanda", "tr", "nl", new DateTime(2026, 6, 18, 7, 0, 0)),
                new MatchDto("Avustralya", "Senegal", "au", "sn", new DateTime(2026, 6, 18, 10, 0, 0)),
                new MatchDto("Senegal", "Türkiye", "sn", "tr", new DateTime(2026, 6, 22, 7, 0, 0)),
                new MatchDto("Avustralya", "Hollanda", "au", "nl", new DateTime(2026, 6, 22, 10, 0, 0))
            }),
            new("E GRUBU", new[] {
                new MatchDto("Japonya", "Güney Kore", "jp", "kr", new DateTime(2026, 6, 12, 15, 0, 0)),
                new MatchDto("Avustralya", "Suudi Arabistan", "au", "sa", new DateTime(2026, 6, 12, 18, 0, 0)),
                new MatchDto("Japonya", "Avustralya", "jp", "au", new DateTime(2026, 6, 16, 21, 0, 0)),
                new MatchDto("Güney Kore", "Suudi Arabistan", "kr", "sa", new DateTime(2026, 6, 16, 23, 30, 0)),
                new MatchDto("Suudi Arabistan", "Japonya", "sa", "jp", new DateTime(2026, 6, 20, 18, 0, 0)),
                new MatchDto("Güney Kore", "Avustralya", "kr", "au", new DateTime(2026, 6, 20, 21, 0, 0))
            }),
            new("F GRUBU", new[] {
                new MatchDto("Fas", "Mısır", "ma", "eg", new DateTime(2026, 6, 13, 15, 0, 0)),
                new MatchDto("Nijerya", "Senegal", "ng", "sn", new DateTime(2026, 6, 13, 18, 0, 0)),
                new MatchDto("Fas", "Nijerya", "ma", "ng", new DateTime(2026, 6, 17, 21, 0, 0)),
                new MatchDto("Mısır", "Senegal", "eg", "sn", new DateTime(2026, 6, 17, 23, 30, 0)),
                new MatchDto("Senegal", "Fas", "sn", "ma", new DateTime(2026, 6, 21, 18, 0, 0)),
                new MatchDto("Mısır", "Nijerya", "eg", "ng", new DateTime(2026, 6, 21, 21, 0, 0))
            }),
            new("G GRUBU", new[] {
                new MatchDto("Hırvatistan", "Sırbistan", "hr", "rs", new DateTime(2026, 6, 14, 15, 0, 0)),
                new MatchDto("İsviçre", "Danimarka", "ch", "dk", new DateTime(2026, 6, 14, 18, 0, 0)),
                new MatchDto("Hırvatistan", "İsviçre", "hr", "ch", new DateTime(2026, 6, 18, 21, 0, 0)),
                new MatchDto("Sırbistan", "Danimarka", "rs", "dk", new DateTime(2026, 6, 18, 23, 30, 0)),
                new MatchDto("Danimarka", "Hırvatistan", "dk", "hr", new DateTime(2026, 6, 22, 18, 0, 0)),
                new MatchDto("Sırbistan", "İsviçre", "rs", "ch", new DateTime(2026, 6, 22, 21, 0, 0))
            }),
            new("H GRUBU", new[] {
                new MatchDto("Polonya", "Türkiye", "pl", "tr", new DateTime(2026, 6, 15, 15, 0, 0)),
                new MatchDto("Çekya", "İskoçya", "cz", "gb", new DateTime(2026, 6, 15, 18, 0, 0)),
                new MatchDto("Polonya", "Çekya", "pl", "cz", new DateTime(2026, 6, 19, 21, 0, 0)),
                new MatchDto("Türkiye", "İskoçya", "tr", "gb", new DateTime(2026, 6, 19, 23, 30, 0)),
                new MatchDto("İskoçya", "Polonya", "gb", "pl", new DateTime(2026, 6, 23, 18, 0, 0)),
                new MatchDto("Türkiye", "Çekya", "tr", "cz", new DateTime(2026, 6, 23, 21, 0, 0))
            }),
            new("I GRUBU", new[] {
                new MatchDto("Kamerun", "Fildişi Sahili", "cm", "ci", new DateTime(2026, 6, 16, 15, 0, 0)),
                new MatchDto("Gana", "Tunus", "gh", "tn", new DateTime(2026, 6, 16, 18, 0, 0)),
                new MatchDto("Kamerun", "Gana", "cm", "gh", new DateTime(2026, 6, 20, 21, 0, 0)),
                new MatchDto("Fildişi Sahili", "Tunus", "ci", "tn", new DateTime(2026, 6, 20, 23, 30, 0)),
                new MatchDto("Tunus", "Kamerun", "tn", "cm", new DateTime(2026, 6, 24, 18, 0, 0)),
                new MatchDto("Fildişi Sahili", "Gana", "ci", "gh", new DateTime(2026, 6, 24, 21, 0, 0))
            }),
            new("J GRUBU", new[] {
                new MatchDto("Katar", "Ekvador", "qa", "ec", new DateTime(2026, 6, 13, 15, 0, 0)),
                new MatchDto("Kolombiya", "Peru", "co", "pe", new DateTime(2026, 6, 13, 18, 0, 0)),
                new MatchDto("Katar", "Kolombiya", "qa", "co", new DateTime(2026, 6, 17, 21, 0, 0)),
                new MatchDto("Ekvador", "Peru", "ec", "pe", new DateTime(2026, 6, 17, 23, 30, 0)),
                new MatchDto("Peru", "Katar", "pe", "qa", new DateTime(2026, 6, 21, 18, 0, 0)),
                new MatchDto("Ekvador", "Kolombiya", "ec", "co", new DateTime(2026, 6, 21, 21, 0, 0))
            }),
            new("K GRUBU", new[] {
                new MatchDto("İsveç", "Norveç", "se", "no", new DateTime(2026, 6, 14, 15, 0, 0)),
                new MatchDto("Avusturya", "Galler", "at", "gb", new DateTime(2026, 6, 14, 18, 0, 0)),
                new MatchDto("İsveç", "Avusturya", "se", "at", new DateTime(2026, 6, 18, 21, 0, 0)),
                new MatchDto("Norveç", "Galler", "no", "gb", new DateTime(2026, 6, 18, 23, 30, 0)),
                new MatchDto("Galler", "İsveç", "gb", "se", new DateTime(2026, 6, 22, 18, 0, 0)),
                new MatchDto("Norveç", "Avusturya", "no", "at", new DateTime(2026, 6, 22, 21, 0, 0))
            }),
            new("L GRUBU", new[] {
                new MatchDto("Yeni Zelanda", "Venezuela", "nz", "ve", new DateTime(2026, 6, 15, 15, 0, 0)),
                new MatchDto("Şili", "Bolivya", "cl", "bo", new DateTime(2026, 6, 15, 18, 0, 0)),
                new MatchDto("Yeni Zelanda", "Şili", "nz", "cl", new DateTime(2026, 6, 19, 21, 0, 0)),
                new MatchDto("Venezuela", "Bolivya", "ve", "bo", new DateTime(2026, 6, 19, 23, 30, 0)),
                new MatchDto("Bolivya", "Yeni Zelanda", "bo", "nz", new DateTime(2026, 6, 23, 18, 0, 0)),
                new MatchDto("Venezuela", "Şili", "ve", "cl", new DateTime(2026, 6, 23, 21, 0, 0))
            })
        };

        return groups;
    }
}

public record MatchGroupDto(string Title, IEnumerable<MatchDto> Matches);
public record MatchDto(string Home, string Away, string HomeCode, string AwayCode, DateTime Date);
