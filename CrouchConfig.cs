using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWE;
using UnityEngine;
using HarmonyLib;
using System.Net.NetworkInformation;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Json;
using QModManager.API.ModLoading;


namespace Crouch
{
    
        public static class Logger
        {
            public static void Log(string message)
            {
                UnityEngine.Debug.Log("[Crouch] " + message);
            }
        }

        [Menu("CrouchMod Options")]
        public class CrouchConfig : ConfigFile
        {
            [Toggle("Enable Hinting")]
            public bool isHintingEnabled = true;

            [Keybind("Crouch Key")]
            public KeyCode CrouchKey = KeyCode.LeftControl;

            [Keybind("Kick Off Key")]
            public KeyCode KickOffKey = KeyCode.N;

            [Keybind("Pitch Control Key")]
            public KeyCode PitchControlKey = KeyCode.A;

        [Keybind("Test Key")]
            public KeyCode TestKey = KeyCode.B;
    }
}

