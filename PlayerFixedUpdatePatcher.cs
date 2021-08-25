using UnityEngine;
using HarmonyLib;
using SMLHelper.V2.Options;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UWE;

namespace Crouch
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("UpdateRotation")]
    public class PlayerRotatePatcher
    {

        public static float velocity = 0f;

        [HarmonyPrefix]

        public static bool OverrideRotation(Player __instance)
        {
            if (CrouchBehavior.main.pitchToggled)

            {
                return false;

            }

            return true;
        }

        [HarmonyPostfix]
        public static void AlignBody(Player __instance)
        {
            float origin = __instance.transform.eulerAngles.y;
            Transform target = MainCameraControl.main.transform;            
            float smooth = 0.3f;
            
            

            if (CrouchBehavior.main.pitchToggled)
            {
                
                float yAngle = Mathf.SmoothDampAngle(origin, target.eulerAngles.y, ref velocity, 1f, smooth);

                Vector3 targetpos = target.position;
                targetpos += Quaternion.Euler(0, yAngle, 0) * new Vector3(0, 0, -1);

                __instance.transform.position = targetpos;                                              

            }           
            
        }
    }
}
