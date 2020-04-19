using RoR2;
using BepInEx;
using MonoMod.Cil;
using UnityEngine;
using System;

namespace ThinkInvisible.GlassArtifactOHP {
    
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class GlassArtifactOHPPlugin:BaseUnityPlugin {
        public const string ModVer = "2.0.0";
        public const string ModName = "GlassArtifactOHP";
        public const string ModGuid = "com.ThinkInvisible.GlassArtifactOHP";
        
        public void Awake() {
            IL.RoR2.CharacterBody.RecalculateStats += IL_CBRecalcStats;
        }

        private void IL_CBRecalcStats(ILContext il) {
            ILCursor c = new ILCursor(il);
            bool ILFound = c.TryGotoNext(MoveType.After,
                x => x.MatchLdcI4(0),
                x => x.MatchStloc(36),
                x => x.MatchLdarg(0),
                x => x.MatchLdarg(0),
                x => x.MatchCallvirt<CharacterBody>("get_isPlayerControlled")
                );
            if(ILFound) {
                c.RemoveRange(6);
            } else {
                Debug.LogError("GlassArtifactOHP: failed to apply IL patch! Mod not loaded.");
            }
        }
    }
    }
}