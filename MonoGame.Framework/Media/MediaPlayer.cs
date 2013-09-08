using Microsoft.Xna.Framework.Audio;
using UnityEngine;

namespace Microsoft.Xna.Framework.Media
{
	public static class MediaPlayer
	{
		private static readonly AudioSource AudioSource = SoundEffect.GameObject.AddComponent<AudioSource>();
		public static MediaState State { get; private set; }

		public static void Play(Song song)
		{
			AudioSource.Stop();
			AudioSource.clip = song.AudioClip;
			AudioSource.Play();
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
		}

		public static void Resume()
		{
			AudioSource.Play();
		}

		public static void Pause()
		{
			AudioSource.Pause();
		}
	}
}
