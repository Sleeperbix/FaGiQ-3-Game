using Godot;
using System;

public partial class RevealAnswerButton : Button
{
    [Export] private ManagerQuestionRoom managerQuestionPath;

    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }
    private void OnButtonPressed()
    {
        ManagerAudio.Instance.PlaySFX("res://Audio/SystemSFX/Reveal.wav");
        managerQuestionPath.RevealAnswer();
    }
}
