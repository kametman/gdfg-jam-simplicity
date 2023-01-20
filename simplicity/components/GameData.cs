using Godot;
using System.Collections.Generic;
using System.Linq;

public class GameData : Node2D
{
    public int Score { get { return _score; } }
    public int MatchLevel { get { return _matchLevel; } }
    public int Lives { get { return _lives; } }
    public int[] HighScores { get { return _highScores.ToArray(); } }

    [Export] private int _score;
    [Export] private List<int> _highScores;
    [Export] private int _matchLevel;
    private int[] _levelThresholds = new int[] { 10, 25, 50, 100, };
    [Export] private int _lives;

    private AudioManager _audioManager;

    public override void _Ready()
    {
        _highScores = new List<int>();
        _audioManager = GetNode<AudioManager>("AudioManager");
    }

    public void ResetScore()
    {
        _score = 0;
        _matchLevel = 0;
        _lives = 3;
    }

    public bool UpdateScore(int points)
    {
        if (points == 0)
        {
            _audioManager.PlayNoMatchSfx();
            _lives--;

            if (_lives == 0)
            {
                AddNewScore(_score);
                return true;
            }
        }
        else
        {
            if (points == 1) { _audioManager.PlayGoodMatchSfx(); }
            else { _audioManager.PlayPerfectMatchSfx(); }
            
            _score += points;
            CalculateMatchLevel();
        }
        return false;
    }

    private bool AddNewScore(int newScore)
    {
        _highScores.Add(newScore);
        _highScores = _highScores.OrderByDescending(x => x).Take(3).ToList();
        return newScore == _highScores[0];
    }

    private void CalculateMatchLevel()
    {
        if (_score < _levelThresholds[0]) { _matchLevel = 0; }
        else if (_score < _levelThresholds[1]) { _matchLevel = 1; }
        else if (_score < _levelThresholds[2]) { _matchLevel = 2; }
        else if (_score < _levelThresholds[3]) { _matchLevel = 3; }
        else { _matchLevel = 4; }
    }
}
