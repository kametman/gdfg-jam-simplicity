using Godot;
using System;

public class AudioManager : Node2D
{
    private AudioStreamPlayer2D _goodMatchPlayer;
    private AudioStreamPlayer2D _noMatchPlayer;
    private AudioStreamPlayer2D _perfectMatchPlayer;

    private float _pitchScale = 1f;

    public override void _Ready()
    {
        _goodMatchPlayer = GetNode<AudioStreamPlayer2D>("GoodMatchPlayer");
        _noMatchPlayer = GetNode<AudioStreamPlayer2D>("NoMatchPlayer");
        _perfectMatchPlayer = GetNode<AudioStreamPlayer2D>("PerfectMatchPlayer");

        ResetPitch();
    }


    public void PlayGoodMatchSfx()
    {
        _goodMatchPlayer.Play();
        //_perfectMatchPlayer.Play();
        IncreasePitch();
    }

    public void PlayNoMatchSfx()
    {
        ResetPitch();
        _noMatchPlayer.Play();
    }

    public void PlayPerfectMatchSfx()
    {
        _goodMatchPlayer.Play();
        _goodMatchPlayer.Play();
        //_perfectMatchPlayer.Play();
        IncreasePitch();
    }

    private void ResetPitch()
    {
        _pitchScale = 1f;
        _goodMatchPlayer.PitchScale = _pitchScale;
        _perfectMatchPlayer.PitchScale = _pitchScale;
    }
    private void IncreasePitch()
    {
        _pitchScale += 0.25f;
        _goodMatchPlayer.PitchScale = _pitchScale;
        _perfectMatchPlayer.PitchScale = _pitchScale;
    }
}
