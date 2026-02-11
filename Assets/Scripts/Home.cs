using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Home : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        highScore.text = "HighScore kills:" +PlayerPrefs.GetInt("HighScore").ToString();
    }
}
