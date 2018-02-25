using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering.Materials;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Materials.ComputeColors;
using SiliconStudio.Xenko.Graphics.GeometricPrimitives;
using SiliconStudio.Xenko.Extensions;

namespace TimeNexus
{
	public class ShaderLoader : StartupScript
	{
		public Texture Texture;
		public override void Start()
		{
			Texture = Content.Load<Texture>("Textures/circle02");
			var material = Material.New(GraphicsDevice, new MaterialDescriptor
			{
				Attributes = new MaterialAttributes
				{
					DiffuseModel = new MaterialDiffuseLambertModelFeature(),
					Diffuse = new MaterialDiffuseMapFeature
					{
						DiffuseMap = new ComputeShaderClassColor { MixinReference = "TimeChangeFuture" },
					},
				},
			});

			material.Passes[0].Parameters.Set(Effects.TimeChangeFutureKeys.MyTexture, Texture);

			var entity = new Entity(new Vector3(0.5f, 1.5f, 0.5f));
			entity.GetOrCreate<ModelComponent>().Model = new Model
			{
					new Mesh
					{
						Draw = GeometricPrimitive.Sphere.New(GraphicsDevice).ToMeshDraw(),
					},
					material
			};

			Entity.Scene.Entities.Add(entity);
		}
	}
}
