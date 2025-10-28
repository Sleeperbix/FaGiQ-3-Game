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

    public int playerIndex;

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
        RefreshDisplay();

        buttonPlus.Pressed += OnPlusPressed;
        buttonMinus.Pressed += OnMinusPressed;

    }

    private void OnPlusPressed()
    {
        ManagerGame.playerScores[playerIndex]++;
        var textColour = GetContrastingTextColour(background.Color).ToHtml(false);
        labelScore.Text = $"[color=#{textColour}]{ManagerGame.playerScores[playerIndex]}[/color]";
    }

    private void OnMinusPressed()
    {
        ManagerGame.playerScores[playerIndex]--;
        var textColour = GetContrastingTextColour(background.Color).ToHtml(false);
        labelScore.Text = $"[color=#{textColour}]{ManagerGame.playerScores[playerIndex]}[/color]";
    }

    private Color GetContrastingTextColour(Color background)
    {
        float luminance = (0.299f * background.R + 0.587f * background.G + 0.114f * background.B);
        return luminance > 0.5f ? Colors.Black : Colors.White;
    }

    public void RefreshDisplay()
    {
        background.Color = ManagerGame.playerColours[playerIndex];        
        var textColour = GetContrastingTextColour(background.Color).ToHtml(false);
        labelScore.Text = $"[color=#{textColour}]{ManagerGame.playerScores[playerIndex]}[/color]";
        labelName.Text = $"[color=#{textColour}]{ManagerGame.playerNames[playerIndex]}";


    }
}
