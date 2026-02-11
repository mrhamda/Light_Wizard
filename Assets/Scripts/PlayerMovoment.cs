using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
public class PlayerMovoment : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject magicBulletPrefab;
    [SerializeField] GameObject spawnMagicBulletSpawn;
    float horizontal;
    private Rigidbody2D rb2D;
    public float health = 100;
    public float magicBulletDamage = 28;
    [SerializeField] Slider healthBar;
    public float mana = 100;
    public float magicBulletManaCost = 50;
    public Slider manaSlider;
    public Animator anim;
    public bool canDamage;
    int trollAmount;
    [SerializeField] TextMeshProUGUI trollsAmountText;
    [SerializeField] AudioClip magicBulletCastAudioClip;
    AudioSource audioSource;
    [SerializeField] TextMeshProUGUI addedManaAndHealthText;
    [SerializeField] Animator addedManaAndHealthTextAnim;
    [SerializeField] AudioClip pickUpPowerUpsAudioClip;
    [SerializeField] GameObject dieMenu;
    public GameObject pauseMenu;
    public Animator pauseMenuAnim;
    public int score;
    public TextMeshProUGUI scoreText;
    public bool canMove = true;
    public float ultMagicBulleManaCost;
    public GameObject ultMagicBullePrefab;
    public float ultMagicBulletDamage;
    bool canUlt;
    float nextTimeToUlt;
    [SerializeField] TextMeshProUGUI ultText;
    float nextTimeToMana;
    [SerializeField] TextMeshProUGUI manaGainText;
    [SerializeField] AudioClip ultAudio;
    [SerializeField] AudioClip reChargeManaAudio;
    float amountAddedTimeToTime;
    float addedMana = 50f;
    [SerializeField] AudioSource volumeAudioSource;
    public float darkMana;
    public bool isDarkMood;
    public Slider darkManaSlider;
    [SerializeField] Sprite darkWizardSprite;
    [SerializeField] Sprite LightWizardSprite;
    private Vignette vignette;
    [SerializeField] PostProcessVolume volume;
    [SerializeField] AudioClip whooshAudioClip;
    [SerializeField] ParticleSystem TransformEffect;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        healthBar.maxValue = health;
        healthBar.minValue = 0;
        manaSlider.maxValue = mana;
        manaSlider.minValue = 0;
        
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        addedManaAndHealthText.enabled = false;
        addedManaAndHealthTextAnim.SetBool("Dissolove", true);

        dieMenu.SetActive(false);

        pauseMenu.SetActive(false);

        canMove = true;

       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        VolumePlayer();
        VolumeSoulFrageMent();
       
        if (canMove == true)
        {

            if (transform.tag == "Untagged")
            {
                ShootLightMagic();
                ShootDarkMagic();
            }

         
            AddManaEveryFram();

            Move();
            

            CannotMoveOutSideTheScreen();

            HealthAndMana();

            AddManaEveryFram();

            TrollText();

            PauseMenu();


            Mana();

            DarkMana();

        }
      


    }
    void DarkMana()
    {
        darkManaSlider.value = darkMana;
        if(darkMana < darkManaSlider.maxValue)
        {
            darkMana += 0.0001f;
        }
       
    }
    void VolumePlayer()
    {
        volumeAudioSource.volume = PlayerPrefs.GetFloat("SoundEffect");//Effect
        audioSource.volume = PlayerPrefs.GetFloat("Song");//Song
    }
    void VolumeSoulFrageMent()
    {
        GameObject[] soulsFrageMents = GameObject.FindGameObjectsWithTag("SoulFrageMent");
        foreach (GameObject _soulFrageMent in soulsFrageMents)
        {
            _soulFrageMent.GetComponent<AudioSource>().volume = audioSource.volume;
        }
    }

    void Mana()
    {
       

        if (Time.timeSinceLevelLoad < 60)
             {  
              magicBulletManaCost = 20;
             amountAddedTimeToTime = 40;
            ultMagicBulleManaCost = 75;
            addedMana = 100f;
        }
        else if(Time.timeSinceLevelLoad > 60 && Time.timeSinceLevelLoad <120)
             {
               magicBulletManaCost = 20;
              amountAddedTimeToTime = 20;
             ultMagicBulleManaCost = 90;
            addedMana = 200f;
        }
        else if(Time.timeSinceLevelLoad > 120 && Time.timeSinceLevelLoad < 240)
             {
              ultMagicBulleManaCost = 110;
            magicBulletManaCost = 10;
            amountAddedTimeToTime = 25;
            addedMana = 200f;
        }
           
        
    }
    void Move()
    {
         horizontal = Input.GetAxisRaw("Horizontal") * speed;
        if (horizontal == -speed)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontal == speed)
        {

            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        rb2D.velocity = new Vector2(horizontal, rb2D.velocity.y);
    }
  
    

    void CannotMoveOutSideTheScreen()
    {
       if(transform.position.x >= 13)
        {
           
            transform.position = new Vector3(13,transform.position.y,transform.position.z);
        }else if(transform.position.x <= -13)
        {
            
            transform.position = new Vector3(-13, transform.position.y, transform.position.z);

        }
    }

    void HealthAndMana()
    {
        if(health <=0)
        {
            dieMenu.SetActive(true);
            Destroy(gameObject);
        }
        manaSlider.value = mana;
        healthBar.value = health;
    }
    void ShootDarkMagic()
    {
       
        if(Input.GetKeyDown(KeyCode.V) && darkManaSlider.value >= darkManaSlider.maxValue)
        {
            audioSource.PlayOneShot(whooshAudioClip);
            isDarkMood = true;
            TransformEffect.Play();
        }
        if (isDarkMood == true )
        {
            vignette.active = true;
            volume.profile.TryGetSettings(out vignette);
           
            transform.GetComponent<SpriteRenderer>().sprite = darkWizardSprite;
            if(darkMana >0)
            {
                darkMana -= 0.05f;

            }
            if(darkMana <=0)
            {
                isDarkMood = false;
            }
            if (nextTimeToUlt - Time.time > 0)
            {
                ultText.text = (Mathf.RoundToInt(nextTimeToUlt - Time.time)).ToString();
                
            }
            else if (Time.time >= nextTimeToUlt)
            {
                ultText.text = "Ready";

            }
            if (Input.GetKeyDown(KeyCode.C) && Time.time >= nextTimeToMana)
            {
                audioSource.PlayOneShot(reChargeManaAudio);
                if (mana + addedMana + 100 > manaSlider.maxValue)
                {
                    float amount = manaSlider.maxValue - manaSlider.value;
                    mana += amount;
                }
                else
                {
                    mana += addedMana + 100;
                }
                nextTimeToMana = Time.time + amountAddedTimeToTime;
            }
            if (nextTimeToMana - Time.time > 0)
            {
                manaGainText.text = (Mathf.RoundToInt(nextTimeToMana - Time.time)).ToString();
            }
            else if (Time.time >= nextTimeToMana)
            {
                manaGainText.text = "Ready";

            }
            if (mana > ultMagicBulleManaCost)
            {
                if (Input.GetKeyDown(KeyCode.X) && Time.time >= nextTimeToUlt)
                {
                    audioSource.PlayOneShot(ultAudio);
                    darkMana -= 10;
                    GameObject magic_Bullet = Instantiate(ultMagicBullePrefab);
                    magic_Bullet.transform.position = spawnMagicBulletSpawn.transform.position;
                    magic_Bullet.transform.position = new Vector2(magic_Bullet.transform.position.x, magic_Bullet.transform.position.y + 1f);
                    magicBulletPrefab.transform.right = transform.right;
                    magic_Bullet.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                    if (transform.rotation == Quaternion.Euler(0, -180, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 90);

                    }
                    nextTimeToUlt = Time.time + (amountAddedTimeToTime - 10);
                    mana -= ultMagicBulleManaCost;

                }
            }
            if (mana > magicBulletManaCost)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z))
                {
                    darkMana -= 5;
                    audioSource.PlayOneShot(magicBulletCastAudioClip);
                    mana -= magicBulletManaCost;
                    GameObject magic_Bullet = Instantiate(magicBulletPrefab);
                    magic_Bullet.transform.position = spawnMagicBulletSpawn.transform.position;
                    magicBulletPrefab.transform.right = transform.right;
                    if (transform.rotation == Quaternion.Euler(0, -180, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 90);

                    }
                    magic_Bullet.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);



                }
            }

        }
    }
    void ShootLightMagic()
    {
        if (isDarkMood == false )
        {
            volume.profile.TryGetSettings(out vignette);
            vignette.active = false;
            transform.GetComponent<SpriteRenderer>().sprite = LightWizardSprite;
            if (nextTimeToUlt - Time.time > 0)
            {
                ultText.text = (Mathf.RoundToInt(nextTimeToUlt - Time.time)).ToString();
            }
            else if (Time.time >= nextTimeToUlt)
            {
                ultText.text = "Ready";

            }
            if (Input.GetKeyDown(KeyCode.C) && Time.time >= nextTimeToMana)
            {
                audioSource.PlayOneShot(reChargeManaAudio);
                if (mana + addedMana > manaSlider.maxValue)
                {
                    float amount = manaSlider.maxValue - manaSlider.value;
                    mana += amount;
                }
                else
                {
                    mana += addedMana;
                }
                nextTimeToMana = Time.time + amountAddedTimeToTime;
            }
            if (nextTimeToMana - Time.time > 0)
            {
                manaGainText.text = (Mathf.RoundToInt(nextTimeToMana - Time.time)).ToString();
            }
            else if (Time.time >= nextTimeToMana)
            {
                manaGainText.text = "Ready";

            }
            if (mana > ultMagicBulleManaCost)
            {
                if (Input.GetKeyDown(KeyCode.X) && Time.time >= nextTimeToUlt)
                {
                    audioSource.PlayOneShot(ultAudio);
                    GameObject magic_Bullet = Instantiate(ultMagicBullePrefab);
                    magic_Bullet.transform.position = spawnMagicBulletSpawn.transform.position;
                    magic_Bullet.transform.position = new Vector2(magic_Bullet.transform.position.x, magic_Bullet.transform.position.y + 1f);
                    magicBulletPrefab.transform.right = transform.right;
                    if (transform.rotation == Quaternion.Euler(0, -180, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 90);

                    }
                    nextTimeToUlt = Time.time + (amountAddedTimeToTime - 10);
                    mana -= ultMagicBulleManaCost;

                }
            }

            if (mana > magicBulletManaCost)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z))
                {
                    audioSource.PlayOneShot(magicBulletCastAudioClip);
                    mana -= magicBulletManaCost;
                    GameObject magic_Bullet = Instantiate(magicBulletPrefab);
                    magic_Bullet.transform.position = spawnMagicBulletSpawn.transform.position;
                    magicBulletPrefab.transform.right = transform.right;
                    if (transform.rotation == Quaternion.Euler(0, -180, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                    {
                        magic_Bullet.transform.rotation = Quaternion.Euler(0, 0, 90);

                    }


                }
            }
        

           
        }

     }

    void AddManaEveryFram()
    {
        if(trollAmount <10)
        {
            if (mana < manaSlider.maxValue)
            {
                mana += 0.01f;

            }
        }else
        {
            if (trollAmount >= 9)
            {
                mana += 0.1f;
            }
        }
      

      


    }
    private void TrollText()
    {
        trollAmount = GameObject.FindObjectsOfType<Troll>().Length;

        trollsAmountText.text =  trollAmount.ToString();
       
    }

    
    void PauseMenu()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            canMove = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        

        addedManaAndHealthText.enabled = true;
        if (collision.CompareTag("Heart"))
        {
            
            addedManaAndHealthTextAnim.SetBool("Dissolove", false);
            audioSource.PlayOneShot(pickUpPowerUpsAudioClip);
            Destroy(collision.gameObject);
           
            health += 20;
            if (health > healthBar.maxValue)
            {
                health = healthBar.maxValue;
            }
            addedManaAndHealthText.text = "Healed up!";
            StartCoroutine(DissoloveAddedManaAndHealthTextAnim());
        }
        else if(collision.CompareTag("ManaPowerUp"))
        {
            addedManaAndHealthText.enabled = true;
            audioSource.PlayOneShot(pickUpPowerUpsAudioClip);
            addedManaAndHealthTextAnim.SetBool("Dissolove", false);
            Destroy(collision.gameObject);
            if(manaSlider.maxValue <1000)
            {
                manaSlider.maxValue += 40;
            }else
            {
                manaSlider.maxValue += 20;

            }
            mana += 30;
            addedManaAndHealthText.text = "Mana Storge Increased!";
            StartCoroutine(DissoloveAddedManaAndHealthTextAnim());


        
        }else if(collision.CompareTag("SoulFrageMent"))
        {
            if(darkManaSlider.value + 20 < darkManaSlider.maxValue)
            {
                darkMana += 15;

            }else
            {
                float whatIsLeft = darkManaSlider.maxValue - darkManaSlider.value;
                darkMana += whatIsLeft;
            }
            addedManaAndHealthText.enabled = true;
            audioSource.PlayOneShot(pickUpPowerUpsAudioClip);
            addedManaAndHealthTextAnim.SetBool("Dissolove", false);
            Destroy(collision.gameObject);
            addedManaAndHealthText.text = "Dark Mana Plocked!";
            StartCoroutine(DissoloveAddedManaAndHealthTextAnim());
        }

      
    }
    IEnumerator DissoloveAddedManaAndHealthTextAnim()
    {
        yield return new WaitForSeconds(1.5f);
        addedManaAndHealthTextAnim.SetBool("Dissolove", true);

    }

    IEnumerator ShowReadyText()
    {
        yield return new WaitForSeconds(1.5f);
        addedManaAndHealthText.enabled = true;
        audioSource.PlayOneShot(pickUpPowerUpsAudioClip);
        addedManaAndHealthTextAnim.SetBool("Dissolove", false);
        manaSlider.maxValue += 40;
        mana += 30;
        addedManaAndHealthText.text = "Ult is Ready";
        StartCoroutine(DissoloveAddedManaAndHealthTextAnim());
        yield return new WaitForSeconds(1.5f);
        addedManaAndHealthTextAnim.SetBool("Dissolove", true);
    }
}



