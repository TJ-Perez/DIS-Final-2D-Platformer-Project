using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrig : MonoBehaviour
{
    private GameObject Player;
    private Player playerScript;

    public bool VictoryTriggered;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        playerScript = Player.GetComponent<Player>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("We won!");
        playerScript.lives = 1000;
        VictoryTriggered = true;
    }

    
}
