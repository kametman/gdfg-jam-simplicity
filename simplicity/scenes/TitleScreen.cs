using Godot;
using System;

public class TitleScreen : Node2D
{
    private GameData _gameData;
    private Label _highScoreTitleLabel;
    private Label _highScoresLabel;

    public override void _Ready()
    {
        _gameData = GetNode<GameData>("/root/GameData");
        _highScoreTitleLabel = GetNode<Label>("UI/HighScoreTitleLabel");
        _highScoresLabel = GetNode<Label>("UI/HighScoresLabel");
    }

    public override void _Process(float delta)
    {
        _highScoreTitleLabel.Visible = _gameData.HighScores.Length > 0;
        if (_gameData.HighScores.Length > 0)
        {
            _highScoreTitleLabel.Visible = true;
            _highScoresLabel.Visible = true;

            var highScores = string.Join(System.Environment.NewLine, _gameData.HighScores);
            _highScoresLabel.Text = highScores;
        }
        else
        {
            _highScoreTitleLabel.Visible = false;
            _highScoresLabel.Visible = false;
            _highScoresLabel.Text = string.Empty;
        }
    }

    public void OnPlayButtonPressed()
    {
        GetTree().ChangeScene("res://scenes/HintScreen.tscn");
    }
}
