using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("상호작용 정보")]
    public string objectName = "아이템 ";
    public string interactionText = "[E] 상호작용";
    public InteractionType interactionType = InteractionType.Item;                  //타입은 우선 Item

    [Header("하이라이트 설정")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 1.5f;

    public Renderer objectRenderer;
    private Color originalColor;
    private bool isHighlighted = false;

    public enum InteractionType
    {
        Item,                       //아이템 (동전, 열쇠 등)
        Machine,                    //기계 (레버 ,버튼 등)
        Builing,                    //건물 (문, 상자 등)
        NPC,                        //NPC
        Collectible                 //수집품
    }

  
    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();  
        if(objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        gameObject.layer = 8;           //(Layer 8 = Interactable 로 가정)
    }

    public virtual void OnPlayerEnter()
    {
        Debug.Log($"[{objectName}) 감지됨");
        HighlightObject();
    }

    public virtual void OnPlayerExit()
    {
        Debug.Log($"[{objectName}] 범위에서 벗어남");
        RemoveHighlight();
    }


    public virtual void Interact()
    {
        //상호작용 타입에 따른 기본 동작
        switch (interactionType)
        {
            case InteractionType.Item:
                CollectItem();
                break;
            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Builing:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                CollectItem();
                break;
        }
    }

    public string GetInteractionText()
    {
        return interactionText;
    }

    protected virtual void HighlightObject()                                            //가상 함수로 하이라이트 구현 
    {
        if (objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", highlightIntensity);
            isHighlighted = true;
        }
    }

    protected virtual void RemoveHighlight()                                            //가상 함수로 하이라이트 제거 구현 
    {
        if (objectRenderer != null && isHighlighted)
        {
            objectRenderer.material.color = originalColor;
            objectRenderer.material.SetFloat("_Emission", 0f);
            isHighlighted = false;
        }
    }

    protected virtual void CollectItem()
    {
        Debug.Log($"{objectName}을(를) 획득했습니다!");
        Destroy(gameObject);
    }

    protected virtual void OperateMachine()
    {
        Debug.Log($"{objectName}을(를) 작동시켰습니다.!");
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.green;
        }
    }

    protected virtual void AccessBuilding()
    {
        Debug.Log($"{objectName}을(를) 에 접근했습니다.");
        transform.Rotate(Vector3.up * 90f);
    }

    protected virtual void TalkToNPC()
    {
        Debug.Log($"{objectName}와 대화를 시작합니다. ");
    }
}
