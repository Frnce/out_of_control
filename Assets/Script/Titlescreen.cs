using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titlescreen : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime = 1f;
    [SerializeField]
    private float fadeIntTime = 2f;
    [SerializeField]
    private float fadeWaitTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void StartGame()
    {
        StartCoroutine(TransitionToScene(1));
    }
    private IEnumerator TransitionToScene(int sceneIndex)
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(sceneIndex); //1 = castle 1-1 , build scene index
        yield return new WaitForSeconds(fadeWaitTime);
        yield return fader.FadeIn(fadeIntTime);
    }
}
