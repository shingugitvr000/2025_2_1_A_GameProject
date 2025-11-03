using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    [Header("Achievement Settings")]
    public List<AchievementData> allAchievements = new List<AchievementData>();

    [Header("UI References")]
    public GameObject achievementPopupPrefab;
    public Transform popupParent;
    public GameObject achievementPanel;
    public Transform achievementListContent;
    public GameObject achievementSlotPrefab;

    private Dictionary<AchievementType, int> progressData = new Dictionary<AchievementType, int>();     //통계 저장

    void Awake()
    {
        if (instance == null)                           //싱글톤 화 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetAllAchievements();                                 //시작시에 리셋 강제로 (테스트용) 나중에 배포시에는 지운다. 
        foreach(AchievementType type in System.Enum.GetValues(typeof(AchievementType)))             //각 타입별 초기화
        {
            progressData[type] = 0;
        }
        LoadAchievements();
        UpdateAchievementUI();
    }


    //업적 UI 업데이트 
    public void UpdateAchievementUI()
    {
        if (achievementListContent == null || achievementSlotPrefab == null)
            return;

        //기존 슬롯 제거
        foreach(Transform child in achievementListContent)
        {
            Destroy(child.gameObject);
        }

        foreach(AchievementData achievement in allAchievements)
        {
            GameObject slot = Instantiate(achievementSlotPrefab, achievementListContent);
            AchievementSlot slotScript = slot.GetComponent<AchievementSlot>();
            if(slotScript != null)
            {
                slotScript.SetAchievement(achievement, GetProgress(achievement));
            }
        }
    }

    public void UpdateProgress(AchievementType type , int amount = 1)           //진행도 업데이트 - 모든 업적이 이 함수를 통해 처리
    {
        progressData[type] += amount;

        //해당 타입의 모든 업적 체크
        foreach(AchievementData achievement in allAchievements)
        {
            if (achievement.achievementType == type && !achievement.isUnlocked)
            {
                if(progressData[type] >= achievement.requiredAmount)
                {
                    UnlockAchievement(achievement);
                }
            }
        }
    }

    void UnlockAchievement(AchievementData achievement)             //업적 언락
    {
        achievement.isUnlocked = true;
        //보상이 있는 업적일 경우 보상 로직을 여기에 넣는다.                                     
        ShowAchievementPopup(achievement);
        UpdateAchievementUI();
    }


    void ShowAchievementPopup(AchievementData achievement)                              //업적 팝업 표시 
    {
        if (achievementPopupPrefab != null && popupParent != null)
        {
            GameObject popup = Instantiate(achievementPopupPrefab, popupParent);

            Text titleText = popup.transform.Find("Title")?.GetComponent<Text>();
            Text descText = popup.transform.Find("Description")?.GetComponent<Text>();

            if (titleText != null) titleText.text = "업적 달성";
            if (descText != null) descText.text = achievement.achievementName;

            Destroy(popup, 3.0f);
        }
    }


    public float GetProgress(AchievementData achievement)               //진행도 가져오기 
    {
        if (achievement.isUnlocked) return 1f;
        int current = progressData.ContainsKey(achievement.achievementType) ? progressData[achievement.achievementType] : 0;
        return Mathf.Min((float)current / achievement.requiredAmount, 1f);
    }

    void SaveAchievements()                                     //데이터 저장
    {
        foreach (var kvp in progressData)
        {
            PlayerPrefs.SetInt("Achievement_" + kvp.Key, kvp.Value);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            PlayerPrefs.SetInt("Unlocked_" + achievement.name , achievement.isUnlocked ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    void LoadAchievements()                                         //데이터 로드
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = PlayerPrefs.GetInt("Achievement_" + type, 0);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt("Unlocked_" + achievement.name, 0) == 1;
        }
    }

    public void ResetAllAchievements()                              //업적 초기화 (리셋)
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))        //모든 진행도 초기화 
        {
            progressData[type] = 0;
            PlayerPrefs.DeleteKey("Achievement_" + type);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = false;
            PlayerPrefs.DeleteKey("Unlocked_" + achievement.name);
        }

        PlayerPrefs.Save();
        UpdateAchievementUI();
        
    }
}
