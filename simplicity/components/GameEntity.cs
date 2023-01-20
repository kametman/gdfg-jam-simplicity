using Godot;
using System;
using System.Collections.Generic;

public class GameEntity : Node2D
{
    [Export] public int ShapeIndex = 0;
    [Export] public int ColorIndex = 0;

    [Export] private string[] _imagesList;
    [Export] private Color[] _colorsList;

    private List<Texture> _texturesList;
    private Sprite _entitySprite;
    private Sprite _glowSprite;

    public override void _Ready()
    {
        _texturesList = new List<Texture>();
        for (var i = 0; i < _imagesList.Length; i++)
        {
            _texturesList.Add(GD.Load<Texture>(_imagesList[i]));
        }

        _entitySprite = GetNode<Sprite>("EntitySprite");
        _glowSprite = GetNode<Sprite>("EntitySprite/GlowSprite");
    }

    public override void _Process(float delta)
    {
        var color = _colorsList[ColorIndex % _colorsList.Length];
        _entitySprite.Texture = _texturesList[ShapeIndex % _imagesList.Length];
        _entitySprite.Modulate = color;
        _glowSprite.Texture = _texturesList[ShapeIndex % _imagesList.Length];
        
        color = (new Color(color.r, color.g, color.b, 0.5f) + 2 * Colors.White) / 3;
        _glowSprite.Modulate = color;
    }
}
