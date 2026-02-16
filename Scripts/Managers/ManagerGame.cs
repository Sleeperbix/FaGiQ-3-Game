using Godot;
using System;
using System.Collections.Generic;

public partial class ManagerGame : Node
{
    public static ManagerGame Instance;

    public static C_QuestionMultipleChoice SelectedQuestion;
    public static List<string> activeMultipleChoiceCategories;
    public SceneFader SceneFader { get; private set; }


    // Player Stuff
    public static int playerCount = 8;
    public static bool playerTokensActive = true;
    public static List<string> playerNames = new List<string> { "The", "Same", "Old", "Fears", "Wish", "You", "Were", "Here" };
    public static List<Color> playerColours = new List<Color> { new("#ff0000"), new("#0000ff"), new("#ffff00"), new("#00ff00"),
                                                                new("#ff00ff"), new("#ff9100"), new("#c4c4c4"), new("#474747") };

    public static List<int> playerScores = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
    

    public override void _Ready()
    {
        Instance = this;
        SceneFader = GetParent().GetNode<SceneFader>("SceneFader");
        if (SceneFader == null)
        {
            GD.PrintErr("SceneFader not assigned to ManagerGame!");
        }
    }
    public void SetSelectedQuestion(C_QuestionMultipleChoice question)
    {
        SelectedQuestion = question;
    }

    public void SetMultipleChoiceCategories(List<string> categories)
    {
        activeMultipleChoiceCategories = categories;        
    }

    public async void TransitionToScene(string scenePath)
    {
        if (SceneFader != null)
        {
            await SceneFader.FadeToScene(scenePath);
        }
    }
}
