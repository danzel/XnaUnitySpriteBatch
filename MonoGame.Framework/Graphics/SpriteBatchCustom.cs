using System;

namespace Microsoft.Xna.Framework.Graphics
{
	public class SpriteBatchCustom
	{
		private Matrix _matrix;
		Rectangle _tempRect = new Rectangle(0, 0, 0, 0);
		Vector2 _texCoordTL = new Vector2(0, 0);
		Vector2 _texCoordBR = new Vector2(0, 0);

		SpriteSortMode _sortMode;

		public SpriteBatchCustom(GraphicsDevice graphicsDevice)
		{
			throw new NotImplementedException();
		}

		public void End()
		{
			throw new NotImplementedException();
		}

		public void Begin(SpriteSortMode spriteSortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix matrix)
		{
			_matrix = matrix;
		}

		public void Draw(Texture2D texture,
			Vector2? position = null,
			Rectangle? drawRectangle = null,
			Rectangle? sourceRectangle = null,
			Vector2? origin = null,
			float rotation = 0f,
			Vector2? scale = null,
			Color? color = null,
			SpriteEffects effect = SpriteEffects.None,
			float depth = 0f)
		{

			// Assign default values to null parameters here, as they are not compile-time constants
			if (!color.HasValue)
				color = Color.White;
			if (!origin.HasValue)
				origin = Vector2.Zero;
			if (!scale.HasValue)
				scale = Vector2.One;

			// If both drawRectangle and position are null, or if both have been assigned a value, raise an error
			if ((drawRectangle.HasValue) == (position.HasValue))
			{
				throw new InvalidOperationException("Expected drawRectangle or position, but received neither or both.");
			}
			else if (position != null)
			{
				// Call Draw() using position
				Draw(texture, (Vector2)position, sourceRectangle, (Color)color, rotation, (Vector2)origin, (Vector2)scale, effect, depth);
			}
			else
			{
				// Call Draw() using drawRectangle
				Draw(texture, (Rectangle)drawRectangle, sourceRectangle, (Color)color, rotation, (Vector2)origin, effect, depth);
			}
		}
		public void Draw(Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			Vector2 scale,
			SpriteEffects effect,
			float depth)
		{
			CheckValid(texture);

			var w = texture.Width * scale.X;
			var h = texture.Height * scale.Y;
			if (sourceRectangle.HasValue)
			{
				w = sourceRectangle.Value.Width * scale.X;
				h = sourceRectangle.Value.Height * scale.Y;
			}

			DrawInternal(texture,
				new Vector4(position.X, position.Y, w, h),
				sourceRectangle,
				color,
				rotation,
				origin * scale,
				effect,
				depth);
		}


		public void Draw(Texture2D texture,
			Rectangle destinationRectangle,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			SpriteEffects effect,
			float depth)
		{
			CheckValid(texture);

			DrawInternal(texture,
				new Vector4(destinationRectangle.X,
							destinationRectangle.Y,
							destinationRectangle.Width,
							destinationRectangle.Height),
				sourceRectangle,
				color,
				rotation,
				new Vector2(origin.X * ((float)destinationRectangle.Width / (float)((sourceRectangle.HasValue && sourceRectangle.Value.Width != 0) ? sourceRectangle.Value.Width : texture.Width)),
							origin.Y * ((float)destinationRectangle.Height) / (float)((sourceRectangle.HasValue && sourceRectangle.Value.Height != 0) ? sourceRectangle.Value.Height : texture.Height)),
				effect,
				depth);
		}

		internal void DrawInternal(Texture2D texture,
			Vector4 destinationRectangle,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			SpriteEffects effect,
			float depth)
		{
			var item = _batcher.CreateBatchItem();

			item.Depth = depth;
			item.Texture = texture;

			if (sourceRectangle.HasValue)
			{
				_tempRect = sourceRectangle.Value;
			}
			else
			{
				_tempRect.X = 0;
				_tempRect.Y = 0;
				_tempRect.Width = texture.Width;
				_tempRect.Height = texture.Height;
			}

			_texCoordTL.X = _tempRect.X / (float)texture.Width;
			_texCoordTL.Y = _tempRect.Y / (float)texture.Height;
			_texCoordBR.X = (_tempRect.X + _tempRect.Width) / (float)texture.Width;
			_texCoordBR.Y = (_tempRect.Y + _tempRect.Height) / (float)texture.Height;

			if ((effect & SpriteEffects.FlipVertically) != 0)
			{
				var temp = _texCoordBR.Y;
				_texCoordBR.Y = _texCoordTL.Y;
				_texCoordTL.Y = temp;
			}
			if ((effect & SpriteEffects.FlipHorizontally) != 0)
			{
				var temp = _texCoordBR.X;
				_texCoordBR.X = _texCoordTL.X;
				_texCoordTL.X = temp;
			}

			item.Set(destinationRectangle.X,
					destinationRectangle.Y,
					-origin.X,
					-origin.Y,
					destinationRectangle.Z,
					destinationRectangle.W,
					(float)Math.Sin(rotation),
					(float)Math.Cos(rotation),
					color,
					_texCoordTL,
					_texCoordBR);

			if (_sortMode == SpriteSortMode.Immediate)
				_batcher.DrawBatch(_sortMode);
		}

		void CheckValid(Texture2D texture)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");
			//if (!_beginCalled)
			//	throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
		}
	}
}
