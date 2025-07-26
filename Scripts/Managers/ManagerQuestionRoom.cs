using Godot;
using System;

public partial class ManagerQuestionRoom : Control
{
	[Export] private Control questionGUI;
	[Export] private Control topBarGUI;

	// Text labels, question text, answers etc.
	private RichTextLabel guiCategoryText;
	private RichTextLabel guiQuestionText;
	private RichTextLabel guiAnswerA;
	private RichTextLabel guiAnswerB;
	private RichTextLabel guiAnswerC;
	private RichTextLabel guiAnswerD;
	private TextureRect guiQuestionBG;


	public override void _Ready()
	{
		// Retrieve the selected question from ManagerGame
		var question = ManagerGame.SelectedQuestion;

		// Locate fields
		guiCategoryText = topBarGUI.GetNode<RichTextLabel>("TopBarBG/TopBarText");
		guiQuestionText = questionGUI.GetNode<RichTextLabel>("ColorRect/QuestionContainer/QuestionText");
		guiAnswerA = questionGUI.GetNode<RichTextLabel>("ColorRect/QuestionContainer/AnswerA");
		guiAnswerB = questionGUI.GetNode<RichTextLabel>("ColorRect/QuestionContainer/AnswerB");
		guiAnswerC = questionGUI.GetNode<RichTextLabel>("ColorRect/QuestionContainer/AnswerC");
		guiAnswerD = questionGUI.GetNode<RichTextLabel>("ColorRect/QuestionContainer/AnswerD");
		//guiCorrectAnswer
		//guiAnswerFact
		//guiQuestionImage
		guiQuestionBG = questionGUI.GetNode<TextureRect>("QuestionBG");

		// Assign fields
		guiCategoryText.Text = question.CategoryTitle;
		guiQuestionText.Text = question.QuestionText;
		guiAnswerA.Text = question.AnswerA;
		guiAnswerB.Text = question.AnswerB;
		guiAnswerC.Text = question.AnswerC;
		guiAnswerD.Text = question.AnswerD;
		//guiCorrectAnswer
		//guiAnswerFact
		//guiQuestionImage
		
		string imagePath = "res://Assets/" + question.BGImage;
		Texture2D imageTexture = GD.Load<Texture2D>(imagePath);
		GD.Print(imagePath);
		guiQuestionBG.Texture = imageTexture;
		// if (imageTexture == null)
		// {
		// 	GD.Print("Texture failed to load!!");
		// }
		// else
		// {
		// 	GD.Print("Texture Loaded!");
		// }
		string musicPath = "res://Assets/" + question.BGMusic;
		ManagerAudio.Instance.PlayMusicLoop(musicPath);




		if (question != null)
		{
			GD.Print("Loaded question category: " + question.CategoryTitle);
		}
		else
		{
			GD.Print("SelectedQuestion is null!");
		}

		if (questionGUI != null)
		{
			GD.Print("Found Question GUI");
		}
	}
}
