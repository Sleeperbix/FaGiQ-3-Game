using Godot;
using System;

public partial class BackgroundWander : Node
{
    [Export] public TextureRect backgroundImage;
    [Export] public float moveDuration = 5.0f;

    private Random random = new();

    public override void _Ready()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetNode<TextureRect>("BGImage");
        }

        Wander();
    }

    private void Wander()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        Vector2 imageSize = backgroundImage.Size;

        Vector2 maxOffset = (imageSize - screenSize) / 2f;

        maxOffset.X = Mathf.Max(0, maxOffset.X);
        maxOffset.Y = Mathf.Max(0, maxOffset.Y);

        Vector2 targetOffset = new Vector2((float)(random.NextDouble() * 2 - 1) * maxOffset.X,
                                           (float)(random.NextDouble() * 2 - 1) * maxOffset.Y
        );

        Vector2 centredPosition = (screenSize - imageSize) / 2f;
        Vector2 targetPosition = centredPosition + targetOffset;

        Tween tween = CreateTween();
        tween.TweenProperty(backgroundImage, "position", targetPosition, moveDuration)
            .SetTrans(Tween.TransitionType.Sine)
            .SetEase(Tween.EaseType.InOut);

        tween.TweenCallback(Callable.From(() => { Wander(); }));
    }
}
