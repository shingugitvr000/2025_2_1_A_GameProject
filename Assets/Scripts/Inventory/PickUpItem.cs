using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : InteractableObject
{
    public ItemData itemData;                           //이 오브젝트가 가진 아이템 데이터 
    public int amount = 1;                              //아이템 개수 

    public override void Interact()
    {
        base.Interact();

        if (InventoryManager.Instance != null)              //인벤토리 매니저가 있으면
        {
            bool added = InventoryManager.Instance.AddItem(itemData, amount);           //인벤토리에 아이템 추가 시도 

            if (added)                                  //성공하면 오브젝트 제거
            {
                Destroy(gameObject);
            }
        }
    }
}
