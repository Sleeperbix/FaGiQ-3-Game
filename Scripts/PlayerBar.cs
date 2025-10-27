using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerBar : Node
{
    [Export] private PackedScene playerTabScene;
    [Export] private int numberOfPlayers = 4;

    private HBoxContainer playerContainer;

    public override void _Ready()
    {
        playerContainer = GetNode<HBoxContainer>("PlayerContainer");

        for (int i = 0; i < numberOfPlayers && i < 8; i++)
        {
            var tab = playerTabScene.Instantiate<PlayerTab>();
            tab.Initalize(i);
            tab.playerName = ManagerGame.playerNames[i];
            tab.playerColour = ManagerGame.playerColours[i];
            tab.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            playerContainer.AddChild(tab);
        }

    }

}
