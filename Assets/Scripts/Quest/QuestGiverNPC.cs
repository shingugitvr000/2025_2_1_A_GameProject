using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverNPC : InteractableObject
{
    [Header("NPC Quest Settings")]
    public QuestData questToGive;
    public string npcName = "NPC";
    public string questStartMessage = "새로운 퀘스트가 있습니다. ";
    public string noQuestMessage = "퀘스트가 없습니다.";
    public string QuestAlreadyActiveMessage = "이미 진행중인 퀘스트가 있습니다.";

    private QuestManager questManager;

    protected override void Start()
    {
        base.Start();
        questManager = FindObjectOfType<QuestManager>();    

        if (questManager == null)
        {
            Debug.LogError("QuestManager 가 없습니다.");
        }
        interactionText = "[E] " + npcName + "와 대화하기";
    }

    public override void Interact()
    {
        base.Interact();

        questManager.StartQuest(questToGive);
    }

    private void Update()
    {
        if (questToGive != null && questManager != null && questManager.currentQuest == null)
        {
            interactionText = "[E]" + npcName + "와 대화하기";
        }
        else if(questManager != null && questManager.currentQuest != null)
        {
            interactionText = "[E] " + npcName;
        }
    }
}
