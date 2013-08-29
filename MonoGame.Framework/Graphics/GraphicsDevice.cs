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
		public Matrix Matrix
		{
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

			var material = _materialPool.Get(Textures[0]);

			var mesh = _meshPool.Get(primitiveCount / 2);
			mesh.Populate(vertexData, numVertices);

			UnityGraphics.DrawMesh(mesh.Mesh, _matrix, material, 0);
		}

		public void ResetPools()
		{
			_materialPool.Reset();
			_meshPool.Reset();
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
			private class MaterialHolder
			{
				public readonly Material Material;
				public readonly Texture2D Texture2D;

				public MaterialHolder(Material material, Texture2D texture2D)
				{
					Material = material;
					Texture2D = texture2D;
				}
			}

			private readonly List<MaterialHolder> _materials = new List<MaterialHolder>();
			private int _index;
			private readonly Shader _shader = Shader.Find("Custom/SpriteShader");

			private MaterialHolder Create(Texture2D texture)
			{
				var mat = new Material(_shader);
				mat.mainTexture = texture.Texture;
				mat.renderQueue += _materials.Count;
				return new MaterialHolder(mat, texture);
			}

			public Material Get(Texture2D texture)
			{
				while (_index < _materials.Count)
				{
					if (_materials[_index].Texture2D == texture)
					{
						_index++;
						return _materials[_index - 1].Material;
					}

					_index++;
				}

				var material = Create(texture);
				_materials.Add(material);
				_index++;
				return _materials[_index - 1].Material;
			}

			public void Reset()
			{
				_index = 0;
			}
		}

		private class MeshHolder
		{
			public readonly int SpriteCount;
			public readonly Mesh Mesh;

			public readonly UnityEngine.Vector3[] Vertices;
			public readonly UnityEngine.Vector2[] UVs;
			public readonly Color32[] Colors;

			public MeshHolder(int spriteCount)
			{
				Mesh = new Mesh();
				//Mesh.MarkDynamic(); //Seems to be a win on wp8

				SpriteCount = NextPowerOf2(spriteCount);
				int vCount = SpriteCount * 4;

				Vertices = new UnityEngine.Vector3[vCount];
				UVs = new UnityEngine.Vector2[vCount];
				Colors = new Color32[vCount];

				//Put some random crap in this so we can just set the triangles once
				//if these are not populated then unity totally fucks up our mesh and never draws it
				for (var i = 0; i < vCount; i++)
				{
					Vertices[i] = new UnityEngine.Vector3(1, i);
					UVs[i] = new UnityEngine.Vector2(0, i);
					Colors[i] = new Color32(255, 255, 255, 255);
				}

				var triangles = new int[SpriteCount * 6];
				for (var i = 0; i < SpriteCount; i++)
				{
					/*
					 *  TL    TR
					 *   0----1 0,1,2,3 = index offsets for vertex indices
					 *   |   /| TL,TR,BL,BR are vertex references in SpriteBatchItem.
					 *   |  / |
					 *   | /  |
					 *   |/   |
					 *   2----3
					 *  BL    BR
					 */
					// Triangle 1
					triangles[i * 6 + 0] = i * 4;
					triangles[i * 6 + 1] = i * 4 + 1;
					triangles[i * 6 + 2] = i * 4 + 2;
					// Triangle 2
					triangles[i * 6 + 3] = i * 4 + 1;
					triangles[i * 6 + 4] = i * 4 + 3;
					triangles[i * 6 + 5] = i * 4 + 2;
				}

				Mesh.vertices = Vertices;
				Mesh.uv = UVs;
				Mesh.colors32 = Colors;
				Mesh.triangles = triangles;
			}

			public void Populate(VertexPositionColorTexture[] vertexData, int numVertices)
			{
				for (int i = 0; i < numVertices; i++)
				{
					var p = vertexData[i].Position;
					Vertices[i] = new UnityEngine.Vector3(p.X, p.Y, p.Z);

					var uv = vertexData[i].TextureCoordinate;
					UVs[i] = new UnityEngine.Vector2(uv.X, 1 - uv.Y);

					var c = vertexData[i].Color;
					Colors[i] = new Color32(c.R, c.G, c.B, c.A);
				}
				//we could clearly less if we remembered how many we used last time
				Array.Clear(Vertices, numVertices, Vertices.Length - numVertices);

				Mesh.vertices = Vertices;
				Mesh.uv = UVs;
				Mesh.colors32 = Colors;
			}

			public int NextPowerOf2(int minimum)
			{
				int result = 1;

				while (result < minimum)
					result *= 2;

				return result;
			}
		}


		private class MeshPool
		{
			private List<MeshHolder> _unusedMeshes = new List<MeshHolder>();
			private List<MeshHolder> _usedMeshes = new List<MeshHolder>();

			private List<MeshHolder> _otherMeshes = new List<MeshHolder>();
			//private int _index;

			/// <summary>
			/// get a mesh with at least this many triangles
			/// </summary>
			public MeshHolder Get(int spriteCount)
			{
				MeshHolder best = null;
				int bestIndex = -1;
				for (int i = 0; i < _unusedMeshes.Count; i++)
				{
					var unusedMesh = _unusedMeshes[i];
					if ((best == null || best.SpriteCount > unusedMesh.SpriteCount) && unusedMesh.SpriteCount >= spriteCount)
					{
						best = unusedMesh;
						bestIndex = i;
					}
				}
				if (best == null)
				{
					best = new MeshHolder(spriteCount);
				}
				else
				{
					_unusedMeshes.RemoveAt(bestIndex);
				}
				_usedMeshes.Add(best);

				return best;
			}

			public void Reset()
			{
				//Double Buffer our Meshes (Doesnt seem to be a win on wp8)
				//Ref http://forum.unity3d.com/threads/118723-Huge-performance-loss-in-Mesh-CreateVBO-for-dynamic-meshes-IOS
				
				//meshes from last frame are now unused
				_unusedMeshes.AddRange(_otherMeshes);
				_otherMeshes.Clear();
				
				//swap our use meshes and the now empty other meshes
				var temp = _otherMeshes;
				_otherMeshes = _usedMeshes;
				_usedMeshes = temp;
			}
		}
	}
}
