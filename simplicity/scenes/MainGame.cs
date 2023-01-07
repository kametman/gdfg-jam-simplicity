using Godot;
using System;

public class MainGame : Node2D
{
    private GameData _gameData;
    private Label _scoreLabel;
    private PathFollow2D _spawnLocation;
    private PackedScene _matchEntityPrefab;

    public override void _Ready()
    {
        GD.Randomize();

        _gameData = GetNode<GameData>("/root/GameData");
        _scoreLabel = GetNode<Label>("UI/ScoreLabel");
        _spawnLocation = GetNode<PathFollow2D>("SpawnPath/SpawnLocation");
        _matchEntityPrefab = (PackedScene)ResourceLoader.Load("res://components/MatchEntity.tscn");
    }

    public override void _Process(float delta)
    {
        _scoreLabel.Text = _gameData.Score.ToString();
    }

    public void OnSpawnTimerTimeout()
    {
        _spawnLocation.Offset = GD.Randi();
        
        var direction = _spawnLocation.Rotation + Mathf.Pi / 2;
        var velocity =  new Vector2((float)GD.RandRange(150.0, 250.0), 0);

        var newMatch = _matchEntityPrefab.Instance<MatchEntity>();
        newMatch.Position = _spawnLocation.Position;
        newMatch.LinearVelocity = velocity.Rotated(direction);

        AddChild(newMatch);
        newMatch.Connect("MatchCollision", _gameData, "OnMatchEntityMatchCollision");
    }
}
