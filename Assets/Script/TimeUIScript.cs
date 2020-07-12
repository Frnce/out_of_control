using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUIScript : MonoBehaviour
{
    public TMP_Text text;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = gameManager.TimeSurvived();
    }
}
