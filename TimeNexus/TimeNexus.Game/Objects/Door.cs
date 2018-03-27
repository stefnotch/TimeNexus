using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.ExtensionMethods;
using TimeNexus.Player;
using System.Reactive.Linq;

namespace TimeNexus.Objects
{
	public class Door : StartupScript
	{
		public UIPage DoorUI { get; set; }
		public Vector3 Offset { get; set; }

		private Entity _UIEntity;

		private readonly float MinDistance = 5f;

		public override void Start()
		{
			if (DoorUI == null) this.Log.Error("No door UI attached to this entity");
			_UIEntity = new Entity();

			var DoorUIComponent = new UIComponent()
			{
				Page = DoorUI ?? new UIPage(),
				Resolution = new Vector3(100, 100, 1000),
				IsBillboard = true,
				IsFullScreen = false,
				Enabled = false,
				SnapText = true
			};

			_UIEntity.Add(DoorUIComponent);
			_UIEntity.Transform.Position = Offset;

			this.Entity.AddChild(_UIEntity);


			this.Entity.GetOrCreate<LookatTrigger>()
				.OnLookatStay
				.Subscribe(hitResult =>
				{
					var dist = (hitResult.Point - PlayerController.Player.Transform.GetWorldPosition()).Length();
					if (dist < MinDistance)
					{
						DisplayUI(true);
					}
					else
					{
						DisplayUI(false);
					}
				});

			this.Entity.GetOrCreate<LookatTrigger>()
				.OnLookatEnd
				.Subscribe(_ => DisplayUI(false));
		}

		private bool _UIVisible;
		public void DisplayUI(bool visible)
		{
			if (_UIVisible == visible) return;

			_UIVisible = visible;
			_UIEntity.Enable<UIComponent>(_UIVisible);
		}
	}
}
