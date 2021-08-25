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

        public static Vector3 velocity = Vector3.zero;

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
            Vector3 origin = __instance.transform.forward;
            Vector3 target = MainCameraControl.main.transform.forward;
            

            if (CrouchBehavior.main.pitchToggled)
            {
                
                __instance.transform.forward = Vector3.SmoothDamp(origin, target, ref velocity, 0.5f);
                Rigidbody proper = __instance.rigidBody;
                proper.position = __instance.transform.position;
                proper.rotation = __instance.transform.rotation;                

            }           

        }
    }
}
