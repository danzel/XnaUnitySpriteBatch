using System;
using System.Diagnostics;

namespace Microsoft.Xna.Framework.Graphics
{
	public class GraphicsDevice : IDisposable
	{
		public Texture2D[] Textures = new Texture2D[1];
		public Matrix Matrix;

		internal GraphicsDevice(Viewport viewport)
		{
			Viewport = viewport;
		}

		public Viewport Viewport { get; private set; }

		public void DrawUserIndexedPrimitives(PrimitiveType primitiveType, VertexPositionColorTexture[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration)
		{
			Debug.Assert(vertexData != null && vertexData.Length > 0, "The vertexData must not be null or zero length!");
			Debug.Assert(indexData != null && indexData.Length > 0, "The indexData must not be null or zero length!");

			throw new NotImplementedException("Not implemented");
		}


		public void Clear(Color color)
		{
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
