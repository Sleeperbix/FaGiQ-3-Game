using Godot;
using System;

public partial class ButtonAnimation : Button
{
    private Tween idleTween;

    public override void _Ready()
    {
        this.MouseEntered += OnMouseEntered;
        this.MouseExited += OnMouseExited;
        this.Pressed += OnPressed;

        StartIdlePulse();
        PivotOffset = new Vector2(Size.X / 2f, Size.Y / 2f);

    }

    private void StartIdlePulse()
    {        
        idleTween?.Kill();
        idleTween = GetTree().CreateTween().SetLoops();

        float randomSpeed = (float)GD.RandRange(1.5, 3.0);

        idleTween.TweenProperty(this, "scale", new Vector2(0.95f, 0.95f), randomSpeed).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        idleTween.TweenProperty(this, "scale", new Vector2(1.00f, 1.00f), randomSpeed).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
    }

    private void OnMouseEntered()
    {
        ZIndex = 10;
        idleTween?.Kill();
        var tween = GetTree().CreateTween().SetLoops();
        tween.TweenProperty(this, "scale", new Vector2(1.2f, 1.2f), 0.5f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 0.5f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);

    }

    private void OnMouseExited()
    {
        ZIndex = 0;        
        idleTween?.Kill();
        var tween = GetTree().CreateTween().SetLoops();
        tween.TweenProperty(this, "scale", new Vector2(1.0f, 1.0f), 0.5f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        StartIdlePulse();
    }

    private void OnPressed()
    {
        var spinTween = GetTree().CreateTween();
        spinTween.TweenProperty(this, "rotation_degrees", 360, 1.0f).AsRelative().SetTrans(Tween.TransitionType.Cubic);
    }
}
