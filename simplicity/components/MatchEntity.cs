using Godot;
using System;

public class MatchEntity : RigidBody2D
{
    [Signal] public delegate void MatchCollision(MatchEntity matchEntity, int points);

    private GameEntity _gameEntity;

    public override void _Ready()
    {
        _gameEntity = GetNode<GameEntity>("GameEntity");
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
                EmitSignal(nameof(MatchCollision),this, 3);
            }
            else if (playerShape == thisShape || playerColor == thisColor)
            {
                EmitSignal(nameof(MatchCollision), this, 1);
            }
        }
        else { EmitSignal(nameof(MatchCollision), this, 0); }

        QueueFree();
    }
}
