using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum FXType
{
    Default,
    Poof,
    HooAdultIdle,
    HooBabyIdle,
    Click,
    Fire,
    CyberDeath,
}

public class FX_Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SerializedDict
    {
        public FXType key;
        public UnityEngine.GameObject value;
        public bool onesie;
    }

    public AudioMixerGroup mixer;
    private UnityEngine.GameObject holder;

    public List<SerializedDict> Serialized_FX_Dict = new List<SerializedDict>();
    public Dictionary<FXType, UnityEngine.GameObject> FX_Dict = new Dictionary<FXType, UnityEngine.GameObject>();
    Dictionary<FXType, bool> onesieChecker = new Dictionary<FXType, bool>();
    Dictionary<FXType, GameObject> onesieTracker = new Dictionary<FXType, GameObject>();

    public GameObject fx_default;

    // Singleton code
    public static FX_Spawner instance;
    private void Awake() {
        if (null == instance) {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        foreach (var entry in Serialized_FX_Dict)
        {
            FX_Dict[entry.key] = entry.value;
            onesieChecker[entry.key] = entry.onesie;
        }
        if (FX_Dict.ContainsKey(FXType.Default))
        {
            FX_Dict[FXType.Default] = null;
            onesieChecker[FXType.Default] = false;
        }
        holder = new UnityEngine.GameObject("FX Objects");
    }


    public UnityEngine.GameObject SpawnFX(UnityEngine.GameObject fx, Vector3 position, Vector3 rotation, float vol = -1, Transform parent = null, FXType effectName=FXType.Default) {
        if (fx == null) return null;

        if (onesieTracker.ContainsKey(effectName))
        {
            if (onesieTracker[effectName])
                return null;
        }

        UnityEngine.GameObject spawned_fx = Instantiate(fx, position, Quaternion.identity);
        if (onesieChecker.ContainsKey(effectName) && onesieChecker[effectName])
        {
            onesieTracker[effectName] = spawned_fx;
        }


        if (spawned_fx == null) return null;

        spawned_fx.transform.parent = (parent != null ? parent : holder.transform);

        if (rotation != Vector3.zero)
            spawned_fx.transform.forward = rotation;
        FX_Object fx_obj = spawned_fx.GetComponent<FX_Object>();
        fx_obj.vol = vol;
        fx_obj.mixerGroup = mixer;

        return spawned_fx;
    }

    public UnityEngine.GameObject SpawnFX(FXType effectName, Vector3 position, Vector3 rotation, float vol = -1, Transform parent = null, bool onesie=false) {
        if (!FX_Dict.ContainsKey(effectName))
            return SpawnFX(fx_default, position, rotation, vol, parent, FXType.Default);
        return SpawnFX(FX_Dict[effectName], position, rotation, vol, parent, effectName);
        //return SpawnFX(FX_Dict.GetValueOrDefault(effectName, FX_Dict[FXType.Default]), position, rotation, vol, parent);
    }

    public UnityEngine.GameObject SpawnFX(FXType effectName, Vector3 position, Quaternion rotation, float vol = -1, Transform parent = null, bool onesie=false)
    {
        if (!FX_Dict.ContainsKey(effectName))
            return SpawnFX(fx_default, position, rotation.eulerAngles, vol, parent, FXType.Default);
        return SpawnFX(FX_Dict[effectName], position, rotation.eulerAngles, vol, parent, effectName);
    }
}