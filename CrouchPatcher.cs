using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMLHelper.V2.Options;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UWE;
using UnityEngine;
using HarmonyLib;
using System.Net.NetworkInformation;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Json;
using QModManager.API.ModLoading;

namespace Crouch
{    
    [QModCore]
    public static class CrouchPatcher
    {
        internal static CrouchConfig Config { get; private set; }
        //public static Options Options = new Options();

        [QModPatch]
        public static void Patch()
        {           
            Config = OptionsPanelHandler.Main.RegisterModOptions<CrouchConfig>();
            //OptionsPanelHandler.RegisterModOptions(Options);
            var harmony = new Harmony("com.falselight.crouchmod");
            harmony.PatchAll();
        }

    }
}
