using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    private static PlayerScript instance = null;
    public static PlayerScript Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            return null;
        }
    }
    public int maxHealth;
    private int currentHealth;
    public bool inControl = true;
    public Transform entitySpriteRenderer;
    [Space]
    public float movementSpeed = 25f;
    private Vector3 playerDir;
    private Rigidbody2D rb2d;
    private GameManager gameManager;
    private AudioManager audioManager;
    private Animator anim;

    private Shader hitShader;
    private Shader defaultShader;
    private Sprite defaultSprite;


    public HealthUIScript healthUI;
    public AudioClip hitSfx;
    public ParticleSystem dustFx;

    private Camera mainCamera;
    public bool IsFacingRight { get; set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
        audioManager = AudioManager.Instance;
        anim = GetComponent<Animator>();

        hitShader = Shader.Find("GUI/Text Shader");
        defaultShader = Shader.Find("Sprites/Default");
        defaultSprite = entitySpriteRenderer.GetComponent<SpriteRenderer>().sprite;

        currentHealth = maxHealth;
        IsFacingRight = true;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameStates == GameStates.GAME)
        {
            playerDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (playerDir.x != 0 || playerDir.y != 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
            if (playerDir.x < 0 && IsFacingRight)
            {
                FlipSprite();
            }
            if (playerDir.x > 0 && !IsFacingRight)
            {
                FlipSprite();
            }
            Move();
        }
    }
    private void Move()
    {
        var movement = playerDir.normalized * movementSpeed * Time.deltaTime;
        rb2d.MovePosition(transform.position + movement);
    }
    public void FlipSprite()
    {
        dustFx.Play();
        IsFacingRight = !IsFacingRight;

        Vector3 theScale = entitySpriteRenderer.localScale;
        theScale.x *= -1;   
        entitySpriteRenderer.localScale = theScale;
    }
    public void HitPlayer()
    {
        audioManager.PlaySingle(hitSfx);
        mainCamera.GetComponent<ScreenShakeScript>().TriggerShake();
        currentHealth--;
        healthUI.ReduceHeart();
        entitySpriteRenderer.GetComponent<SpriteRenderer>().material.shader = hitShader;
        entitySpriteRenderer.GetComponent<SpriteRenderer>().material.color = Color.white;
        if (currentHealth <= 0)
        {
            //gameover
            gameManager.ChangeState(GameStates.GAMEOVER);
            Time.timeScale = 0f;
            Debug.Log("Gameover");
        }
        else
        {
            FindObjectOfType<HitStop>().Stop(0.2f);
            StartCoroutine(WaitForSpawn());
        }
    }
    IEnumerator WaitForSpawn() // for hitStop and other hit fx
    {
        while (Time.timeScale != 1.0f)
        {
            yield return null;//wait for hit stop to end
        }
        yield return new WaitForSeconds(0.1f);
        entitySpriteRenderer.GetComponent<SpriteRenderer>().material.shader = defaultShader;
    }
    public int CurrentHealth { get { return currentHealth; } }
}
