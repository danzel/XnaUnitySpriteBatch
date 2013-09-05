using System;
using UnityEngine;

namespace Microsoft.Xna.Framework.Audio
{
	public class SoundEffect
	{
		private readonly AudioClip _audioClip;
		private AudioSource _audioSource;

		internal static readonly GameObject GameObject = new GameObject();

		public SoundEffect(AudioClip audioClip)
		{
			if (audioClip == null)
				throw new Exception("AudioClip is null");

			_audioClip = audioClip;
			_audioSource = GameObject.AddComponent<AudioSource>();
			_audioSource.clip = _audioClip;
		}

		public void Play()
		{
			Play(1, 0, 0);
		}

		public void Play(float volume, float pitch, float pan)
		{
			//todo: we need to pool our audio source objects like MonoGame does

			//ref http://answers.unity3d.com/questions/55023/how-does-audiosourcepitch-changes-pitch.html

			_audioSource.volume = volume;
			_audioSource.pan = pan;
			_audioSource.pitch = Mathf.Pow(2, pitch);
			_audioSource.Play();
		}
	}
}
