using Godot;

public partial class PlayerToken : Node2D
{
	[Export] public int playerIndex = 0;
	[Export] public float radius = 20f;
	[Export] public float outlineRadius = 1f;

	public Color fillColour;
	public Color outlineColour;

	private bool dragging = false;
	private Vector2 dragOffset = Vector2.Zero;
	private Label numberLabel;

	public override void _Ready()
	{
		SetProcessInput(true);
		// Get colours
		fillColour = ManagerGame.playerColours[playerIndex];
		outlineColour = GetContrastingTextColour(fillColour);
		
		 // Create number label
		numberLabel = new Label();
		numberLabel.Text = (playerIndex + 1).ToString();
		numberLabel.HorizontalAlignment = HorizontalAlignment.Center;
		numberLabel.VerticalAlignment = VerticalAlignment.Center;
		numberLabel.AddThemeColorOverride("font_color", outlineColour);
		numberLabel.CustomMinimumSize = new Vector2(radius * 2, radius * 2);
		numberLabel.Position = new Vector2(-radius, -radius);
		AddChild(numberLabel);
	}

	public void Initalize(int index)
	{
		//GD.Print($"{ManagerGame.playerCount} Initializing token for player {index}.");
		playerIndex = index;
		fillColour = ManagerGame.playerColours[index];
		outlineColour = GetContrastingTextColour(fillColour);
		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, radius + outlineRadius, outlineColour, antialiased:true);
		DrawCircle(Vector2.Zero, radius, fillColour, antialiased:true);
	}
	private Color GetContrastingTextColour(Color background)
	{
		float luminance = (0.299f * background.R + 0.587f * background.G + 0.114f * background.B);
		return luminance > 0.5f ? Colors.Black : Colors.White;
	}
	
	public void SetColours(Color fill, Color outline)
	{
		fillColour = fill;
		outlineColour = outline;
		numberLabel.AddThemeColorOverride("font_color", outlineColour);
		QueueRedraw(); 
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left)
			{
				if (mouseButton.Pressed)
				{
					Vector2 localPos = ToLocal(mouseButton.Position);
					if (localPos.Length() <= radius)
					{
						dragging = true;
						dragOffset = Position - mouseButton.Position;
					}
				}
				else
				{
					dragging = false;
				}
			}
		}
		else if (@event is InputEventMouseMotion mouseMotion && dragging)
		{
			Position = mouseMotion.Position + dragOffset;
		}
	}
}
