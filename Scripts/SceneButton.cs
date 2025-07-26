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
        GD.Print("You pressed me!");
        ManagerGame.Instance.TransitionToScene(targetScene);
    }
}
