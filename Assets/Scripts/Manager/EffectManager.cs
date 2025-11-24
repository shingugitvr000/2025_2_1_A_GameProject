using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [System.Serializable]
    public class EffectData
    {
        public string effectName;                       //이펙트 이름 
        public GameObject effectPrefabs;                //이펙트 프리팹
        public float defaultDuration = 2f;              //기본 지속 시간
    }
    [Header("이펙트 목록")]
    [SerializeField] private List<EffectData> effectList = new List<EffectData>();   
    private Dictionary<string, EffectData> effectDictionary = new Dictionary<string, EffectData>();  //이펙트를 이름으로 빠르게 찾기 위한 딕셔너리 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionary()             //리스트를 딕셔너리로 변환
    {
        effectDictionary.Clear();
        foreach (var effect in effectList)
        {
            if (!effectDictionary.ContainsKey(effect.effectName))
            {
                effectDictionary.Add(effect.effectName, effect);
            }
            else
            {
                Debug.LogWarning($"중복된 이펙트 이름 : {effect.effectName}");
            }
        }
    }

    public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation)      //이펙트 재생 기본 형
    {
        if (effectDictionary.TryGetValue(effectName , out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefabs, position, rotation);
            Destroy(effect, data.defaultDuration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. : {effectName}");
            return null;
        }
    }

    public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation, float duration)  //이펙트 재생 지속 시간 설정 가능하게 하기 
    {
        if (effectDictionary.TryGetValue(effectName, out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefabs, position, rotation);
            Destroy(effect, duration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. : {effectName}");
            return null;
        }
    }

    public GameObject PlayEffect(string effectName , Vector3 position)      
    {
        return PlayEffect(effectName, position, quaternion.identity);
    }

    public GameObject PlayEffect(string effectName , Vector3 position , float duration) //이펙트 재생(지속시간 직접 설정)
    {
        return PlayEffect(effectName, position, quaternion.identity, duration);
    }

    public void PlayEffectWithDelay(string effectName, Vector3 position, Quaternion rotation, float delay, float duration)
    {
        StartCoroutine( PlayEffectDelayed(effectName, position, rotation, delay, duration));    
    }

    private IEnumerator PlayEffectDelayed(string effectName , Vector3 position, Quaternion rotation ,float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        if (duration > 0)
        {
            PlayEffect(effectName, position, rotation, duration);
        }
        else
        {
            PlayEffect(effectName, position, rotation);
        }

    }
}
