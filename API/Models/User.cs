namespace API.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<MatchPrediction> Predictions { get; set; } = new();
}
