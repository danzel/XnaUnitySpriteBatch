using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using UnityEngine;

namespace Microsoft.Xna.Framework
{
	public class Game : IDisposable
	{
		public bool IsMouseVisible { get; set; }
		public GraphicsDevice GraphicsDevice { get; private set; }

		internal static Game Instance { get; private set; }
		public GameWindow Window { get { return _window; } }
		public ContentManager Content { get; private set; }

		private readonly GameTime _gameTime = new GameTime(new TimeSpan(), TimeSpan.FromSeconds(Time.fixedDeltaTime));
		private readonly TimeSpan _fixedDeltaTime = TimeSpan.FromSeconds(Time.fixedDeltaTime);

		public Game()
		{
			GraphicsDevice = new GraphicsDevice(new Viewport(0, 0, Screen.width, Screen.height));
			Content = new ContentManager();

			_window = new UnityGameWindow(GraphicsDevice);
		}

		/// <summary>
		/// Must be called by Unity in a FixedUpdate call
		/// </summary>
		public void UnityFixedUpdate()
		{
			_gameTime.TotalGameTime = _gameTime.TotalGameTime.Add(_fixedDeltaTime);
			Update(_gameTime);
		}

		/// <summary>
		/// Must be called by Unity in a Update call
		/// </summary>
		public void UnityUpdate()
		{
			_window.Update();
			UpdateInput();

			GraphicsDevice.ResetPools();
			Draw(_gameTime);
		}

		public void UnityInitialize()
		{
			Initialize();
			LoadContent();
		}

		private bool _mouseIsDown = false;
		private UnityGameWindow _window;

		private const int MouseId = int.MinValue;

		private void UpdateInput()
		{
			bool mouseIsDown = UnityEngine.Input.GetMouseButton(0);
			if (!_mouseIsDown && mouseIsDown)
				TouchPanel.AddEvent(MouseId, TouchLocationState.Pressed, ToMonoGame(UnityEngine.Input.mousePosition));
			else if (_mouseIsDown && !mouseIsDown)
				TouchPanel.AddEvent(MouseId, TouchLocationState.Released, ToMonoGame(UnityEngine.Input.mousePosition));
			else if (_mouseIsDown)
				TouchPanel.AddEvent(MouseId, TouchLocationState.Moved, ToMonoGame(UnityEngine.Input.mousePosition));
			_mouseIsDown = mouseIsDown;

			//todo: multitouch


			//UnityEngine.Input.GetMouseButton(0);
			//UnityEngine.Input.mousePosition

			//UnityEngine.Input.touches[0].fingerId
		}

		private Vector2 ToMonoGame(UnityEngine.Vector3 vec)
		{
			return new Vector2(vec.x, Screen.height - vec.y);
		}

		public void Dispose()
		{
		}

		protected virtual void Initialize()
		{
		}

		protected virtual void LoadContent()
		{
		}

		protected virtual void Update(GameTime gameTime)
		{
		}

		protected virtual void Draw(GameTime gameTime)
		{
		}

		//TODO: work out when to call these from unity
		protected virtual void OnDeactivated(object sender, EventArgs args)
		{
		}

		protected virtual void OnActivated(object sender, EventArgs args)
		{
		}
	}
}
