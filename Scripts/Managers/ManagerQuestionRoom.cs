using Godot;
using System;
using System.Collections.Generic;

public partial class ManagerQuestionRoom : Control
{
	[Export] private Control questionGUI;
	[Export] private Control topBarGUI;

	// Text labels, question text, answers etc.
	private C_QuestionMultipleChoice question;
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

	// Timer stuff

	[Export] public Timer questionTimer;
	[Export] public RichTextLabel timerText;
	[Export] public Button timerButton;
	[Export] public int timeLimit = 15;
	private int timeLeft;

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

		// Assign button and timer stuff

		questionTimer.Timeout += TimerTick;
		timerButton.Pressed += TimerStarted;
		TimerReset();

	}

	private void TimerStarted()
	{
		TimerReset();
		questionTimer.Start();		
	}

	private void TimerTick()
	{
		timeLeft--;
		timerText.Text = timeLeft.ToString();

		if (timeLeft <= 0)
		{
			questionTimer.Stop();
			ManagerAudio.Instance.PlaySFX("res://Audio/SystemSFX/Timer.wav");
			FlashTimer();
		}
	}

	private void FlashTimer()
	{
		Tween tween = CreateTween();
		timerText.Modulate = new Color(timerText.Modulate, 1f);

		float flashDuration = 0.1f;
		int flashCount = 3;

		for (int i = 0; i < flashCount; i++)
		{
			float delay = i * flashDuration * 2;

			tween.TweenProperty(timerText, "modulate:a", 0.3f, flashDuration)
				.SetDelay(delay)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
			
			tween.TweenProperty(timerText, "modulate:a", 1.0f, flashDuration)
				.SetDelay(delay + flashDuration)
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
		}
	}

	private void TimerReset()
	{
		timeLeft = timeLimit;
		timerText.Text = timeLeft.ToString();
		questionTimer.WaitTime = 1.0f;
		questionTimer.OneShot = false;
		questionTimer.Stop();
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
