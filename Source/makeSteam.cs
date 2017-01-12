
//using UnityEngine;
//using System.Collections;

//using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;


namespace vaporvent
{
	public class makeSteam : PartModule 
	{
		List<KSPParticleEmitter> emitter;

		void PLSteam()
		{
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                return;
            print ("vaporvent: started makesteam, vessel.situation: " + vessel.situation.ToString());
            emitter = part.FindModelComponents<KSPParticleEmitter>();
			if (emitter != null)
			{
				foreach(KSPParticleEmitter unit in emitter)
				{
					if (Vessel.Situations.PRELAUNCH == vessel.situation || Vessel.Situations.LANDED == vessel.situation) 
					{
						unit.emit = true;
						print ("vaporvent: true");
					} 
					else 
					{
						unit.emit = false;
						print ("vaporvent: false");
                        CancelInvoke();
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
            emitter = part.FindModelComponents<KSPParticleEmitter>();
            if (emitter != null)
                foreach (KSPParticleEmitter unit in emitter)
                    EffectBehaviour.AddParticleEmitter(unit);

            base.OnStart(state);
            SlowCheck();
        }
        public void OnDestroy()
        {
            emitter = part.FindModelComponents<KSPParticleEmitter>();
            if (emitter != null)
                foreach (KSPParticleEmitter unit in emitter)
                    EffectBehaviour.RemoveParticleEmitter(unit);
        }
    }
}