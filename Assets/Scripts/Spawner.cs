using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(AudioSource))]
public class Spawner : MonoBehaviour
{
    [SerializeField] Troll trollPrefab;
    public Vector2[] positions;
    float secondsBetweenSpawn;
    float nextTimeToSpawn;
    [SerializeField] Vector2 minMax;
    public int number;
    AudioSource audioSource;
    [SerializeField] AudioClip respawnAudioClip;
    [SerializeField] GameObject[] powerUpPrefabs;
    PlayerMovoment player;
    int highScore;
    [SerializeField] TextMeshProUGUI scoreTextInsideGame;
    public float secondsBetween_Spawn;
    float nextTimeToSpawnPowerUps;
    // Start is called before the first frame update
    void Start()
    {
        nextTimeToSpawn = Time.time;
        audioSource = GetComponent<AudioSource>();
         player = GameObject.FindObjectOfType<PlayerMovoment>();
        highScore = PlayerPrefs.GetInt("HighScore");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Song");
        int health = GameObject.FindGameObjectsWithTag("Heart").Length;
        int mana = GameObject.FindGameObjectsWithTag("ManaPowerUp").Length;
        int powerups = health + mana;
        if (player.canMove == true&& player !=null )
        {
           

            Spawn();
            secondsBetween_Spawn = secondsBetweenSpawn;
           
        }
        Score();

    }

    void Spawn()
    {
        if(Time.time >= nextTimeToSpawnPowerUps)
        {
            if(secondsBetweenSpawn >0.5f)
            {
                int number0 = Random.Range(0, powerUpPrefabs.Length);
                GameObject power_Up = Instantiate(powerUpPrefabs[number0]);
                power_Up.transform.position = new Vector2(Random.Range(-13, 13), -7);
                nextTimeToSpawnPowerUps = nextTimeToSpawn + 5f;
            }
           
        }
       

        PlayerMovoment player = GameObject.FindObjectOfType<PlayerMovoment>();
        int trollsAmount = GameObject.FindObjectsOfType<Troll>().Length;
        if (Time.time >= nextTimeToSpawn && player != null && trollsAmount <= 10)
        {
            audioSource.PlayOneShot(respawnAudioClip);
            secondsBetweenSpawn = Mathf.Lerp(minMax.y, minMax.x, Diffculity.GetDiffculityPercent());
            Troll troll_prefab = Instantiate(trollPrefab);
            number = Random.Range(0, 2);
            nextTimeToSpawnPowerUps = nextTimeToSpawn + 2f;
            nextTimeToSpawn = Time.time + secondsBetweenSpawn;
           
        }
    }

    

   void Score()
    {
        scoreTextInsideGame.text = "Kills:" + player.score;

        if (player.score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", player.score);
            player.scoreText.text = "New highScore! Kills:" + player.score.ToString();

        }
        else if (player.score < PlayerPrefs.GetInt("HighScore"))
        {
            player.scoreText.text = "Kills:" + player.score.ToString();
        }
       
    }
}
