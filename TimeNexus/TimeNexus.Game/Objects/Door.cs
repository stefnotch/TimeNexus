using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Objects
{
	public class Door : StartupScript
	{
		private LookatTrigger _lookatTrigger;

		public UIPage DoorUI { get; set; }
		public Vector3 Offset { get; set; }


		private readonly float MinDistance = 5f;

		public override void Start()
		{

			var UIEntity = new Entity();

			if (DoorUI != null)
			{

				var DoorUIComponent = new UIComponent()
				{
					Page = DoorUI,
					Resolution = new Vector3(100, 100, 1000),
					IsBillboard = true,
					IsFullScreen = false,
					Enabled = false
				};

				UIEntity.Add(DoorUIComponent);
				UIEntity.Transform.Position = Offset;

				this.Entity.AddChild(UIEntity);
			}
			
			_lookatTrigger = this.Entity.GetOrCreate<LookatTrigger>();
			_lookatTrigger.InteractionRadius = 4.0f;
			_lookatTrigger.OnTrigger += (door, player) =>
			{
				if (DoorUI != null)
				{
					UIEntity.Enable<UIComponent>(true);
				}
			};

			_lookatTrigger.OnTriggerEnd += (door, player) =>
			{
				if (DoorUI != null)
				{
					UIEntity.Enable<UIComponent>(false);
				}
			};
		}
	}
}
