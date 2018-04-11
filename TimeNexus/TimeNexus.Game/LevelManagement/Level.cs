﻿using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.LevelManagement
{
	public class Level
	{
		private BoundingBox _boundingBox = BoundingBox.Empty;
		internal readonly List<Gateway> _gateways = new List<Gateway>();

		/// <summary>
		/// The scene
		/// </summary>
		public Scene Scene { get; private set; }

		/// <summary>
		/// The bounding box of the scene
		/// </summary>
		public BoundingBox BoundingBox
		{
			get => _boundingBox;
		}

		/// <summary>
		/// Used to prevent colliding bounding boxes from colliding with each other
		/// </summary>
		public int Dimension { get; set; }

		public LevelSettings Settings { get; }

		/// <summary>
		/// A <list type="Gateway">list of level entrances/exits</list>
		/// Every entrance is also an exit :D
		/// </summary>
		public IReadOnlyList<Gateway> Gateways { get => _gateways.AsReadOnly(); }


		public Level(Scene scene)
		{
			Scene = scene;
			ComputeBoundingBox(Scene);
			_gateways.AddRange(GetRootComponents<Gateway>(Scene));
			Settings = GetRootComponents<LevelSettings>(Scene).DefaultIfEmpty(new LevelSettings()).FirstOrDefault();
		}

		public void Rotate(Quaternion rotation) {

			var rotMatrix = Matrix.RotationQuaternion(rotation);

			foreach (Entity e in this.Scene.Entities)
			{
				e.Transform.LocalMatrix = rotMatrix * e.Transform.LocalMatrix;
				e.Transform.LocalMatrix.Decompose(out _, out Quaternion outRotation, out Vector3 position);
				e.Transform.Rotation = outRotation;
				e.Transform.Position = position;
			}
			ComputeBoundingBox(Scene);
		}

		public void Translate(Vector3 vector)
		{

			foreach (Entity e in this.Scene.Entities)
			{
				e.Transform.Position += vector;
			}
			ComputeBoundingBox(Scene);
		}

		private IEnumerable<T> GetRootComponents<T>(Scene s) where T:EntityComponent
		{
			foreach (Entity e in s.Entities)
			{
				var component = e.Get<T>();
				if (component != null) yield return component;
			}

		}

		/// <summary>
		/// Computes the bounding box of the scene
		/// </summary>
		/// <param name="scene"></param>
		private void ComputeBoundingBox(Scene scene)
		{
			foreach (Entity e in scene.Entities)
			{
				//TODO: Entities also have a PhysicsComponent, which also has something like a bounding box
				foreach (var model in e.GetAll<ModelComponent>())
				{
					BoundingBox.Merge(ref model.BoundingBox, ref _boundingBox, out _boundingBox);
				}
			}
		}
	}
}
