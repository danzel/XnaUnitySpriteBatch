using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
	public class GraphicsDeviceManager
	{
		public GraphicsDeviceManager(Game game)
		{
			GraphicsDevice = game.GraphicsDevice;
		}

		public DisplayOrientation SupportedOrientations { get; set; }
		public bool IsFullScreen { get; set; }
		public GraphicsDevice GraphicsDevice { get; private set; }
	}
}
