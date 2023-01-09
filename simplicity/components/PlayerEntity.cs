using Godot;
using System;

public class PlayerEntity : KinematicBody2D
{
    [Export] private float _speed;
    [Export] private uint _screenMargin = 16;
    private Vector2 _screenSize;
    private GameEntity _gameEntity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewportRect().Size;
        _gameEntity = GetNode<GameEntity>("GameEntity");
    }

    public (int, int) GetEntityType()
    {
        return (_gameEntity.ShapeIndex, _gameEntity.ColorIndex);
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

            var postionX = Mathf.Clamp(Position.x, _screenMargin, _screenSize.x - _screenMargin);
            var postionY = Mathf.Clamp(Position.y, _screenMargin, _screenSize.y - _screenMargin);
            Position = new Vector2(postionX, postionY);
        }
    }

    // public override void _Input(InputEvent @event)
    // {
    //     if (@event is InputEventMouseButton)
    //     {
    //         var mouseEvent = @event as InputEventMouseButton;
    //         if (mouseEvent.ButtonIndex == (int)ButtonList.Left) 
    //         {
    //             if (mouseEvent.Pressed)
    //             {
    //                 var mousePosition = mouseEvent.Position;
    //                 var positionDiff = mousePosition - Position;
    //                 if (Math.Abs(positionDiff.x) > Math.Abs(positionDiff.y))
    //                 {
    //                     if (positionDiff.x < 0) { Input.ActionPress("move_left"); }
    //                     if (positionDiff.x > 0) { Input.ActionPress("move_right"); }
    //                 }
    //                 if (Math.Abs(positionDiff.y) < Math.Abs(positionDiff.x))
    //                 {
    //                     if (positionDiff.y < 0) { Input.ActionPress("move_up"); }
    //                     if (positionDiff.y > 0) { Input.ActionPress("move_down"); }
    //                 }

    //                 // if (mousePosition.y < Position.y) { Input.ActionPress("move_up"); }
    //                 // if (mousePosition.y > Position.y) { Input.ActionPress("move_down"); }
    //                 // if (mousePosition.x < Position.x) { Input.ActionPress("move_left"); }
    //                 // if (mousePosition.x > Position.x) { Input.ActionPress("move_right"); }
    //             }
    //             else
    //             {
    //                 Input.ActionRelease("move_up");
    //                 Input.ActionRelease("move_down");
    //                 Input.ActionRelease("move_left");
    //                 Input.ActionRelease("move_right");
    //             }
    //         }
    //     }

    //     base._Input(@event);
    // }

    public void OnPlayerChanged(int shapeIndex, int colorIndex)
    {
        _gameEntity.ShapeIndex = shapeIndex;
        _gameEntity.ColorIndex = colorIndex;
        // matchEntity.Disconnect("PlayerChanged", this, "OnPlayerChanged");

    }
}
