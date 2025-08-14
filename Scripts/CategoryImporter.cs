using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class CategoryImporter : Control
{
    [Export] private RichTextLabel statusLabel;
    [Export] private Button searchButton;
    [Export] private VBoxContainer resultsBox;
    [Export] private Label infoLabel;

    public override void _Ready()
    {
        searchButton.Pressed += OnSearchPressed;
    }

    private void OnSearchPressed()
    {
        string baseDir = ProjectSettings.GlobalizePath("res://Assets");
        List<string> foundCategories = FindCategoryJsons(baseDir);

        statusLabel.Text = $"Found {foundCategories.Count} categories.";

        foreach (Node child in resultsBox.GetChildren())
            child.QueueFree();

        DisplayCategories(foundCategories);
    }

    private List<string> FindCategoryJsons(string baseDir)
    {
        List<String> foundJsons = new List<string>();

        foreach (string dir in Directory.GetDirectories(baseDir))
        {
            string[] jsonFiles = Directory.GetFiles(dir, "*.json", SearchOption.TopDirectoryOnly);
            foreach (string json in jsonFiles)
            {
                string relativePath = Path.GetRelativePath(baseDir, json).Replace("\\", "/");
                foundJsons.Add(relativePath);
            }
        }
        return foundJsons;
    }

    private (string Title, string Author, int QuestionsCount, string Summary) GetCategoryInfo(string filePath)
    {
        string jsonText = File.ReadAllText(filePath);

        var json = new Json();
        json.Parse(jsonText);

        var parsed = json.Data.As<Godot.Collections.Dictionary>();

        string title = "(No Title)";
        string author = "(No Author)";
        string summary = "(No Summary)";
        int questionsCount = 0;

        if (parsed.ContainsKey("Global"))
        {
            var global = parsed["Global"].AsGodotDictionary();
            if (global.ContainsKey("CategoryTitle"))
                title = global["CategoryTitle"].ToString();
            if (global.ContainsKey("Author"))
                author = global["Author"].ToString();
            if (global.ContainsKey("Summary"))
                summary = global["Summary"].ToString();
        }
        if (parsed.ContainsKey("Questions"))
        {
            var questions = parsed["Questions"].AsGodotArray();
            questionsCount = questions.Count;
        }

        return (title, author, questionsCount, summary);
    }

    private void DisplayCategories(List<string> foundCategories)
    {
        string baseDir = ProjectSettings.GlobalizePath("res://Assets");

        foreach (string jsonPath in foundCategories)
        {
            string fullPath = Path.Combine(baseDir, jsonPath);
            var info = GetCategoryInfo(fullPath);

            CheckBox chk = new CheckBox();
            chk.Text = info.Title;
            chk.MouseEntered += () => OnCategoryHovered(info.Title, info.Author, info.QuestionsCount, info.Summary);

            resultsBox.AddChild(chk);
            GD.Print(chk.Text);
            GD.Print(chk.GlobalPosition);
        }
    }

    private void OnCategoryHovered(string title, string author, int questionsCount, string summary)
    {
        infoLabel.Text = $"Category Title: {title}\nAuthor: {author}\nTotal Questions: {questionsCount}\nSummary: {summary}";
    }
}
