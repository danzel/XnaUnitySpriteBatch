using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using UnityEngine;

namespace Microsoft.Xna.Framework
{
	internal class UnityGameWindow : GameWindow
	{
		private readonly GraphicsDevice _graphicsDevice;
		private int _width;
		private int _height;

		public override bool AllowUserResizing { get; set; }

		public override Rectangle ClientBounds
		{
			get { return new Rectangle(0, 0, Screen.width, Screen.height); }
		}

		public override DisplayOrientation CurrentOrientation
		{
			get
			{
				if (Screen.width > Screen.height)
					return DisplayOrientation.LandscapeLeft;
				return DisplayOrientation.Portrait;
			}
		}

		public override IntPtr Handle
		{
			get { throw new NotImplementedException(); }
		}

		public override string ScreenDeviceName
		{
			get { throw new NotImplementedException(); }
		}

		public override void BeginScreenDeviceChange(bool willBeFullScreen)
		{
			throw new NotImplementedException();
		}

		public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
		{
			throw new NotImplementedException();
		}

		protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
		{
			//throw new NotImplementedException();
		}

		protected override void SetTitle(string title)
		{
			throw new NotImplementedException();
		}

		internal UnityGameWindow(GraphicsDevice graphicsDevice)
		{
			_graphicsDevice = graphicsDevice;
			_width = Screen.width;
			_height = Screen.height;
		}

		internal void Update()
		{
			var width = Screen.width;
			var height = Screen.height;

			if (_width != width || _height != height)
			{
				_width = width;
				_height = height;

				_graphicsDevice.Viewport = new Viewport(0, 0, width, height);

				OnOrientationChanged();
			}
		}
	}
}
