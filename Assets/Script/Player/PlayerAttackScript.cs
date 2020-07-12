using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public GameObject playerHand;
    public SpriteRenderer weaponSprite;
    [Space]
    public Sprite uncorruptedWeaponSprite;
    public Sprite corruptedWeaponSprite;
    public TrailRenderer unCorruptedTrail;
    public TrailRenderer corruptedTrail;
    [Space]
    public float fireRate = 0.1f;
    public float bulletSpeed = 0f;
    public Transform ammoEmitter;
    private Vector3 mouse;
    public GameObject bullet;
    public AudioClip shootSfx;
    public float GetMouseAngle { get; private set; }
    private bool canShoot = true;

    private Animator anim;
    private Animator weaponAnim;
    private PlayerScript player;
    private AudioManager audioManager;


    float maxTime = 0.1f;
    float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerScript>();
        anim = GetComponent<Animator>();
        audioManager = AudioManager.Instance;
        weaponAnim = playerHand.GetComponent<Animator>();
        currentTime = maxTime;

        unCorruptedTrail.enabled = false;
        corruptedTrail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if ((GetMouseAngle > 5 && GetMouseAngle < 170) && player.IsFacingRight) //left
        //{
        //    player.FlipSprite();
        //    weaponSprite.sortingOrder = player.entitySpriteRenderer.GetComponent<SpriteRenderer>().sortingOrder + 1;
        //    FlipWeapon();
        //}
        //else if ((GetMouseAngle > 170 && GetMouseAngle < 360) && !player.IsFacingRight) // right
        //{
        //    player.FlipSprite();

        //    weaponSprite.sortingOrder = player.entitySpriteRenderer.GetComponent<SpriteRenderer>().sortingOrder - 1;
        //    FlipWeapon();
        //}
        if(GameManager.Instance.gameStates == GameStates.GAME)
        {
            if (player.inControl)
            {
                weaponSprite.sprite = uncorruptedWeaponSprite;
                Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartCoroutine(AttackRoutine());
                }
            }
            else
            {
                weaponSprite.sprite = corruptedWeaponSprite;
                if (currentTime <= 0f)
                {
                    Aim(new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f)));
                    StartCoroutine(AttackRoutine());
                    currentTime = maxTime;
                }
                else
                {
                    currentTime -= Time.deltaTime;
                }
            }
        }
    }
    private void FlipWeapon()
    {
        Vector3 theScale = weaponSprite.transform.localScale;
        theScale.x *= -1;
        weaponSprite.transform.localScale = theScale;
    }
    private void Aim(Vector3 dir)
    {
        //The Weapon should be facing up or vector2.up for it to work properly
        mouse = dir;
        mouse.z = Camera.main.nearClipPlane;
        Vector2 mouseVector = (mouse - transform.position).normalized;
        float gunAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg - 90f;
        GetMouseAngle = gunAngle;
        playerHand.transform.rotation = Quaternion.AngleAxis(GetMouseAngle, Vector3.forward);
    }
    private IEnumerator AttackRoutine()
    {
        //For trail renderer
        if (player.inControl)
        {
            unCorruptedTrail.enabled = true;
        }
        else
        {
            corruptedTrail.enabled = true;
        }
        canShoot = false;
        weaponAnim.SetTrigger("Attack");
        //ammoEmitter.localEulerAngles = new Vector3(0f, 0f, Random.Range(-50f, 50f));
        ProjectileScript ammo = Instantiate(bullet, ammoEmitter.position, ammoEmitter.localRotation).GetComponent<ProjectileScript>();
        ammo.Setup(ammoEmitter.up, GetMouseAngle, bulletSpeed);
        audioManager.RandomizeSfx(shootSfx);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        //For trail renderer
        if (player.inControl)
        {
            unCorruptedTrail.enabled = false;
        }
        else
        {
            corruptedTrail.enabled = false;
        }
    }
}
