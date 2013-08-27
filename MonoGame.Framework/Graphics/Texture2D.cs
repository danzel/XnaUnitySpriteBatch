using System;
using UnityTexture = UnityEngine.Texture2D;

namespace Microsoft.Xna.Framework.Graphics
{
	public class Texture2D
	{
		internal readonly UnityTexture Texture;

		public Texture2D(UnityTexture texture)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");
			Texture = texture;
		}

		public int Width { get { return Texture.width; } }
		public int Height { get { return Texture.height; } }
	}
}
