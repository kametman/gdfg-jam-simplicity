using Godot;
using System;

public class HintScreen : Node2D
{
    private GameData _gameData;

    public override void _Ready()
    {
        _gameData = GetNode<GameData>("/root/GameData");
    }

    public void OnStartButtonPressed()
    {
        _gameData.ResetScore();
        GetTree().ChangeScene("res://scenes/MainGame.tscn");
    }
}
