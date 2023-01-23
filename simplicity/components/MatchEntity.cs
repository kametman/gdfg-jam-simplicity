using Godot;
using System;

public class MatchEntity : KinematicBody2D
{
    private enum PathShapes
    {
        Straight, ZigZag, Square,
    }

    [Signal] public delegate void MatchCollided(int points);
    [Signal] public delegate void PlayerChanged(int shapeIndex, int colorIndex);
    [Signal] public delegate void ScreenShake(float shakeAmount, float duration);

    private GameEntity _gameEntity;
    private Timer _pathTimer;
    private float _speed;
    private float _baseDirection;
    private float _intervalHalf = Mathf.Pi / 4f;

    private PathShapes _pathShape = PathShapes.Straight;
    private int _pathCounter = 0;
    private PathShapes[] _pathsList = new PathShapes[]
    {
        PathShapes.Straight, PathShapes.Straight, PathShapes.Straight, PathShapes.ZigZag,
        PathShapes.Straight, PathShapes.Straight, PathShapes.Straight, PathShapes.Square,
    };
    private float _zigZagVeeringAngle = Mathf.Pi / 4;
    private float[] _squareAngles = new float[]
    {
        0f, Mathf.Pi / 4f, 0f, -Mathf.Pi / 4f,
    };

    public override void _Ready()
    {
        _gameEntity = GetNode<GameEntity>("GameEntity");
        _pathTimer = GetNode<Timer>("PathTimer");
    }

    public void Init(int shapeIndex, int colorIndex, float speed, float direction, bool newPath = false)
    {
        _gameEntity.ShapeIndex = shapeIndex;
        _gameEntity.ColorIndex = colorIndex;

        _speed = speed;

        var entityDirection = (float)GD.RandRange(direction - _intervalHalf, direction + _intervalHalf);

        _baseDirection = entityDirection;

        if (newPath)
        {
            var pathIndex = (int)GD.RandRange(0, _pathsList.Length);
            _pathShape = _pathsList[pathIndex];
            if (_pathShape == PathShapes.ZigZag || _pathShape == PathShapes.Square)
            {
                _pathTimer.Start();
            }
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        var baseVelocity = new Vector2(_speed, 0);
        var entityVelocity = baseVelocity;

        if (_pathShape == PathShapes.Straight)
        {
            entityVelocity = baseVelocity.Rotated(_baseDirection);
        }
        else if (_pathShape == PathShapes.ZigZag)
        {
            _pathCounter = _pathCounter % 2;
            var veerDirection = _pathCounter == 0 ? 1 : -1;
            entityVelocity = baseVelocity.Rotated(_baseDirection + (_zigZagVeeringAngle * veerDirection));
        }
        else if (_pathShape == PathShapes.Square)
        {
            _pathCounter = _pathCounter % _squareAngles.Length;
            entityVelocity = baseVelocity.Rotated(_baseDirection + _squareAngles[_pathCounter]);
        }

        var collision = MoveAndCollide(entityVelocity * delta);
        if (collision != null)
        {
            HandleCollision((Node)collision.Collider);
        }
    }

    public void HandleCollision(Node body)
    {
        if (body is PlayerEntity)
        {
            var thisShape = _gameEntity.ShapeIndex;
            var thisColor = _gameEntity.ColorIndex;

            var (playerShape, playerColor) = ((PlayerEntity)body).GetEntityType();

            if (playerShape == thisShape && playerColor == thisColor)
            {
                EmitSignal(nameof(MatchCollided), 3);
            }
            else 
            {
                if (playerShape == thisShape || playerColor == thisColor)
                {
                    EmitSignal(nameof(MatchCollided), 1);
                }
                else 
                { 
                    EmitSignal(nameof(MatchCollided), 0); 
                    EmitSignal(nameof(ScreenShake), 5f, 0.25f);
                }

                EmitSignal(nameof(PlayerChanged), thisShape, thisColor);
            }
        }

        QueueFree();
    }

    public void OnVisibilityNotifier2dScreenExited()
    {
        QueueFree();
    }

    public void OnGameOver()
    {
        QueueFree();
    }

    public void OnPathTimerTimeout()
    {
        _pathCounter++;
    }
}
