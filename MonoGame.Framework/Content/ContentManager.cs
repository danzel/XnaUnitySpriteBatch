using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using UnityTexture = UnityEngine.Texture2D;
using UnityAudioClip = UnityEngine.AudioClip;
using UnityResources = UnityEngine.Resources;
using TextAsset = UnityEngine.TextAsset;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
	public class ContentManager
	{
		public string RootDirectory { get; set; }

		public T Load<T>(string fileName) where T : class
		{
			if (typeof(T) == typeof(Texture2D))
			{
				return new Texture2D(NativeLoad<UnityTexture>(fileName)) as T;
			}
			if (typeof(T) == typeof(SoundEffect))
			{
				return new SoundEffect(NativeLoad<UnityAudioClip>(fileName)) as T;
			}
			if (typeof(T) == typeof(Song))
			{
				return new Song(NativeLoad<UnityAudioClip>(fileName)) as T;
			}
			if (typeof(T) == typeof(string))
			{
				return (NativeLoad<TextAsset>(fileName)).text as T;
			}
			//throw new Exception();
			return default(T);
		}

		private T NativeLoad<T>(string fileName) where T : class
		{
			var res = UnityResources.Load(fileName, typeof(T)) as T;
			if (res == null)
			{
				throw new Exception("Failed to load " + fileName + " as " + typeof(T));
			}
			return res;
		}
	}
}
