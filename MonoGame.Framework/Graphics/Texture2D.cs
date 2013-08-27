using UnityTexture = UnityEngine.Texture;

namespace Microsoft.Xna.Framework.Graphics
{
	public class Texture2D
	{
		internal readonly UnityTexture Texture;

		public Texture2D(UnityTexture texture)
		{
			Texture = texture;
		}

		public int Width { get { return Texture.width; } }
		public int Height { get { return Texture.height; } }
	}
}
