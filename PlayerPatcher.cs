using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crouch
{
    using UnityEngine;
    using HarmonyLib;
    using SMLHelper.V2.Options;
    using SMLHelper.V2.Handlers;
    using SMLHelper.V2.Utility;
    using UWE;



    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Awake")]

    public class PlayerAwakePatch
    {
        [HarmonyPostfix]

        public static void AddCrouchBehavior(Player __instance)

        {
            __instance.gameObject.AddComponent<CrouchBehavior>();

        }


    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]

    public class PlayerUpdatePatch
    {


        [HarmonyPrefix]


        public static void CrouchCheck(Player __instance)

        {
            CrouchBehavior.main.UpdateTarget();


            if (Input.GetKeyDown(CrouchPatcher.Config.CrouchKey))

            {
                // Try to somehow make the animation pause at the peak of the jump                              
                //decrease height
                //ensure that jump action drives player model & camera
                //downwards to make it really feel like the player is crouching

                //decrease traversal speed

                //hijack jump key, set it to become kick-off/leave crouch state button when player is in crouch state.                


                if (__instance.IsUnderwater() || __instance.IsUnderwaterForSwimming())

                {
                    CrouchBehavior.main.isCrouchingUnderwater = !CrouchBehavior.main.isCrouchingUnderwater;

                    if (CrouchBehavior.main.isCrouchingUnderwater && CrouchBehavior.main.kickOffPressed && CrouchBehavior.main.kickAllowed)

                    {
                        CrouchBehavior.main.OnKickOff();
                        CrouchBehavior.main.kickOff = true;
                        CrouchBehavior.main.isCrouchingUnderwater = false;

                        float timekickstop = 0f;


                        if (timekickstop == Time.time + 4f)

                        {

                            CrouchBehavior.main.kickOff = false;
                        }
                    }

                    // If player kicks off when not allowed, leave crouch state

                    else if (CrouchBehavior.main.isCrouching && CrouchBehavior.main.kickOffPressed && !CrouchBehavior.main.kickAllowed)

                    {
                        CrouchBehavior.main.isCrouchingUnderwater = false;
                    }


                }


                else

                {

                    CrouchBehavior.main.isCrouching = !CrouchBehavior.main.isCrouching;
                    CrouchBehavior.main.isCrouchingUnderwater = false;

                }

            }

            else if (Input.GetKeyDown(CrouchPatcher.Config.TestKey))

            {

                CrouchBehavior.main.Test();
            }

        }

        [HarmonyPostfix]

        public static void PitchCheck(Player __instance)
        {
            if (Input.GetKeyDown(CrouchPatcher.Config.PitchControlKey))

            {
                CrouchBehavior.main.pitchToggled = !CrouchBehavior.main.pitchToggled;
            }

            if (CrouchBehavior.main.pitchToggled)

            {
                MainCameraControl.main.minimumY = -360f;
                MainCameraControl.main.maximumY = 360f;


            }

            else if (!CrouchBehavior.main.pitchToggled)

            {
                MainCameraControl.main.minimumY = -80f;
                MainCameraControl.main.maximumY = 80f;
            }

        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("FixedUpdate")]

    public class PlayerFixedUpdatePatch
    {


        [HarmonyPostfix]
        public static void UpdatePitch (Player __instance)
        {

            if (CrouchBehavior.main.pitchToggled)
            {
                Rigidbody proper = __instance.rigidBody;
                proper.position = __instance.transform.position;
            }
        
         }
    }

    [HarmonyPatch(typeof(ArmsController))]
    [HarmonyPatch("Update")]

    public class UpdateDivingPatcher
    {
        

        [HarmonyPostfix]
        public static void Postfix(ArmsController __instance)


        {
            Animation anim = new Animation();

            if (CrouchBehavior.main.isCrouching || CrouchBehavior.main.isCrouchingUnderwater)

            {
                SafeAnimator.SetBool(__instance.animator, "is_underwater", false);
                SafeAnimator.SetBool(__instance.animator, "jump", true);
                __instance.StartSetAnimParam("jump", 999f);

            }

            else if (!CrouchBehavior.main.isCrouching && !CrouchBehavior.main.isCrouchingUnderwater)

            {
                SafeAnimator.SetBool(__instance.animator, "jump", false);
                if (Player.main.IsUnderwater())

                {
                    SafeAnimator.SetBool(__instance.animator, "is_underwater", true);
                }

            }

            else if (CrouchBehavior.main.kickOff || Input.GetKeyDown(CrouchPatcher.Config.TestKey))

            {
                SafeAnimator.SetBool(__instance.animator, "jump", false);
                SafeAnimator.SetBool(__instance.animator, "is_underwater", false);
                SafeAnimator.SetBool(__instance.animator, "diving", true);
                SafeAnimator.SetBool(__instance.animator, "diving_land", true);

                __instance.StartSetAnimParam("diving_land", 4f);
                anim["diving_land"].speed = 8f;

            }

            else if (!CrouchBehavior.main.kickOff || Input.GetKeyUp(CrouchPatcher.Config.TestKey))

            {

                SafeAnimator.SetBool(__instance.animator, "diving", false);
                SafeAnimator.SetBool(__instance.animator, "diving_land", false);

                if (Player.main.IsUnderwater())

                {
                    SafeAnimator.SetBool(__instance.animator, "is_underwater", true);
                }
                


            }

        }
            
    }


    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("UpdateController")]

    public class UpdateHeight
    {
        [HarmonyPostfix]
        public static void Postfix(PlayerController __instance)


        {
                      

            if (CrouchBehavior.main.isCrouching)

            {

                __instance.desiredControllerHeight = CrouchBehavior.main.crouchHeight;

            }

            else if (!CrouchBehavior.main.isCrouching)

            {

                __instance.desiredControllerHeight = 1.6f;

                if (Player.main.IsUnderwater())
                {
                    __instance.desiredControllerHeight = 1f;
                }

            }

          

        }
    }

    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("SetMotorMode")]

    public class UpdateMotorMode
    {
        [HarmonyPostfix]

        public static void Postfix(PlayerController __instance)

        {

            if (CrouchBehavior.main.isCrouching)
            {
                __instance.groundController.forwardMaxSpeed = 2.5f;
                __instance.groundController.backwardMaxSpeed = 1.5f;
                __instance.groundController.strafeMaxSpeed = 2f;

            }

            else if (CrouchBehavior.main.isCrouchingUnderwater)
            {
                __instance.underWaterController.swimDrag = 1.2f;
                __instance.underWaterController.forwardMaxSpeed = 0.5f;
                __instance.underWaterController.backwardMaxSpeed = 0.2f;
                __instance.underWaterController.strafeMaxSpeed = 0.3f;


            }
        }

    }


}
