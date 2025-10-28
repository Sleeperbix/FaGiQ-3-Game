using Godot;
using System;
using System.Collections.Generic;


public partial class PlayerSetup : Control
{
    private List<LineEdit> nameInputs = new();
    private List<ColorPickerButton> colourInputs = new();
    public override void _Ready()
    {
        for (int i = 1; i <= 8; i++)
        {
            int playerIndex = i - 1;

            string namePath = $"LeftColumn/VBoxContainer/PlayerNameColourBox/HBoxs/HBox{i}/LineEdit";
            if (HasNode(namePath))
            {
                var lineEdit = GetNode<LineEdit>(namePath);
                nameInputs.Add(lineEdit);                

                lineEdit.Text = ManagerGame.playerNames[playerIndex];
                lineEdit.TextSubmitted += newText => OnNameSubmitted(playerIndex, newText);
            }

            string colourPath = $"LeftColumn/VBoxContainer/PlayerNameColourBox/HBoxs/HBox{i}/ColorPicker";
            if (HasNode(colourPath))
            {
                var colourPicker = GetNode<ColorPickerButton>(colourPath);
                colourInputs.Add(colourPicker);

                colourPicker.Color = ManagerGame.playerColours[playerIndex];
                colourPicker.ColorChanged += newColour => onColourChanged(playerIndex, newColour);
            }
        }
    }

    private void OnNameSubmitted(int playerIndex, string newName)
    {
        ManagerGame.playerNames[playerIndex] = newName;
        RefreshTabs(playerIndex);        
    }

    private void onColourChanged(int playerIndex, Color newColour)
    {
        ManagerGame.playerColours[playerIndex] = newColour;
        RefreshTabs(playerIndex);
        RefreshTokens(playerIndex);
    }

    private void RefreshTabs(int playerIndex)
    {
        var playerTabs = GetTree().GetNodesInGroup("PlayerTabs");
        foreach (var node in playerTabs)
        {
            if (node is PlayerTab tab && tab.playerIndex == playerIndex)
                tab.RefreshDisplay();
        }
    }
    
    private void RefreshTokens(int playerIndex)
    {
        Color newColour = ManagerGame.playerColours[playerIndex];

        var tokens = GetTree().GetNodesInGroup("PlayerTokens");
        foreach (PlayerToken token in tokens)
        {
            if (token.playerIndex == playerIndex)
            {
                Color fill = newColour;
                float luminance = (0.299f * fill.R + 0.587f * fill.G + 0.114f * fill.B);
                Color outline = luminance > 0.5f ? Colors.Black : Colors.White;

                token.SetColours(fill, outline);
            }
        }
    }
}
