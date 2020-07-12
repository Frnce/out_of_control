using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CorruptedOrbScript : MonoBehaviour
{
    private bool isInteractable = false;

    private PlayerScript player;
    private GameManager gameManager;

    public GameObject directionCanvas;
    public Sprite corruptedOrb;
    public SpriteRenderer orbRenderer;
    public CanvasGroup flashfx;

    public GameObject unCorruptParticle;
    public GameObject corrupteParticle;

    public AudioClip corruptSfx;

    private AudioManager audioManager;
    private bool flash = false;
    private bool isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerScript.Instance;
        gameManager = GameManager.Instance;

        directionCanvas.SetActive(false);

        unCorruptParticle.SetActive(true);
        corrupteParticle.SetActive(false);

        audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (flash)
        {
            flashfx.alpha -= Time.deltaTime;
            if (flashfx.alpha <= 0)
            {
                flashfx.alpha = 0;
                flash = false;
            }
        }
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInteractable = false;
                isTriggered = true;
                audioManager.PlaySingle(corruptSfx);
                directionCanvas.SetActive(false);
                doFlash();
                orbRenderer.sprite = corruptedOrb;

                unCorruptParticle.SetActive(false);
                corrupteParticle.SetActive(true);
                //StartCoroutine(OrbSequence());
                player.inControl = false;
                gameManager.StartTime();
            }
            directionCanvas.SetActive(true);
        }
        else
        {
            directionCanvas.SetActive(false);
        }
    }
    private void doFlash()
    {
        flash = true;
        flashfx.alpha = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isInteractable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteractable = false;
        }
    }
}
