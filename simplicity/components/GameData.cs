using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameData : Node2D
{
    public int Score { get { return _score; } }


    [Export] private int _score;
    [Export] private List<int> _highScores;

    public override void _Ready()
    {
        _highScores = new List<int>();
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public bool AddNewScore(int newScore)
    {
        _highScores.Add(newScore);
        _highScores = _highScores.OrderByDescending(x => x).Take(3).ToList();
        return newScore == _highScores[0];
    }

    public void OnMatchEntityMatchCollision(MatchEntity matchEntity, int points)
    {
        _score += points;
        matchEntity.Disconnect("MatchCollision", this, "OnMatchEntityMatchCollision");
    }
}
