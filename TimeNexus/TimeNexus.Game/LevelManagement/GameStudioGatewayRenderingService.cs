using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Extensions;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Graphics.GeometricPrimitives;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Materials;
using SiliconStudio.Xenko.Rendering.Materials.ComputeColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.LevelManagement
{
	public class GameStudioGatewayRenderingService : GameSystem
	{
		private GraphicsDevice graphicsDevice;
		private MaterialInstance arrowMaterial;
		private Mesh arrowMesh;

		public GameStudioGatewayRenderingService(IServiceRegistry registry) : base(registry)
		{
		}

		public override void Initialize()
		{
			graphicsDevice = Services.GetSafeServiceAs<IGraphicsDeviceService>().GraphicsDevice;
			arrowMaterial = Material.New(graphicsDevice, new MaterialDescriptor()
			{
				Attributes =
				{
					Diffuse = new MaterialDiffuseMapFeature(new ComputeColor(new Color4(1f, 1f, 1f, 1))),
					DiffuseModel = new MaterialDiffuseLambertModelFeature(),
					Emissive = new MaterialEmissiveMapFeature(new ComputeColor(new Color4(1f, 1f, 1f, 1)))
				}
			});

			var arrow = GeometricPrimitive.Cone.New(radius: 0.1f, height: 0.3f);
			for(int i = 0; i < arrow.Vertices.Length; i++)
			{
				arrow.Vertices[i].Position.Y -= 0.15f;
			}

			arrowMesh = new Mesh()
			{
				Draw = new GeometricPrimitive(graphicsDevice, arrow).ToMeshDraw()
			};
		}

		public Model CreateArrow()
		{
			var arrowModel = new Model();
			arrowModel.Meshes.Add(arrowMesh);
			arrowModel.Materials.Add(arrowMaterial);
			/*		new MaterialDescriptor()
					{
						Attributes =
				{
				Diffuse = new MaterialDiffuseMapFeature()
				}
					};*/
			//material.Parameters.Set(MaterialKeys.DiffuseValue, new ComputeTextureColor(new Texture()))
			return arrowModel;
		}
	}
}
