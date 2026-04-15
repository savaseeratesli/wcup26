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
                new MatchDto("Meksika", "Kanada", "mx", "ca"),
                new MatchDto("ABD", "Panama", "us", "pa")
            }),
            new("B GRUBU", new[] {
                new MatchDto("Türkiye", "İtalya", "tr", "it"),
                new MatchDto("Arjantin", "Fransa", "ar", "fr")
            }),
            new("C GRUBU", new[] {
                new MatchDto("Brezilya", "Almanya", "br", "de"),
                new MatchDto("İspanya", "Portekiz", "es", "pt")
            })
        };

        return groups;
    }
}

public record MatchGroupDto(string Title, IEnumerable<MatchDto> Matches);
public record MatchDto(string Home, string Away, string HomeCode, string AwayCode);
