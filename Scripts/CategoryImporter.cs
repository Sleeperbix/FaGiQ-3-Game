using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

public partial class CategoryImporter : Control
{
    [Export] private Button searchButton;
    [Export] private Button tickAllButton;
    [Export] private Button untickAllButton;
    [Export] private VBoxContainer resultsBox;
    [Export] private Label infoLabel;
    private string filePath = ProjectSettings.GlobalizePath("res://Assets/Questions.txt");

    public override void _Ready()
    {
        searchButton.Pressed += OnSearchPressed;
        searchButton.MouseEntered += () => OnButtonHovered("Search all directories in filepath location.");
        searchButton.MouseExited += () => OnButtonHovered("...");

        tickAllButton.Pressed += OnTickAllPressed;
        tickAllButton.MouseEntered += () => OnButtonHovered("Enable all categories in list.");
        tickAllButton.MouseExited += () => OnButtonHovered("...");

        untickAllButton.Pressed += OnUntickAllPressed;
        untickAllButton.MouseEntered += () => OnButtonHovered("Disable all categories in list.");
        untickAllButton.MouseExited += () => OnButtonHovered("...");
        
        List<string> categoriesFromFile = ReadCategoriesFromTxt();        
        DisplayCategories(categoriesFromFile);
    }


    private List<string> ReadCategoriesFromTxt()
    {
        var categories = new List<string>();

        try
        {
            if (!File.Exists(filePath))
            {
                GD.Print($"File not found: {filePath}");
                return categories;
            }
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                // Trim whitespace just in case
                var trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    categories.Add(trimmed);
                }
            }
        }
        catch (Exception ex)
        {
            GD.Print($"Error reading categories: {ex.Message}");
        }
        return categories;
    }

    private void OnSearchPressed()
    {
        string baseDir = ProjectSettings.GlobalizePath("res://Assets");
        List<string> foundCategories = FindCategoryJsons(baseDir);

        infoLabel.Text = $"Found {foundCategories.Count} categories.";

        foreach (Node child in resultsBox.GetChildren())
            child.QueueFree();

        ExportCategories(foundCategories);
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

    private void ExportCategories(List<string> foundCategories)
    {
        try
        {
            File.WriteAllLines(filePath, foundCategories);
            GD.Print($"Exported {foundCategories.Count} categories to {filePath}");
        }
        catch (Exception ex)
        {
            GD.Print($"Failed to export categories: {ex.Message}");
        }
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

    private void OnButtonHovered(string text)
    {
        infoLabel.Text = text;
    }

    private void OnTickAllPressed()
    {
        foreach (Node child in resultsBox.GetChildren())
        {
            if (child is CheckBox chk)
                chk.ButtonPressed = true;
        }
    }
    private void OnUntickAllPressed()
    {
        foreach (Node child in resultsBox.GetChildren())
        {
            if (child is CheckBox chk)
                chk.ButtonPressed = false;
        }
    }
}
