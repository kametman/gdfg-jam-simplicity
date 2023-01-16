using Godot;
using System;

public class MatchEntity : KinematicBody2D
{
    [Signal] public delegate void MatchCollided(int points);
    [Signal] public delegate void PlayerChanged(int shapeIndex, int colorIndex);
    [Signal] public delegate void ScreenShake(float shakeAmount, float duration);

    private GameEntity _gameEntity;
    private float _speed;
    private float _direction;

    public override void _Ready()
    {
        _gameEntity = GetNode<GameEntity>("GameEntity");
    }

    public void Init(int shapeIndex, int colorIndex, float speed, float direction)
    {
        _gameEntity.ShapeIndex = shapeIndex;
        _gameEntity.ColorIndex = colorIndex;
        _speed = speed;
        _direction = direction;
    }
    public override void _PhysicsProcess(float delta)
    {
        var velocity = new Vector2(_speed, 0);
        var collision = MoveAndCollide(velocity.Rotated(_direction) * delta);
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
}
