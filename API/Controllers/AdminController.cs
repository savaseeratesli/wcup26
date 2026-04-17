using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly WCupDbContext _context;

    public AdminController(IConfiguration configuration, WCupDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login(AdminLoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Admin kullanıcı adı ve şifre gereklidir.");
        }

        var adminSection = _configuration.GetSection("AdminCredentials");
        var adminUsername = adminSection.GetValue<string>("Username");
        var adminPassword = adminSection.GetValue<string>("Password");

        if (request.Username.Trim() == adminUsername && request.Password == adminPassword)
        {
            return Ok(new { message = "Admin girişi başarılı." });
        }

        return Unauthorized("Geçersiz admin bilgileri.");
    }

    [HttpGet("results")]
    public async Task<ActionResult<IEnumerable<MatchResultDto>>> GetResults()
    {
        var results = await _context.MatchResults.ToListAsync();
        return results.Select(r => new MatchResultDto(r.Group, r.HomeTeam, r.AwayTeam, r.HomeScore, r.AwayScore)).ToList();
    }

    [HttpPost("results")]
    public async Task<IActionResult> SaveResults(SaveMatchResultsRequest request)
    {
        if (request.Results == null || !request.Results.Any())
        {
            return BadRequest("En az bir maç sonucu gönderilmelidir.");
        }

        var existingResults = await _context.MatchResults.ToListAsync();

        foreach (var result in request.Results)
        {
            var normalizedGroup = result.Group.Trim();
            var found = existingResults.FirstOrDefault(r =>
                r.Group == normalizedGroup &&
                r.HomeTeam == result.HomeTeam &&
                r.AwayTeam == result.AwayTeam);

            if (found is not null)
            {
                found.HomeScore = result.HomeScore;
                found.AwayScore = result.AwayScore;
                _context.MatchResults.Update(found);
            }
            else
            {
                _context.MatchResults.Add(new MatchResult
                {
                    Group = normalizedGroup,
                    HomeTeam = result.HomeTeam,
                    AwayTeam = result.AwayTeam,
                    HomeScore = result.HomeScore,
                    AwayScore = result.AwayScore
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Maç sonuçları kaydedildi." });
    }

    [HttpGet("scoreboard")]
    public async Task<ActionResult<IEnumerable<UserScoreDto>>> GetScoreboard()
    {
        var officialResults = await _context.MatchResults.ToListAsync();
        if (!officialResults.Any())
        {
            return Ok(Array.Empty<UserScoreDto>());
        }

        var users = await _context.Users
            .Include(u => u.Predictions)
            .ToListAsync();

        var scoreboard = users.Select(user =>
        {
            var score = user.Predictions.Sum(prediction => CalculatePredictionScore(prediction, officialResults));
            return new UserScoreDto(user.Username, score);
        })
        .OrderByDescending(x => x.Score)
        .ThenBy(x => x.Username)
        .ToList();

        return scoreboard;
    }

    private static int CalculatePredictionScore(MatchPrediction prediction, IEnumerable<MatchResult> results)
    {
        var official = results.FirstOrDefault(result =>
            result.Group == prediction.Group &&
            result.HomeTeam == prediction.HomeTeam &&
            result.AwayTeam == prediction.AwayTeam);

        if (official is null)
        {
            return 0;
        }

        if (official.HomeScore == prediction.HomeScore && official.AwayScore == prediction.AwayScore)
        {
            return 3;
        }

        var predictedDiff = prediction.HomeScore - prediction.AwayScore;
        var officialDiff = official.HomeScore - official.AwayScore;

        if ((predictedDiff > 0 && officialDiff > 0) ||
            (predictedDiff < 0 && officialDiff < 0) ||
            (predictedDiff == 0 && officialDiff == 0))
        {
            return 1;
        }

        return 0;
    }
}

public record AdminLoginRequest(string Username, string Password);
public record MatchResultDto(string Group, string HomeTeam, string AwayTeam, int HomeScore, int AwayScore);
public record SaveMatchResultsRequest(List<MatchResultDto> Results);
public record UserScoreDto(string Username, int Score);
