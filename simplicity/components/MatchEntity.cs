using Godot;
using System;

public class MatchEntity : RigidBody2D
{
    [Signal] public delegate void MatchCollided(int points);
    [Signal] public delegate void PlayerChanged(int shapeIndex, int colorIndex);

    private GameEntity _gameEntity;

    public override void _Ready()
    {
        _gameEntity = GetNode<GameEntity>("GameEntity");
    }

    public void Init(int shapeIndex, int colorIndex)
    {
        _gameEntity.ShapeIndex = shapeIndex;
        _gameEntity.ColorIndex = colorIndex;
    }

    public void OnMatchEntityBodyEntered(Node body)
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
                else { EmitSignal(nameof(MatchCollided), 0); }

                EmitSignal(nameof(PlayerChanged), thisShape, thisColor);
            }
        }

        QueueFree();
    }

    public void OnVisibilityNotifier2dScreenExited()
    {
        QueueFree();
    }
}
