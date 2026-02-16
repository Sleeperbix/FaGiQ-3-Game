using Godot;
using System.Collections.Generic;

public partial class PlayerBar : Control
{
    [Export] private PackedScene playerTabPrefab;
    [Export] private PackedScene playerTokenPrefab;

    private HBoxContainer playerContainer;
    private List<PlayerTab> playerTabs = new();
    private List<PlayerToken> playerTokens = new();

    public override void _Ready()
    {
        playerContainer = GetNode<HBoxContainer>("PlayerContainer");
        RefreshPlayers(ManagerGame.playerCount);
    }

    public void RefreshPlayers(int count)
    {
        // Clear all players tabs/tokens, then recreate.
        ClearPlayers();
        playerContainer = GetNode<HBoxContainer>("PlayerContainer");

        for (int i = 0; i < count; i++)
        {
            var tab = playerTabPrefab.Instantiate<PlayerTab>();
            tab.Initalize(i);
            tab.playerName = ManagerGame.playerNames[i];
            tab.playerColour = ManagerGame.playerColours[i];
            tab.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            playerContainer.AddChild(tab);
            playerTabs.Add(tab);
        }        
        CallDeferred(nameof(CreateTokens));            
        
    }

    private void CreateTokens()
    {
        for (int i = 0; i < playerTabs.Count; i++)
        {
            var token = playerTokenPrefab.Instantiate<PlayerToken>();
            token.Initalize(i);

            GetTree().CurrentScene.CallDeferred("add_child", token);

            playerTokens.Add(token);
        }
        PositionTokensEvenly();
    }

    private void PositionTokensEvenly()
    {
        int playerCount = playerTokens.Count;
        Vector2 windowSize = GetViewport().GetVisibleRect().Size;
        float screenWidth = windowSize.X;
        float screenHeight = windowSize.Y;

        float slotWidth = screenWidth / playerCount;
        float yPos = screenHeight - 115f; // Set number to height from bottom of screen.

        for (int i = 0; i < playerCount; i++)
        {
            var token = playerTokens[i];
            float xPos = (slotWidth * i) + (slotWidth / 2f);
            token.GlobalPosition = new Vector2(xPos, yPos);
            if (!ManagerGame.playerTokensActive)
            {
                token.Visible = false;
            }
        }
    }

    
    public void ClearPlayers()
    {
        foreach (Node child in playerContainer.GetChildren())
            child.QueueFree();
        playerTabs.Clear();
        var tokens = GetTree().GetNodesInGroup("PlayerTokens");
        foreach (var node in tokens)
        {
            if (node is PlayerToken token)
                token.QueueFree();
        }
        playerTokens.Clear();
    }
}
