using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
using UnityGraphics = UnityEngine.Graphics;

namespace Microsoft.Xna.Framework.Graphics
{
	public class GraphicsDevice : IDisposable
	{
		public Texture2D[] Textures = new Texture2D[1];
		private Matrix4x4 _matrix;
		private Matrix4x4 _baseMatrix;

		internal GraphicsDevice(Viewport viewport)
		{
			Viewport = viewport;
			_baseMatrix = Matrix4x4.TRS(new UnityEngine.Vector3(-viewport.Width / 2, viewport.Height / 2, 0), UnityEngine.Quaternion.identity, new UnityEngine.Vector3(1, -1, 1));
		}

		public Viewport Viewport { get; private set; }
		public Matrix Matrix {
			set
			{
				for (var i = 0; i < 4 * 4; i++)
				{
					_matrix[i] = value[i];
				}
				_matrix = _baseMatrix * _matrix;
			}
		}

		private readonly MaterialPool _materialPool = new MaterialPool();
		private readonly MeshPool _meshPool = new MeshPool();

		public void DrawUserIndexedPrimitives(PrimitiveType primitiveType, VertexPositionColorTexture[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration)
		{
			Debug.Assert(vertexData != null && vertexData.Length > 0, "The vertexData must not be null or zero length!");
			Debug.Assert(indexData != null && indexData.Length > 0, "The indexData must not be null or zero length!");

			var material = _materialPool.Get();
			material.mainTexture = Textures[0].Texture;

			var mesh = _meshPool.Get();
			mesh.Clear(); //todo: check the parameter
			mesh.vertices = GetVector3(vertexData, numVertices);
			mesh.uv = GetUV(vertexData, numVertices);
			mesh.colors = GetColor(vertexData, numVertices);
			mesh.triangles = GetIndex(indexData, primitiveCount * 3); //hack we know triangles have three
			
			//todo: check if necessary
			mesh.RecalculateNormals();

			UnityGraphics.DrawMesh(mesh, _matrix, material, 0);
		}

		private UnityEngine.Vector3[] GetVector3(VertexPositionColorTexture[] vertexData, int count)
		{
			var res = new UnityEngine.Vector3[count];
			for (int i = 0; i < count; i++)
			{
				var v = vertexData[i].Position;
				res[i] = new UnityEngine.Vector3(v.X, v.Y, v.Z);
			}
			return res;
		}

		private UnityEngine.Vector2[] GetUV(VertexPositionColorTexture[] vertexData, int count)
		{
			var res = new UnityEngine.Vector2[count];
			for (int i = 0; i < count; i++)
			{
				var v = vertexData[i].TextureCoordinate;
				res[i] = new UnityEngine.Vector2(v.X, 1 - v.Y);
			}
			return res;
		}

		private UnityEngine.Color[] GetColor(VertexPositionColorTexture[] vertexData, int count)
		{
			var res = new UnityEngine.Color[count];
			for (int i = 0; i < count; i++)
			{
				var v = vertexData[i].Color.ToVector4();
				res[i] = new UnityEngine.Color(v.X / v.W, v.Y / v.W, v.Z / v.W, v.W);
			}
			return res;
		}

		private int[] GetIndex(short[] indexes, int count)
		{
			var res = new int[count];
			for (int i = 0; i < count; i++)
			{
				res[i] = indexes[i];
			}
			return res;
		}

		public void ResetPools()
		{
			_materialPool.Reset();
		}

		public void Clear(Color color)
		{
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private class MaterialPool
		{
			private List<Material> _materials = new List<Material>();
			private int _index;

			public Material Get()
			{
				if (_materials.Count == _index)
				{
					_materials.Add(new Material(Shader.Find("Custom/SpriteShader")));
					_materials[_index].renderQueue += _index;
				}

				_index++;
				return _materials[_index - 1];
			}

			public void Reset()
			{
				_index = 0;
			}
		}
		private class MeshPool
		{
			private List<Mesh> _meshes = new List<Mesh>();
			private int _index;

			public Mesh Get()
			{
				if (_meshes.Count == _index)
				{
					_meshes.Add(new Mesh());
				}

				_index++;
				return _meshes[_index - 1];
			}

			public void Reset()
			{
				_index = 0;
			}
		}
	}
}
