using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement" , menuName = "Achievement/Achievement Data")]
public class AchievementData : ScriptableObject
{
    public string achievementName;
    public string description;
    public AchievementType achievementType;
    public int requiredAmount;                          //필요 수량 (예 : 코인 10개)
    public int rewardCoins;                             //보상 코인
    public bool isUnlocked;                             //달성 여부
    public Sprite icon;                                 //업적 아이콘
}
