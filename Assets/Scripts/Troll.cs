using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : MonoBehaviour
{
    PlayerMovoment player;
    [SerializeField] float speed;
    public float damage;
    public float health;
    AudioSource audioSource;
    [SerializeField] AudioClip hitPlayAudioClip;
    [SerializeField] ParticleSystem bloodEffectPrefab;
    float nextTimeToDamage;
    SpriteRenderer spriteRenderer;
    int number;
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovoment>();
        Spawner spawner = GameObject.FindObjectOfType<Spawner>();
        transform.position = spawner.positions[spawner.number];
        audioSource = GetComponent<AudioSource>();
        nextTimeToDamage = Time.time + 1f;
        player.canDamage = true;
        if (Time.timeSinceLevelLoad >= 60 && Time.timeSinceLevelLoad <= 120)
        {
            health = 100;
        }else if(Time.timeSinceLevelLoad > 120)
        {
            float _health = Random.Range(100, 200);
            health = _health;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        number = Random.Range(0, sprites.Length );
        spriteRenderer.sprite = sprites[number];
    }

    // Update is called once per frame
    void Update()
    {


        if (player.canMove == true)
        {
            FollowPlayer();


            CannotBeHitten();

            DestroyIfHealthIsZero();

            Volume();

        }
    }
    
    void Volume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Song");
        
    }
    void CannotBeHitten()
    {
        if (player.canDamage == false && player.tag == "CannotBeHitten")
        {

            nextTimeToDamage = Time.time + 1.5f;
            player.canDamage = true;
            Invoke("Delaycandamage", 1.5f);
        }
    }
    void DestroyIfHealthIsZero()
    {
        if(health<=0)
        {
            if (player.tag == "CannotBeHitten")
            {
                player.tag = "Untagged";
                player.anim.SetBool("GotHit", false);
            }
            if (player.manaSlider.value +10 > player.manaSlider.maxValue)
            {
                float x = player.manaSlider.maxValue - player.mana;
                player.mana += x;
            }else
            {
                player.mana += 10;

            }

           
            Destroy(gameObject);
        }
    }
    void FollowPlayer()
    {
       
            if (player != null)
            {
                float playerPositonX = player.transform.position.x;
                float currentPositonX = transform.position.x;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                if (playerPositonX > currentPositonX)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (playerPositonX < currentPositonX)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        
       
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.GetComponent<PlayerMovoment>() && player.tag == "Untagged")
        {
            
            if (Time.time >= nextTimeToDamage)
            {
                player.anim.SetBool("GotHit", true);
                audioSource.PlayOneShot(hitPlayAudioClip);
                ParticleSystem bloodEffect = Instantiate(bloodEffectPrefab);
                if(player.isDarkMood == true)
                {
                    ParticleSystem.MainModule settings = bloodEffect.main;
                    settings.startColor = new ParticleSystem.MinMaxGradient(Color.black);
                }
                bloodEffect.transform.position = collision.transform.position;
                player.canDamage = false;
                player.tag = "CannotBeHitten";
                if (player.isDarkMood == false)
                {

                    player.health -= damage;

                }
                else
                {

                    player.health -= damage + 20;
                }
              

            }
        }
        else if(collision.GetComponent<MagicBullet>())
        {
          if(collision.GetComponent<MagicBullet>().GetComponent<SpriteRenderer>().color == new Color(0,0,0))
            {
                health -= player.magicBulletDamage + 25;

            }
            else if(player.isDarkMood == false)
            {
                health -= player.magicBulletDamage;
               
            }
          

            
        }
        else if(collision.GetComponent<UltMagicBullet>())
        {
            health = 0;
        }
    }

    void Delaycandamage()
    {
        

            if (player != null)
            {
                player.tag = "Untagged";
                player.anim.SetBool("GotHit", false);

            }
        
      
        
      

    }

}
