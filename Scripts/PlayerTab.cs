using Godot;
using System;

public partial class PlayerTab : Control
{
    [Export] public string playerName;
    [Export] public Color playerColour;

    private RichTextLabel labelName;
    private RichTextLabel labelScore;
    private Button buttonPlus;
    private Button buttonMinus;
    private ColorRect background;

    private int playerIndex;

    public void Initalize(int index)
    {
        playerIndex = index;
    }


    public override void _Ready()
    {
        // Find
        labelName = GetNode<RichTextLabel>("PlayerBG/VBoxContainer/PlayerNameText");
        labelScore = GetNode<RichTextLabel>("PlayerBG/VBoxContainer/HBoxContainer/PlayerScoreText");
        buttonPlus = GetNode<Button>("PlayerBG/VBoxContainer/HBoxContainer/ButtonPlus");
        buttonMinus = GetNode<Button>("PlayerBG/VBoxContainer/HBoxContainer/ButtonMinus");
        background = GetNode<ColorRect>("PlayerBG");

        // Assign

        labelName.Text = playerName;
        labelScore.Text = ManagerGame.playerScores[playerIndex].ToString();
        background.Color = playerColour;

        buttonPlus.Pressed += OnPlusPressed;
        buttonMinus.Pressed += OnMinusPressed;

    }

    private void OnPlusPressed()
    {
        ManagerGame.playerScores[playerIndex]++;
        labelScore.Text = ManagerGame.playerScores[playerIndex].ToString();
    }

    private void OnMinusPressed()
    {
        ManagerGame.playerScores[playerIndex]--;
        labelScore.Text = ManagerGame.playerScores[playerIndex].ToString();
    }
}
