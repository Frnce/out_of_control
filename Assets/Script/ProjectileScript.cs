using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PhysicsMaterial2D bounceMaterial;
    private Vector2 direction;
    private float angle;
    private float speed;
    public float speedMultiplier = 1.2f;
    public int maxBounce = 5;
    public AudioClip hitSfx;
    public GameObject playerHitParticles;
    public GameObject dustEffectOnhit;

    private int currentBounce;
    private float newSpeed; 

    private bool moveNow = false;

    private Rigidbody2D rb2d;
    private PlayerScript player;
    private AudioManager audioManager;

    private Animator anim;
    public void Setup(Vector3 _dir,float _angle,float _speed)
    {
        direction = _dir;
        angle = _angle;
        speed = _speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerScript.Instance;
        audioManager = AudioManager.Instance;

        moveNow = false;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        Quaternion quat = Quaternion.identity;
        quat.eulerAngles = new Vector3(0, 0, angle);
        transform.localRotation = quat;
        moveNow = true;

        rb2d.velocity = (direction) * speed;
        newSpeed = speed;

        if (!player.inControl)
        {
            rb2d.sharedMaterial = bounceMaterial;
        }

        anim.SetTrigger("Squash");
    }
    private void FixedUpdate()
    {
        float x = 0f;
        x += Time.deltaTime * 10;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, x));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject dustfx = Instantiate(dustEffectOnhit, transform.position, Quaternion.identity);
        anim.SetTrigger("Squash");
        Destroy(dustfx, 0.5f);
        ////New Random direction
        //Vector3 newDirection = direction;
        //Increase speed
        audioManager.RandomizeSfx(hitSfx);
        if (!player.inControl)
        {
            newSpeed *= speedMultiplier;
            rb2d.velocity = (direction) * newSpeed;
            currentBounce++;
            if (currentBounce >= maxBounce)
            {
                //TODO add explosion
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        //increment the bounce limiter
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT");
        if (collision.CompareTag("Player"))
        {
            GameObject particle = Instantiate(playerHitParticles, transform.position, Quaternion.identity);
            Destroy(particle, 2f);
            player.HitPlayer();
            Destroy(gameObject);
        }  
    }
}
