using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    //This script will run on when the game is loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        //Put all the manager objects here so they will be spawned first before others
        GameObject go = Instantiate(Resources.Load("Main")) as GameObject;
        DontDestroyOnLoad(go);
    }
}
