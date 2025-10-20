using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;                             //최대 체력
    public int currentHealth = 100;                         //현재 체력
        
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("현재 체력 : " + currentHealth);
    }
    
    public void TakeDamage(int amount)
    {
        if(amount > 0)
        {
            currentHealth -= amount;
        }
        else
        {
            currentHealth += amount;
        }
          
        if ( currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("사망");
        }
        Debug.Log("현재 체력 : " + currentHealth);
    }

}
