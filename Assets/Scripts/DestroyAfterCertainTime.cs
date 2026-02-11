using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterCertainTime : MonoBehaviour
{
    [SerializeField] float Delay;
    // Start is called before the first frame update
    void Update()
    {
        PlayerMovoment player = GameObject.FindObjectOfType<PlayerMovoment>();
        if(player != null && player.canMove == true)
        {
            StartCoroutine(Destroy());
        }
    }

  
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(Delay);
        Destroy(gameObject);
    }
}


