using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkParticles : MonoBehaviour
{
    ParticleSystem bloodEffect;
    PlayerMovoment player;
    // Start is called before the first frame update
    void Start()
    {
         bloodEffect = GetComponent<ParticleSystem>();
        player = GameObject.FindObjectOfType<PlayerMovoment>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isDarkMood == true)
        {
            ParticleSystem.MainModule settings = bloodEffect.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Color.black);
        }
       
    }
}
