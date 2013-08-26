using System;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
	public class Game : IDisposable
	{
		public bool IsMouseVisible { get; set; }
		public GraphicsDevice GraphicsDevice { get; private set; }

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		protected virtual void Initialize()
		{
			throw new NotImplementedException();
		}

		protected virtual void LoadContent()
		{
			throw new NotImplementedException();
		}

		protected virtual void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		protected virtual void Draw(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		protected virtual void OnDeactivated(object sender, EventArgs args)
		{
			throw new NotImplementedException();
		}

		protected virtual void OnActivated(object sender, EventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
