namespace API.Models;

public class MatchResult
{
    public int Id { get; set; }
    public string Group { get; set; } = string.Empty;
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
}
