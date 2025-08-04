using Godot;
using System;
using System.Collections.Generic;

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
	private RichTextLabel guiFact;
	private ColorRect guiFactBG;
	private TextureRect guiQuestionImage;
	private TextureRect guiQuestionBG;

	private C_QuestionMultipleChoice question;


	public override void _Ready()
	{
		// Retrieve the selected question from ManagerGame
		question = ManagerGame.SelectedQuestion;

		// Locate fields
		guiCategoryText = topBarGUI.GetNode<RichTextLabel>("TopBarBG/TopBarText");
		guiQuestionText = questionGUI.GetNode<RichTextLabel>("QuestionRect/QuestionText");
		guiAnswerA = questionGUI.GetNode<RichTextLabel>("AnswerRect/QuestionContainer/AnswerA");
		guiAnswerB = questionGUI.GetNode<RichTextLabel>("AnswerRect/QuestionContainer/AnswerB");
		guiAnswerC = questionGUI.GetNode<RichTextLabel>("AnswerRect/QuestionContainer/AnswerC");
		guiAnswerD = questionGUI.GetNode<RichTextLabel>("AnswerRect/QuestionContainer/AnswerD");
		guiFact = questionGUI.GetNode<RichTextLabel>("FactRect/FactText");
		guiFactBG = questionGUI.GetNode<ColorRect>("FactRect");
		guiQuestionImage = questionGUI.GetNode<TextureRect>("ImageRect/QuestionImage");
		guiQuestionBG = questionGUI.GetNode<TextureRect>("QuestionBG");

		// Assign fields
		guiCategoryText.Text = question.CategoryTitle.ToUpper();
		guiQuestionText.Text = question.QuestionText;
		guiAnswerA.Text = "[A]  -  " + question.AnswerA;
		guiAnswerB.Text = "[B]  -  " + question.AnswerB;
		guiAnswerC.Text = "[C]  -  " + question.AnswerC;
		guiAnswerD.Text = "[D]  -  " + question.AnswerD;
		guiFact.Text = question.AnswerFact;
		guiFactBG.Visible = false;

		string imagePath_QuestionImage = "res://Assets/" + question.QuestionImage;
		Texture2D imageTexture_QuestionImage = GD.Load<Texture2D>(imagePath_QuestionImage);
		guiQuestionImage.Texture = imageTexture_QuestionImage;

		string imagePath_BGImage = "res://Assets/" + question.BGImage;
		Texture2D imageTexture_BGImage = GD.Load<Texture2D>(imagePath_BGImage);
		guiQuestionBG.Texture = imageTexture_BGImage;

		string musicPath = "res://Assets/" + question.BGMusic;
		ManagerAudio.Instance.PlayMusicLoop(musicPath);

		// if (question != null)
		// {
		// 	GD.Print("Loaded question category: " + question.CategoryTitle);
		// }
		// else
		// {
		// 	GD.Print("SelectedQuestion is null!");
		// }

		// if (questionGUI != null)
		// {
		// 	GD.Print("Found Question GUI");
		// }
	}

	public void RevealAnswer()
	{
		Dictionary<string, RichTextLabel> answerLabels = new()
		{
			{ "A", guiAnswerA },
			{ "B", guiAnswerB },
			{ "C", guiAnswerC },
			{ "D", guiAnswerD }
		};

		string correctAnswer = question.CorrectAnswer.ToUpper();

		foreach (var answer in answerLabels)
		{
			if (answer.Key == correctAnswer)
				answer.Value.Modulate = new Color(1, 1, 1, 1);
			else
				answer.Value.Modulate = new Color(1, 1, 1, 0.1f);
		}
		guiFactBG.Visible = true;
		TypeFact(question.AnswerFact);
	}

	public async void TypeFact(string text)
	{
		guiFact.Text = "";

		foreach (char c in text)
		{
			guiFact.Text += c;
			await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
		}


	}
}
