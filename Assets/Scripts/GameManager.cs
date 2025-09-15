using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("게임 상태")]
    public int playerScore = 0;
    public int itemsCollected = 0;

    [Header("UI 참조")]
    public Text scoreText;
    public Text itemCountText;
    public Text gameStatusText;

    public static GameManager Instance;             //싱글톤 패턴


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);          //씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log($"아이템 수집! (총 : {itemsCollected} 개");

    }

    void UpdateUI()
    {
        if(scoreText != null)
        {
            scoreText.text = "점수 : " + playerScore;
        }
        if(itemCountText != null)
        {
            itemCountText.text = "아이템 : " + itemsCollected + "개";
        }
    }
}
