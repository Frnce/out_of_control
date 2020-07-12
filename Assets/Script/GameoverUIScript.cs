using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverUIScript : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RestartGame()
    {
        gameManager.RestartGame();
    }
}