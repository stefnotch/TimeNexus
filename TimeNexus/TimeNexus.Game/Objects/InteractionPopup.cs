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
using SiliconStudio.Xenko.Input;
using TimeNexus.Input;
using SiliconStudio.Assets;
using System.Reflection;
using SiliconStudio.Core;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace TimeNexus.Objects
{
	public class InteractionPopup : StartupScript
	{
		[DataMemberIgnore]
		private Entity _UIEntity;

		[DefaultValue(5f)]
		public float InteractionDistance { get; set; } = 5f;
		public UIPage InteractionUI { get; set; }
		public Vector3 Offset { get; set; }

		[DataMemberIgnore]
		private readonly Subject<bool> _onInteraction = new Subject<bool>();

		[DataMemberIgnore]
		public IObservable<bool> OnInteraction { get => _onInteraction; }
		public override void Start()
		{
			if (InteractionUI == null) this.Log.Error("No interaction UI attached to this entity");
			_UIEntity = new Entity();

			var UIComponent = new UIComponent()
			{
				Page = InteractionUI ?? new UIPage(),
				Resolution = new Vector3(100, 100, 1000),
				IsBillboard = true,
				IsFullScreen = false,
				Enabled = false,
				SnapText = true
			};

			_UIEntity.Add(UIComponent);
			_UIEntity.Transform.Position = Offset;

			Entity.AddChild(_UIEntity);

			Entity.GetOrCreate<LookatTrigger>()
				.OnLookatStay
				.Subscribe(hitResult =>
				{
					var dist = (hitResult.Point - PlayerController.Player.Transform.GetWorldPosition()).Length();
					if (dist < InteractionDistance)
					{
						DisplayUI(true);
						HandleInput();
					}
					else
					{
						DisplayUI(false);
					}
				});

			Entity.GetOrCreate<LookatTrigger>()
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

		private void HandleInput()
		{
			if(Input.IsKeyReleased(KeyBindings.Interact))
			{
				_onInteraction.OnNext(true);
			}
		}
	}
}
