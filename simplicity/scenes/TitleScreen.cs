using Godot;
using System;

public class TitleScreen : Node2D
{
    private GameData _gameData;

    public override void _Ready()
    {
        _gameData = GetNode<GameData>("/root/GameData");
    }

    public void OnPlayButtonPressed()
    {
        GetTree().ChangeScene("res://scenes/HintScreen.tscn");
    }
}
