using RoR2;
using BepInEx;
using MonoMod.Cil;
using UnityEngine;
using System;
using System.Collections.Generic;
using R2API;
using Mono.Cecil.Cil;
using System.Reflection;
using R2API.Utils;
using R2API.AssetPlus;

namespace ThinkInvisible.GlassArtifactOHP {
    
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [R2APISubmoduleDependency(nameof(AssetPlus), nameof(ResourcesAPI))]
    public class GlassArtifactOHPPlugin:BaseUnityPlugin {
        public const string ModVer = "3.0.1";
        public const string ModName = "GlassArtifactOHP";
        public const string ModGuid = "com.ThinkInvisible.GlassArtifactOHP";
        
        public ArtifactDef dangerArtifact {get;private set;}

        private bool ilFailed = false;

        public void Awake() {
            using(var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GlassArtifactOHP.glassohp_assets")) {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider("@GlassArtifactOHP", bundle);
                ResourcesAPI.AddProvider(provider);
            }

            IL.RoR2.CharacterBody.RecalculateStats += IL_CBRecalcStats;
            if(!ilFailed)
                ArtifactCatalog.getAdditionalEntries += Evt_ACGetAdditionalEntries;
        }

        private void Evt_ACGetAdditionalEntries(List<ArtifactDef> addEnts) {
            dangerArtifact = ScriptableObject.CreateInstance<ArtifactDef>();
            dangerArtifact.nameToken = "Danger";
            dangerArtifact.descriptionToken = "Allows enemies to kill you with one hit.";
            dangerArtifact.smallIconDeselectedSprite = Resources.Load<Sprite>("@GlassArtifactOHP:Assets/GlassArtifactOHP/danger-off.png");
            dangerArtifact.smallIconSelectedSprite = Resources.Load<Sprite>("@GlassArtifactOHP:Assets/GlassArtifactOHP/danger-on.png");

            addEnts.Add(dangerArtifact);
        }

        private void IL_CBRecalcStats(ILContext il) {
            ILCursor c = new ILCursor(il);
            bool ILFound = c.TryGotoNext(
                x=>x.OpCode == OpCodes.Ldloc_S,
                x=>x.MatchLdcI4(0),
                x=>x.MatchCeq(),
                x=>x.OpCode == OpCodes.Br_S,
                x=>x.MatchLdcI4(0),
                x=>x.MatchCallOrCallvirt<CharacterBody>("set_hasOneShotProtection")
                );
            if(ILFound) {
                c.RemoveRange(3);
                c.EmitDelegate<Func<bool>>(()=>{
                    return !RunArtifactManager.instance.IsArtifactEnabled(dangerArtifact);
                });
            } else {
                ilFailed = true;
                Debug.LogError("GlassArtifactOHP: failed to apply IL patch! Mod not loaded.");
            }
        }
    }
}