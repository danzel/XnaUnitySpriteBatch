using System;
using UnityEngine;

namespace Microsoft.Xna.Framework.Audio
{
	public class SoundEffect
	{
		private readonly AudioClip _audioClip;
		private AudioSource _audioSource;

		private static GameObject _gameObject = new GameObject();

		public SoundEffect(AudioClip audioClip)
		{
			if (audioClip == null)
				throw new Exception("AudioClip is null");

			_audioClip = audioClip;
			_audioSource = _gameObject.AddComponent<AudioSource>();
			_audioSource.clip = _audioClip;
		}

		public void Play()
		{
			//todo: I think we will need to pool our audio source objects like MonoGame does in
			_audioSource.Play();
		}

		public void Play(float volume, float pitch, float pan)
		{
			Play(); //todo: pitch/pan
		}
	}
}
