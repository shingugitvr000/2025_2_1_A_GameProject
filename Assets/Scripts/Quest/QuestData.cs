using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest" , menuName = "Quest System/Quest")]

public class QuestData : ScriptableObject
{
    [Header("기본 정보")]
    public string questTitle = "새로운 퀘스트";                                       //퀘스트 제목
    [TextArea(2, 4)]
    public string description = "퀘스트 설명을 입력하세요";                           //퀘스트 내용
    public Sprite questIcon;                                                          //퀘스트 아이콘

    [Header("퀘스트 설정")]
    public QuestType questType;                                                       //퀘스트 종류
    public int targetAmount = 1;                                                      //목표 수량(수집용)

    [Header("배달 퀘스트용 (Delivery)")]
    public Vector3 deliveryPosition;                                                  //배달 목적지
    public float deliveryRedius = 3f;                                                 //도달 인정 범위 

    [Header("수집/상호작용 퀘스트용")]
    public string targetTag = "";                                                     //대상 오브젝트 태그 

    [Header("보상")]
    public int experienceReward = 100;
    public string rewardMessage = "퀘스트 완료";

    [Header("퀘스트 연결")]
    public QuestData nextQuest;                                                       //다음 퀘스트 (선택사항)

    //런타임 데이터 (저장되지 않음)
    [System.NonSerialized] public int currentProgress = 0;
    [System.NonSerialized] public bool isActive = false;
    [System.NonSerialized] public bool isCompleted = false;

    //퀘스트 초기화
    public void Initialize()
    {
        currentProgress = 0;
        isActive = false;
        isCompleted = false;    
    }

    //퀘스트 완료 체크
    public bool IsComplete()
    {
        switch(questType)
        {
            case QuestType.Delivery:
                return currentProgress >= 1;
            case QuestType.Collect:
            case QuestType.Interact:
                return currentProgress >= targetAmount;
            default: 
                return false;
        }
    }

    //진행률 계산 (0.0 ~ 1.0)
    public float GetProgressPercentage()
    {
        if (targetAmount <= 0) return 0f;
        return Mathf.Clamp01((float)currentProgress / targetAmount);
    }

    //진행 상황 텍스트
    public string GetProgressText()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return isCompleted ? "배달 완료!" : "목적지로 이동하세요";
            case QuestType.Collect:
                return $"{currentProgress} / {targetAmount}";
            case QuestType.Interact:
                return $"{currentProgress} / {targetAmount}";
            default:
                return "";
        }
    }


}
