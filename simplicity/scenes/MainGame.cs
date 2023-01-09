using Godot;
using System;

public class MainGame : Node2D
{
    private GameData _gameData;
    private Label _scoreLabel;
    private Label _livesLabel;
    private PathFollow2D _spawnLocation;
    private PackedScene _matchEntityPrefab;
    private (int, int)[] _newMatches = new (int, int)[]
    {
        (4, 2), (6, 3), (9, 3), (12, 4), (16, 4),
    };
    private Vector2 _screenSize;
    private int _spawnMargin = 16;
    private PlayerEntity _playerEntity;
    private float _scaleFactor = 0.02f;
    private string _livesText = "[][][]";

    public override void _Ready()
    {
        GD.Randomize();

        _gameData = GetNode<GameData>("/root/GameData");
        _scoreLabel = GetNode<Label>("UI/ScoreLabel");
        _livesLabel = GetNode<Label>("UI/LivesLabel");
        _spawnLocation = GetNode<PathFollow2D>("SpawnPath/SpawnLocation");
        _matchEntityPrefab = (PackedScene)ResourceLoader.Load("res://components/MatchEntity.tscn");
        _screenSize = GetViewportRect().Size;
        _playerEntity = GetNode<PlayerEntity>("PlayerEntity");
    }

    public override void _Process(float delta)
    {
        _scoreLabel.Text = _gameData.Score.ToString();
        _livesLabel.Text = _livesText.Substring(0, _gameData.Lives * 2);
    }

    public void OnSpawnTimerTimeout()
    {
        GetNewMatchEntity();
    }

    public void OnMatchEntityMatchCollided(int points)
    {
        var gameOver = _gameData.UpdateScore(points);
        if (gameOver) { GetTree().ChangeScene("res://scenes/TitleScreen.tscn"); }
    }

    private MatchEntity GetNewMatchEntity()
    {
        _spawnLocation.Offset = GD.Randi();
        
        var direction = _spawnLocation.Rotation + Mathf.Pi / 2;
        var velocity =  new Vector2((float)GD.RandRange(150.0, 250.0) + _gameData.Score, 0);
        var matchParams = _newMatches[_gameData.MatchLevel];
        var matchIndex = (int)(GD.Randi() % matchParams.Item1);

        var spawnPosition = _spawnLocation.Position;
        spawnPosition.x = Mathf.Clamp(spawnPosition.x, _spawnMargin, _screenSize.x - _spawnMargin);
        spawnPosition.y = Mathf.Clamp(spawnPosition.y, _spawnMargin, _screenSize.y - _spawnMargin);

        var newScale = 1f + (_gameData.Score * _scaleFactor);

        var matchEntity = _matchEntityPrefab.Instance<MatchEntity>();
        AddChild(matchEntity);
        matchEntity.Init(matchIndex / matchParams.Item2, matchIndex % matchParams.Item2);
        matchEntity.Position = spawnPosition;
        matchEntity.LinearVelocity = velocity.Rotated(direction);
        matchEntity.Scale = new Vector2(newScale, newScale);
        matchEntity.Connect("MatchCollided", this, "OnMatchEntityMatchCollided");
        matchEntity.Connect("PlayerChanged", _playerEntity, "OnPlayerChanged");

        return matchEntity;
    }


}
