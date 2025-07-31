using Godot;
using System;

public partial class SceneButton : Button
{
    [Export] public NodePath ManagerPath;
	[Export] private string targetScene;

    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }
    private void OnButtonPressed()
    {
        ManagerAudio.Instance.PlaySFX("res://Audio/SystemSFX/Button.wav");
        ManagerGame.Instance.TransitionToScene(targetScene);
    }
}
