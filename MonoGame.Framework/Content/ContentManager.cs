using UnityTexture = UnityEngine.Texture;
using UnityResources = UnityEngine.Resources;
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
				return new Texture2D((UnityTexture)UnityResources.Load(fileName)) as T;
			}

			return default(T);
		}
	}
}
