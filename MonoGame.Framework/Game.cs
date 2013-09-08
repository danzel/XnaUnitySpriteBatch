using System;
using System.Diagnostics;
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
		/// Must be called by Unity in a Update call
		/// </summary>
		public void UnityUpdate()
		{
			PreUpdate();

			RunUpdates();

			GraphicsDevice.ResetPools();
			Draw(_gameTime);
		}

		private readonly TimeSpan _targetElapsedTime = TimeSpan.FromTicks((long)10000000 / (long)60);
		private readonly TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500);

		private readonly Stopwatch _gameTimer = Stopwatch.StartNew();
		private TimeSpan _accumulatedElapsedTime;

		//Taken from MonoGame.Game.Tick
		private void RunUpdates()
		{
			_gameTime.ElapsedGameTime = _targetElapsedTime;
			var stepCount = 0;

			// Advance the accumulated elapsed time.
			_accumulatedElapsedTime += _gameTimer.Elapsed;
			_gameTimer.Reset();
			_gameTimer.Start();
			// Perform as many full fixed length time steps as we can.
			while (_accumulatedElapsedTime >= _targetElapsedTime)
			{
				_gameTime.TotalGameTime += _targetElapsedTime;
				_accumulatedElapsedTime -= _targetElapsedTime;
				++stepCount;

				Update(_gameTime);
			}
			// Draw needs to know the total elapsed time
			// that occured for the fixed length updates.
			_gameTime.ElapsedGameTime = TimeSpan.FromTicks(_targetElapsedTime.Ticks * stepCount);
		}

		private void PreUpdate()
		{
			_window.Update();
			UpdateInput();
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

		public void Exit()
		{
			Application.Quit();
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
