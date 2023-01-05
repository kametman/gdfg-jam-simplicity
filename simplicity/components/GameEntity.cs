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

    public override void _Ready()
    {
        _texturesList = new List<Texture>();
        for (var i = 0; i < _imagesList.Length; i++)
        {
            _texturesList.Add(GD.Load<Texture>(_imagesList[i]));
        }

        _entitySprite = GetNode<Sprite>("EntitySprite");
    }

    public override void _Process(float delta)
    {
        _entitySprite.Texture = _texturesList[ShapeIndex % _imagesList.Length];
        _entitySprite.Modulate = _colorsList[ColorIndex % _colorsList.Length];
    }
}
