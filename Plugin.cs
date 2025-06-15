using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Platforms.Patches;
using UnityEngine;
using Locomotion;
using Caputilla.Utils;

namespace Platforms
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Init : BasePlugin
    {
        public static Init instance;
        public Harmony harmonyInstance;

        public override void Load()
        {
            harmonyInstance = HarmonyPatcher.Patch(Constants.GUID);
            instance = this;

            AddComponent<Plugin>();
        }

        public override bool Unload()
        {
            if (harmonyInstance != null)
                HarmonyPatcher.Unpatch(harmonyInstance);

            return true;
        }
    }

    public class Plugin : MonoBehaviour
    {
        public GameObject lplat = null;
        public GameObject rplat = null;
        public static bool isModded;
        public void Start()
        {
            Debug.Log($"{Constants.Name} has loaded!");

            Caputilla.CaputillaManager.Instance.OnModdedJoin += OnJoin;
            Caputilla.CaputillaManager.Instance.OnModdedLeave += OnLeave;
        }

        public void Update()
        {
            if (isModded)
            {
                if (ControllerInputManager.Instance.leftGrip)
                {
                    if (lplat == null)
                    {
                        lplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        lplat.transform.localScale = new Vector3(0.3f, 0.02f, 0.3f);
                        lplat.transform.position = Player.Instance.LeftHand.transform.position + new Vector3(0f, -0.02f, 0f); // thanks HanSolo1000Falcon
                        lplat.transform.rotation = Player.Instance.LeftHand.transform.rotation;
                    }
                }
                else
                {
                    if (lplat != null)
                    {
                        Destroy(lplat);
                        lplat = null;
                    }
                }

                if (ControllerInputManager.Instance.rightGrip)
                {
                    if (rplat == null)
                    {
                        rplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        rplat.transform.localScale = new Vector3(0.3f, 0.02f, 0.3f);
                        rplat.transform.position = Player.Instance.RightHand.transform.position + new Vector3(0f, -0.02f, 0f); // thanks HanSolo1000Falcon
                        rplat.transform.rotation = Player.Instance.RightHand.transform.rotation;
                    }
                }
                else
                {
                    if (lplat != null)
                    {
                        Destroy(lplat);
                        lplat = null;
                    }
                }
            }
        }

        private void OnJoin() => isModded = true;

        private void OnLeave()
        {
            isModded = false;
            Destroy(lplat);
            lplat = null;
            Destroy(rplat);
            rplat = null;
        }
    }
}
