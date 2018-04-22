
//using UnityEngine;
//using System.Collections;

//using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using UnityEngine;

namespace vaporvent
{
	public class makeSteam : PartModule 
	{
        [KSPField(guiActiveEditor = true, isPersistant = true)]
        [UI_Toggle(enabledText = "Hide vapor", disabledText = "Show vapor")]
        public bool alwaysOn = false;



        List<KSPParticleEmitter> emitter;

        bool vaporVisible = false;

        [KSPEvent(guiActive = true, guiName = "Show Vapor", active = true)]
        public void ShowSteamEvent()
        {
            vaporVisible = true;
            SlowCheck();
            UpdateEvents(vaporVisible);
        }

        [KSPEvent(guiActive = true, guiName = "Hide Vapor", active = false)]
        public void HideSteamEvent()
        {
            vaporVisible = false;
            UpdateEvents(vaporVisible);
        }

        private void UpdateEvents(bool visible)
        {
            if (!alwaysOn)
            {
                Events["ShowSteamEvent"].active = !visible;
                Events["HideSteamEvent"].active = visible;
            } else
            {
                Events["ShowSteamEvent"].active = false;
                Events["HideSteamEvent"].active = false;
            }
        }

        [KSPAction("Toggle Vapor Vent")]
        public void ToggleSteamAction(KSPActionParam param)
        {
            vaporVisible = !vaporVisible;
            if (vaporVisible)
                SlowCheck();
            UpdateEvents(vaporVisible);
        }
        void LateUpdate()
        {
            if (HighLogic.LoadedScene == GameScenes.EDITOR)
            {
                emitter = part.FindModelComponents<KSPParticleEmitter>();
                foreach (KSPParticleEmitter unit in emitter)
                {
                    unit.emit = alwaysOn;
                }
                return;
            }
        }

        void PLSteam()
		{
        
            if (HighLogic.LoadedScene == GameScenes.FLIGHT)
            {
                //print("vaporvent: started makesteam, vessel.situation: " + vessel.situation.ToString());
                emitter = part.FindModelComponents<KSPParticleEmitter>();
                if (emitter != null)
                {
                    foreach (KSPParticleEmitter unit in emitter)
                    {
                        if ((vaporVisible || alwaysOn) && vessel.speed < 10) //|| Vessel.Situations.PRELAUNCH == vessel.situation || Vessel.Situations.LANDED == vessel.situation) 
                        {
                            unit.emit = true;
                            //print ("vaporvent: true");
                        }
                        else
                        {
                            unit.emit = false;
                            print("vaporvent: false");
                            vaporVisible = false;
                            UpdateEvents(vaporVisible);
                            CancelInvoke();
                        }
                    }
                }
            }
		}
		void SlowCheck()
		{
            // print("vaporvent: SlowCheck");
			InvokeRepeating("PLSteam", 0, 5F);
		}
        void onLaunch(EventReport evt)
        {
            Debug.Log("VaporVent.onLaunch");
           // if (evt.origin.vessel == this.vessel)
            {
                vaporVisible = false;
                UpdateEvents(vaporVisible);
            }
        }

        public override void OnStart(StartState state)
        {
            // print("vaporvent: OnStart");
            
            emitter = part.FindModelComponents<KSPParticleEmitter>();
            
            if (emitter != null)
                foreach (KSPParticleEmitter unit in emitter)
                    EffectBehaviour.AddParticleEmitter(unit);
           
            base.OnStart(state);
            if (HighLogic.LoadedSceneIsFlight)
            {
                GameEvents.onLaunch.Add(onLaunch);
                if (Vessel.Situations.PRELAUNCH == vessel.situation || Vessel.Situations.LANDED == vessel.situation)
                {
                    vaporVisible = true;
                    UpdateEvents(vaporVisible);
                    SlowCheck();
                }
            }
        }
        public void OnDestroy()
        {
            GameEvents.onLaunch.Remove(onLaunch);
            emitter = part.FindModelComponents<KSPParticleEmitter>();
            if (emitter != null)
                foreach (KSPParticleEmitter unit in emitter)
                    EffectBehaviour.RemoveParticleEmitter(unit);
        }
    }
}