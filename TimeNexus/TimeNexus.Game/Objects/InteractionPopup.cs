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
		
		private bool _open = false;


		[DefaultValue(5f)]
		public float InteractionDistance { get; set; } = 5f;
		public UIPage InteractionUI { get; set; }
		public Vector3 Offset { get; set; }

		public bool Open {
			get => _open;
			set
			{
				_open = value;
				_onOpen.OnNext(value);
			}
		}

		[DataMemberIgnore]
		private readonly Subject<bool> _onOpen = new Subject<bool>();
		/// <summary>
		/// Gets called when the door gets opened or closed
		/// </summary>
		[DataMemberIgnore]
		public IObservable<bool> OnOpen { get => _onOpen; }
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

			this.Entity.AddChild(_UIEntity);


			//Console.WriteLine(Content.IsLoaded("MainScene"));
			//var s = SiliconStudio.Assets.AssetCloner.Clone();
			/*
			var x = Content.Load<Scene>("MainScene");


			var no = Content.GetType().GetTypeInfo().GetDeclaredMethods("DeserializeObject").Where(s => s.GetParameters().First().ParameterType == typeof(string)).First();
			var test = no
				//string initialUrl, string newUrl, Type type, object obj, ContentManagerLoaderSettings settings 
				.Invoke(Content, new object[] { "MainScene", "MainScene", typeof(Scene), null, SiliconStudio.Core.Serialization.Contents.ContentManagerLoaderSettings.Default });

			Scene testScene = (Scene)test;
			testScene.Offset.X = 10;*/




			//Content.Save("Nope", this.Entity.Scene);
			//Scene testScene = Content.Load<Scene>("Nope");
			//testScene.Offset.X = -10;

			//this.Entity.Scene.Children.Add(testScene);

			//Prefab xx = new Prefab();
			//SiliconStudio.Xenko.Engine.Design.EntityCloner.
			//MemoryFileProvider
			//x.

			//Content.Reload(null, "MainScene");
			//Content.Unload()
			//var s = Content.Load<Scene>("MainScene");
			//s.Entities.Remove(s.Entities.First());


			//Content.Load<Scene>("MainScene");

			//Kinda works:
			/*
			Scene customScene = new Scene();
			foreach(Entity e in Entity.Scene.Entities)
			{
				//e.Scene = null;
				Entity.Scene.Entities.Remove(e);
				customScene.Entities.Add(e);
			}

			
			//this.Entity.Scene.Entities.Clear();
			//customScene.Entities.AddRange(entities);
			Content.Unload("MainScene");

			Scene newScene = Content.Load<Scene>("MainScene");
			*/
			Vector3 test = this.Entity.Transform.GetWorldPosition();
			/*this.Entity.Scene.Offset.X += 10;
			this.Entity.Scene.Offset.Z -= 0.6f;
			*/

			

			this.Entity.GetOrCreate<LookatTrigger>()
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

		private void HandleInput()
		{
			if(Input.IsKeyReleased(KeyBindings.Interact))
			{
				Open = !Open;
			}
		}

		public override void Cancel()
		{
			_onOpen.Dispose();
		}
	}
}
