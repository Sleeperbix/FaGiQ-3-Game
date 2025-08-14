using Godot;
using System;

public partial class ManagerAudio : Node
{
	public static ManagerAudio Instance;
	private AudioStreamPlayer musicPlayer;
	private AudioStreamPlayer sfxPlayer;

	public override void _Ready()
	{
		Instance = this;
		musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
		sfxPlayer = GetNode<AudioStreamPlayer>("SFXPlayer");
	}

	public void PlayMusic(string import)
	{
		AudioStream music = GD.Load<AudioStream>(import);
		musicPlayer.Stream = music;
		musicPlayer.Play();
	}

	public void PlayMusicLoop(string import)
	{
		AudioStream music = GD.Load<AudioStream>(import);
		if (music is AudioStreamMP3 mp3) { mp3.Loop = true; }
		musicPlayer.Stream = music;
		musicPlayer.Play();
	}

	public void PlaySFX(string import)
	{
		AudioStream sound = GD.Load<AudioStream>(import);
		sfxPlayer.Stream = sound;
		sfxPlayer.Play();
	}

	public void StopMusic()
	{
		musicPlayer.Stop();
	}
}
