using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    [SerializeField] float speed;
    PlayerMovoment player;
    [SerializeField] float amountOfTimeBeforeDestroy;
    [SerializeField] GameObject soulFrageMent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovoment>();
        StartCoroutine(DestroyAfterCertainTime());
    }

    // Update is called once per frame
    void Update()
    {
        if(player !=null && player.canMove == true)
        {
            MoveForward();
        }
       
    }

    void MoveForward()
    {
        transform.Translate(speed * transform.right * Time.deltaTime);
    }
   
    
    IEnumerator DestroyAfterCertainTime()
    {
        yield return new WaitForSeconds(amountOfTimeBeforeDestroy);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.GetComponent<Troll>())
        {

            if (collision.GetComponent<Troll>().health < player.magicBulletDamage && player.isDarkMood == false )
            {
                player.score++;
                GameObject _soulFrageMent = Instantiate(soulFrageMent);
                _soulFrageMent.transform.position = collision.transform.position;
                Destroy(gameObject);

            }
            else if (collision.GetComponent<Troll>().health < player.magicBulletDamage + 25 && player.isDarkMood == true)
            {
                player.score++;
                int number = Random.Range(0, 2);
                if(number == 0)
                {
                    GameObject _soulFrageMent = Instantiate(soulFrageMent);
                    _soulFrageMent.transform.position = collision.transform.position;
                }
                print(number);
            }
            Destroy(gameObject);

        }
    }
   
}
