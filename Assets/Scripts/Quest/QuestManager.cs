using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI ��ҵ�")]
    public GameObject questUI;
    public Text questTitleText;
    public Text questDescriptionText;
    public Text questProgressText;
    public Button completeButton;

    [Header("����Ʈ ���")]
    public QuestData[] availableQuests;

    private QuestData currentQuest;
    private int currentQuestIndex = 0;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (availableQuests.Length > 0)
        {
            StartQuest(availableQuests[0]);
        }
        if(completeButton != null)
        {
            completeButton.onClick.AddListener(CompleteCurrentQuest);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentQuest != null && currentQuest.isActive)
        {
            CheckQuestProgress();
            UpdateQuestUI();
        }
    }

    //UI ������Ʈ (����Ʈ ���� ��Ȳ UI�� ǥ��)
    void UpdateQuestUI()
    {
        if (currentQuest == null) return;

        if (questTitleText != null)
        {
            questTitleText.text = currentQuest.questTitle;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = currentQuest.description;
        }

        if (questProgressText != null)
        {
            questProgressText.text = currentQuest.GetProgressText();
        }

    }

    //����Ʈ ����
    public void StartQuest(QuestData quest)
    {
        if (quest == null) return;

        currentQuest = quest;                       //����Ʈ�� �޾ƿͼ� CurrentQuest�� �����Ѵ�. 
        currentQuest.Initialize();                  //���� ����Ʈ�� �ʱ�ȭ �Ѵ�.
        currentQuest.isActive = true;

        Debug.Log("����Ʈ ���� : " + questTitleText);
        UpdateQuestUI();
        if (questUI != null)
        {
            questUI.SetActive(true);
        }
    }

    //��� ����Ʈ ���� üũ
    void CheckDeliveryProgress()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;               //������ ��ġ�� ã�´�.
        if(player == null) return;

        float distance = Vector3.Distance(player.position, currentQuest.deliveryPosition);      //������ ������ �Ÿ��� ����Ѵ�.

        if (distance <= currentQuest.deliveryRedius)                                            //������ �Ÿ��� �������� �������� �˻�
        {
            if(currentQuest.currentProgress == 0)
            {
                currentQuest.currentProgress = 1;                                               //����Ʈ �Ϸ�
            }
        }
        else
        {
            currentQuest.currentProgress = 0;                                                   //�������� ����
        }
    }

    //���� ����Ʈ ���� (�ܺο��� ȣ��)
    public void AddCollectProgress(string itemTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Collect && currentQuest.targetTag == itemTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("������ ���� : " + itemTag);
        }
    }

    //��ȣ�ۿ� ����Ʈ (�ܺο��� ȣ��)
    public void AddInteractProgress(string objectTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Interact && currentQuest.targetTag == objectTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("��ȣ �ۿ� �Ϸ� : " + objectTag);
        }
    }

    //���� ����Ʈ �Ϸ�
    public void CompleteCurrentQuest()
    {
        if (currentQuest == null || !currentQuest.isCompleted) return;

        Debug.Log("����Ʈ �Ϸ� !" + currentQuest.rewardMessage);

        // �Ϸ� ��ư ��Ȱ��ȭ
        if (completeButton != null)
        {
            completeButton.gameObject.SetActive(false);
        }

        //���� ����Ʈ�� ������ ����
        currentQuestIndex++;
        if (currentQuestIndex < availableQuests.Length)
        {
            StartQuest(availableQuests[currentQuestIndex]);
        }
        else
        {
            //��� ����Ʈ �Ϸ�
            currentQuest = null;
            if(questUI != null)
            {
                questUI.gameObject.SetActive(false);
            }
        }
    }

    //����Ʈ ���� üũ
    void CheckQuestProgress()
    {
        if (currentQuest.questType == QuestType.Delivery)
        {
            CheckDeliveryProgress();
        }

        //����Ʈ �Ϸ� üũ
        if (currentQuest.IsComplete() && !currentQuest.isCompleted)
        {
            currentQuest.isCompleted = true;

            //�Ϸ� ��ư Ȱ��ȭ 
            if (completeButton != null)
            {
                completeButton.gameObject.SetActive(true);
            }                  
        }
    }

}
