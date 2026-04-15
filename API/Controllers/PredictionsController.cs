using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PredictionsController : ControllerBase
{
    private readonly WCupDbContext _context;

    public PredictionsController(WCupDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchPredictionDto>>> GetPredictions([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("Kullanıcı adı sorgu parametresi olarak gereklidir.");
        }

        var user = await _context.Users
            .Include(u => u.Predictions)
            .FirstOrDefaultAsync(u => u.Username == username.Trim());

        if (user is null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        return user.Predictions
            .Select(p => new MatchPredictionDto(p.Group, p.HomeTeam, p.AwayTeam, p.HomeScore, p.AwayScore))
            .ToList();
    }

    [HttpPost]
    public async Task<IActionResult> SavePredictions(SavePredictionsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            return BadRequest("Kullanıcı adı zorunludur.");
        }

        var username = request.Username.Trim();
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
        {
            user = new User { Username = username };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var existingPredictions = _context.MatchPredictions.Where(mp => mp.UserId == user.Id);
        _context.MatchPredictions.RemoveRange(existingPredictions);

        var newPredictions = request.Predictions.Select(p => new MatchPrediction
        {
            UserId = user.Id,
            Group = p.Group,
            HomeTeam = p.HomeTeam,
            AwayTeam = p.AwayTeam,
            HomeScore = p.HomeScore,
            AwayScore = p.AwayScore
        });

        _context.MatchPredictions.AddRange(newPredictions);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Tahminler kaydedildi." });
    }
}

public record MatchPredictionDto(string Group, string HomeTeam, string AwayTeam, int HomeScore, int AwayScore);
public record SavePredictionsRequest(string Username, List<MatchPredictionDto> Predictions);
