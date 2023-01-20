using Godot;
using System;

public class MainCamera : Camera2D
{
    private Vector2 _defaultOffset;
    private Timer _timer;
    private Tween _tween;
    private float _shakeAmount = 0f;

    public override void _Ready()
    {
        _defaultOffset = Offset;
        _timer = GetNode<Timer>("Timer");
        _tween = GetNode<Tween>("Tween");
        GD.Randomize();
    }

    public override void _Process(float delta)
    {
        Offset = new Vector2((float)GD.RandRange(-1, 1), (float)GD.RandRange(-1, 1)) * _shakeAmount;
    }

    public void OnScreenShake(float shakeAmount, float duration)
    {
        GD.Print("shake");
        _shakeAmount = shakeAmount;
        _timer.WaitTime = duration;
        _timer.Start();
    }

    public void OnTimerTimeout()
    {
        _shakeAmount = 0f;
        _tween.InterpolateProperty(this, "offset", Offset,_defaultOffset, 0.1f);
    }
}
