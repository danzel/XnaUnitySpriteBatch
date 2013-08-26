using System;

namespace Microsoft.Xna.Framework.Graphics
{
	public class GraphicsDevice : IDisposable
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public Viewport Viewport { get; private set; }

		public void Clear(Color color)
		{
			throw new NotImplementedException();
		}
	}
}
