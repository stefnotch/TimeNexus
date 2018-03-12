using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Debugging
{
	public static class DebugDraw
	{
		private static Entity e;
		public static void Line(ScriptComponent parent, Vector3 from, Vector3 to)
		{
			var vertexCount = 2;
			// VertexPositionNormalTexture is the layout that the engine uses in the shaders
			var vBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(parent.GraphicsDevice, new VertexPositionNormalTexture[] {
		 new VertexPositionNormalTexture(from, new Vector3(0, 1, 1), new Vector2(0, 0)),
		 new VertexPositionNormalTexture(to, new Vector3(0, 1, 1), new Vector2(0, 0))
	 });

			var meshDraw = new MeshDraw
			{
				PrimitiveType = PrimitiveType.LineStrip, // Tell the gpu that this is a line
				VertexBuffers = new[] { new VertexBufferBinding(vBuffer, VertexPositionNormalTexture.Layout, vertexCount) },
				DrawCount = vertexCount
			};

			var mesh = new Mesh
			{
				Draw = meshDraw
			};

			var model = new Model
			{
				mesh
			};

			parent.Entity.Scene.Entities.Remove(e);

			e = new Entity
			{
				new ModelComponent(model)
			};


			parent.Entity.Scene.Entities.Add(e);
		}
	}
}
