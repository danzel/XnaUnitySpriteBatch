using System;
using UnityTexture = UnityEngine.Texture2D;
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
				return new Texture2D(UnityResources.Load(fileName, typeof(UnityTexture)) as UnityTexture) as T;
			}
			if (typeof(T) == typeof(string))
			{
				return ((TextAsset)UnityResources.Load(fileName, typeof(TextAsset))).text as T;
			}
			//throw new Exception();
			return default(T);
		}
	}
}
