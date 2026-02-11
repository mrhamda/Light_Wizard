using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovoment player = GameObject.FindObjectOfType<PlayerMovoment>();
        if(player !=null && player.canMove == true)
        {
            StartCoroutine(PlayAnimation());

        }

    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("LastSeconds", true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        anim.speed = 5;
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
