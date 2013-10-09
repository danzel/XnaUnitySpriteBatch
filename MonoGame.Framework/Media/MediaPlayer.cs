using System;
using Microsoft.Xna.Framework.Audio;
using UnityEngine;

namespace Microsoft.Xna.Framework.Media
{
	public static class MediaPlayer
	{
		private static readonly AudioSource AudioSource = SoundEffect.GameObject.AddComponent<AudioSource>();

		private static MediaState _mediaState;
		public static MediaState State
		{
			get { return _mediaState; }
			private set
			{
				_mediaState = value;
				FireMediaStateChanged();
			}
		}

		private static float _currentSongRemainingSeconds;

		public static event EventHandler<EventArgs> MediaStateChanged;

		public static void Play(Song song)
		{
			AudioSource.Stop();
			AudioSource.clip = song.AudioClip;
			AudioSource.Play();
			_currentSongRemainingSeconds = song.AudioClip.length * 0.95f; // fudge it a bit as music stops when we lose focus
			State = MediaState.Playing;
		}

		public static float Volume
		{
			get
			{
				return AudioSource.volume;
			}
			set
			{
				AudioSource.volume = value;
			}
		}

		public static void Stop()
		{
			AudioSource.Stop();
			State = MediaState.Stopped;
		}

		public static void Resume()
		{
			AudioSource.Play();
			State = MediaState.Playing;
		}

		public static void Pause()
		{
			AudioSource.Pause();
			State = MediaState.Paused;
		}

		private static void FireMediaStateChanged()
		{
			if (MediaStateChanged != null)
				MediaStateChanged(null, EventArgs.Empty);
		}

		internal static void Update(float dt)
		{
			if (State == MediaState.Playing)
			{
				_currentSongRemainingSeconds -= dt;
				if (_currentSongRemainingSeconds <= 0 && !AudioSource.isPlaying)
				{
					State = MediaState.Stopped;
				}
			}
		}
	}
}
