using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    //public static GameControler Instance { get; private set; }

    public Victory VictoryScreen;
    public GameOver GameOverScreen;

    public GameObject player;
    public Player PlayerScript;
    public GameObject VictoryTrigger;
    private VictoryTrig VictoryTriggerScript;

    public float timePlayed;

    public int points;

    /*
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    */

    private void GameOver()
    {
        GameOverScreen.setup();
    }

    private void Victory()
    {
        VictoryScreen.setup();
    }

    private void Start()
    {
        VictoryTriggerScript = VictoryTrigger.GetComponent<VictoryTrig>();

        player = GameObject.FindWithTag("Player");
        PlayerScript = player.GetComponent<Player>();
    }

    public void addPoint()
    {
        points += 1;
    }

    private void Update()
    {
        if(VictoryTriggerScript.VictoryTriggered == true)
        {
            Victory();
        }

        if (PlayerScript.lives > 0)
        {
           timePlayed += Time.deltaTime;
        }
        
        if (PlayerScript.lives <= 0) 
        {
            GameOver();
        }
    }

    

}