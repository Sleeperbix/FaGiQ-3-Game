using Godot;
using System;

public partial class PlayerCountButton : Button
{
    [Export] public int playerCount;
    [Export] private NodePath playerBarPath;
    private PlayerBar playerBar;

    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        ManagerGame.playerCount = playerCount;
        var playerBar = GetNode<PlayerBar>(playerBarPath);
        if (playerBar != null)
        {
            playerBar.RefreshPlayers(playerCount);
        }
        else
        {
            GD.Print("Can't find PlayerBar");
        }
    }
}
