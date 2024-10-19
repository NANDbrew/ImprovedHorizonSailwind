using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace ImprovedHorizonSailwind
{
    internal class Patches
    {
        [HarmonyPatch(typeof(IslandHorizon))]
        private static class IslandHorizonPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("LateUpdate")]
            public static void LateUpdate_Patch(float __sunkHeight, float __fullySunkDistance, float __fullyEmergedDistance, Transform __player)
            {
                //finding the player's horizon
                const float worldRadius = 3000;
                float playerHeight = __player.transform.position.y / 1000;
                float horizonVal = 2 * playerHeight * worldRadius;
                float horizonDistance = Mathf.Sqrt(2 * playerHeight * worldRadius);



                //Finding the visual distance of the islands
                float islandVisualHorizon = Mathf.Sqrt(2 * -__sunkHeight * worldRadius);
                __fullySunkDistance = islandVisualHorizon + horizonDistance;
                __fullyEmergedDistance = horizonDistance;
            }
        }

    }

    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    //[BepinProcess("Sailwind.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public const string pluginGuid = "com.TheOriginOfAllEvil.ImprovedHorizon";
        public const string pluginName = "Improved Horizons";
        public const string pluginVersion = "0.1.0";
        internal static new ManualLogSource Logger;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            //Initialising a harmony instance
            Harmony harmony = new Harmony(pluginGuid);
            MethodInfo original = AccessTools.Method(typeof(IslandHorizon), "LateUpdate");
            MethodInfo patch = AccessTools.Method(typeof(Patches), "LateUpdate_Patch");
            harmony.Patch(original, new HarmonyMethod(patch));
        }
    }
}

