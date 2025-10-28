using Godot;
using System.Collections.Generic;

public partial class PlayerBar : Control
{
    [Export] private PackedScene playerTabPrefab;
    [Export] private PackedScene playerTokenPrefab;

    private HBoxContainer playerContainer;
    private List<PlayerTab> playerTabs = new();
    private List<PlayerToken> playerTokens = new();

    [Export] private int numberOfPlayers = 8;

    public override void _Ready()
    {
        playerContainer = GetNode<HBoxContainer>("PlayerContainer");

        // --- Create all tabs immediately ---
        for (int i = 0; i < numberOfPlayers && i < 8; i++)
        {
            var tab = playerTabPrefab.Instantiate<PlayerTab>();
            tab.Initalize(i);
            tab.playerName = ManagerGame.playerNames[i];
            tab.playerColour = ManagerGame.playerColours[i];
            tab.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            playerContainer.AddChild(tab);
            playerTabs.Add(tab);
        }

        // --- Defer token creation ---
        CallDeferred(nameof(CreateTokens));
    }

    private void CreateTokens()
    {
        for (int i = 0; i < playerTabs.Count; i++)
        {
            var token = playerTokenPrefab.Instantiate<PlayerToken>();
            token.playerIndex = i;
            token.Visible = false;

            // Use deferred add_child to be extra safe
            GetTree().CurrentScene.CallDeferred("add_child", token);

            playerTokens.Add(token);
        }

        // Defer positioning until the next idle frame
        CallDeferred(nameof(PositionTokens));
    }

    private void PositionTokens()
    {
        for (int i = 0; i < playerTabs.Count; i++)
        {
            var tab = playerTabs[i];
            var token = playerTokens[i];

            Vector2 globalTabPos = tab.GetGlobalTransformWithCanvas().Origin;
            Rect2 rect = tab.GetRect();

            float offsetY = -10f;

            token.GlobalPosition = new Vector2(
                globalTabPos.X + rect.Size.X / 2f,
                globalTabPos.Y + offsetY
            );

            token.Visible = true;
        }
    }
}
