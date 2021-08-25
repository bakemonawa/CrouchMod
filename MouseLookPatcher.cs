using UnityEngine;
using HarmonyLib;
using SMLHelper.V2.Options;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UWE;

namespace Crouch
{
	[HarmonyPatch(typeof(MouseLook))]
	[HarmonyPatch("Update")]
	public class MouseLookPatcher
	{

		[HarmonyPostfix]

		public static void Postfix(MouseLook __instance)
		{
			
			if (CrouchBehavior.main.pitchToggled)
            {
				__instance.maximumY = 360f;
				__instance.minimumY = -360f;
			}
			
		}
	}
}
