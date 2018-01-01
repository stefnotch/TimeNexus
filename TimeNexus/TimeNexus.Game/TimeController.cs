using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;

namespace TimeNexus
{
	public class TimeController : SyncScript
	{
		// Declared public member fields and properties will show in the game studio
		private int _m = 1;
		private static List<ModelComponent> models = new List<ModelComponent>();
		public int ShowModel
		{
			get
			{
				if (Entity?.Scene?.Entities != null)
				{
					foreach (Entity e in this.Entity.Scene.Entities)
					{
						if (_m == 1)
						{
							e.Enable<ModelComponent>(true);
						}
						else
						{
							e.Enable<ModelComponent>(false);
						}
					}
				}


				

				/*if (_m == 1)
				{
					models.AddRange(this.Entity.GetAll<ModelComponent>().ToList());

					models.ForEach((model) =>
					{
						model.Enabled = true;
					});
				}
				else if (models != null)
				{
					models.ForEach((model) =>
					{
						model.Enabled = false;
					});

				}*/
				return _m;
			}
			set
			{
				_m = value;
			}
		}


		public override void Start()
		{
			// Initialization of the script.
		}

		public override void Update()
		{
			// Do stuff every new frame
		}
	}
}
