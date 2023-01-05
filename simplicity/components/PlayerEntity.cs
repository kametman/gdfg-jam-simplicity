using Godot;
using System;

public class PlayerEntity : KinematicBody2D
{
    [Export] private float _speed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    
    public override void _PhysicsProcess(float delta)
    {
        var direction = Vector2.Zero;
        if (Input.IsActionPressed("move_up")) { direction.y -= 1f; }
        if (Input.IsActionPressed("move_down")) { direction.y += 1f; }
        if (Input.IsActionPressed("move_left")) { direction.x -= 1f; }
        if (Input.IsActionPressed("move_right")) { direction.x += 1f; }

        if (direction != Vector2.Zero)
        {
            direction = direction.Normalized();
            MoveAndSlide(direction * _speed);
        }
    }
    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
