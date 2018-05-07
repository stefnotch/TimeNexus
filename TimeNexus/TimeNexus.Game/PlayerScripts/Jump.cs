using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Extensions;
using SiliconStudio.Xenko.Physics;

namespace TimeNexus.PlayerScripts
{
	[Obsolete]
	public class JumpScript : SyncScript
	{        
		public Keys jump_key;
		public float jump_speed;
		
		private CharacterComponent character;
		private bool grounded;

		public override void Start()
		{
			this.grounded = true;
			this.character = this.Entity.GetParent().Get<CharacterComponent>();
			this.character.JumpSpeed = this.jump_speed;
		}

		public override void Update()
		{
			this.Jump();
			this.Land();
		}
		
		private void Jump()
		{
			//bool keydown = Input.IsKeyDown(this.jump_key);
			//if (keydown) Console.Write("Jump-Key pressed.");
			if (!this.grounded || !Input.IsKeyDown(this.jump_key)) return;

			this.grounded = false;
			this.character.Jump(Vector3.UnitY * this.jump_speed);
		}

		private void Land()
		{
			if (this.grounded) return;

			foreach (Collision c in this.character.Collisions)if(c.ColliderB.CollisionGroup == CollisionFilterGroups.StaticFilter) this.grounded = true;
		}
	}
}
