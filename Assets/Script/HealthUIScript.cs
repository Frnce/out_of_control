using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HeartUI
{
    public bool isEmpty;
    public Image heartImage;
}
public class HealthUIScript : MonoBehaviour
{
    public HeartUI[] heart;

    public Sprite emptyHeartSprite;
    public Sprite fullHeartSprite;

    private PlayerScript player;
    private int playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerScript.Instance;

        playerHealth = player.CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReduceHeart()
    {
        for (int i = 0; i < heart.Length; i++)
        {
            if (heart[i].isEmpty)
            {
                continue;
            }
            else
            {
                heart[i].isEmpty = true;
                heart[i].heartImage.sprite = emptyHeartSprite;
                break;
            }
        }
    }
}
