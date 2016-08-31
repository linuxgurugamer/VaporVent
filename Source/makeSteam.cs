
//using UnityEngine;
//using System.Collections;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


namespace vaporvent
{
	public class makeSteam : PartModule 
	{
		KSPParticleEmitter[] emitter;

		void PLSteam()
		{
			// print ("vaporvent: started makesteam");
			emitter = part.FindModelComponents<KSPParticleEmitter>();
			if (emitter != null)
			{
				foreach(KSPParticleEmitter unit in emitter)
				{
					if (Vessel.Situations.PRELAUNCH == vessel.situation) 
					{
						unit.emit = true;
						// print ("vaporvent: true");
					} 
					else 
					{
						unit.emit = false;
						// print ("vaporvent: false");
					}
				}
			}
		}
		void SlowCheck()
		{
            // print("vaporvent: SlowCheck");
			InvokeRepeating("PLSteam", 0, 5F);
		}

        public override void OnStart(StartState state)
        {
            // print("vaporvent: OnStart");
            base.OnStart(state);
            SlowCheck();
        }
    }
}