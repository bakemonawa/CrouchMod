using System;
using System.Collections;
namespace Crouch
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using UnityEngine;
    using HarmonyLib;
    using SMLHelper.V2.Options;
    using SMLHelper.V2.Handlers;
    using SMLHelper.V2.Utility;
    using UWE;
    using System.Net.NetworkInformation;

    public class CrouchBehavior : MonoBehaviour
    {
        public float kickDistance = 0.9f;
        public float crouchHeight = 0.9f;
        public float hitforce;
        public float kickforce;
        public Vector3 crouchDir = Player.main.transform.up * -1;                
        private float dist;        

        public bool crouchPressed = Input.GetKeyDown(CrouchPatcher.Config.CrouchKey);                
        public bool kickOffPressed = Input.GetKeyDown(CrouchPatcher.Config.KickOffKey);
        public bool kickAllowed;
        public bool kickOff;
        public bool isCrouching;
        public bool isCrouchingUnderwater;                
        public bool pitchToggled;
        

        public global::Utils.MonitoredValue<bool> isUnderwaterForSwimming = new global::Utils.MonitoredValue<bool>();
        public global::Utils.MonitoredValue<bool> isUnderwater = new global::Utils.MonitoredValue<bool>();

        public RaycastHit hit;
        public VFXSurface hitsurface;
        private VFXSurfaceTypes SurfaceType;
        public FootstepSounds footStepSounds;
        public FMODAsset jumpSound;

        private LiveMixin liveMixinTarget;
        
        public Rigidbody useRigidbody;           

        public Player Ryley = Player.main;
        public static CrouchBehavior main;
        public Vector3 forceDir = Player.main.transform.up;

        private void Awake()

        {
            main = this;

        }
              

        public void UpdateTarget()

        {
                       

            Physics.Raycast(Player.main.transform.position, crouchDir, out hit, kickDistance);
            hitsurface = hit.transform.gameObject.GetComponent<VFXSurface>();
            liveMixinTarget = hit.transform.gameObject.GetComponentInParent<LiveMixin>();
            SurfaceType = ((hitsurface != null) ? hitsurface.surfaceType : VFXSurfaceTypes.none);
                     
            
            dist = Vector3.Distance(hit.transform.position, Player.main.transform.position);

            if (dist <= kickDistance)

            {
                kickAllowed = true;
            }

            else
            {
                kickAllowed = false;
            }

        }    
               


      
        public void OnKickOff()
        {
            // If there is a collider at the position where the doubled-over player model's
            // feet would be once they're outstretched (On Jump key press), apply force in the direction of the cursor.
            // The closer the player is to the collider, the more force will be applied.

            

            if (dist != 0)
            {

                float hitforce = 22 / dist;

                float kickforce = 20 / dist;

            }

            if (liveMixinTarget != null)
            {
                liveMixinTarget.TakeDamage(22 * hitforce, hit.transform.position, DamageType.Collide, null);
            }

            if (Ryley.IsUnderwaterForSwimming())
            {
                Transform transform = MainCamera.camera.transform;
                if (SurfaceType == VFXSurfaceTypes.metal)
                {
                    global::Utils.PlayFMODAsset(Ryley.footStepSounds.metalSound, transform, 1f);
                }
                else
                {
                    global::Utils.PlayFMODAsset(Ryley.footStepSounds.landSound, transform, 1f);
                }

                Ryley.rigidBody.AddForce(forceDir * kickforce, ForceMode.VelocityChange);


            }

            global::Utils.PlayFMODAsset(Ryley.jumpSound, MainCamera.camera.transform, 0f);
            

        }

        public void Test()
        {
            
                        
                Ryley.rigidBody.AddForce(forceDir * 20f, ForceMode.VelocityChange);
                global::Utils.PlayFMODAsset(Ryley.jumpSound, MainCamera.camera.transform, 0f);
                global::Utils.PlayFMODAsset(Ryley.footStepSounds.landSound, hit.transform.position, 1f);
            
        }

        


    }

}
