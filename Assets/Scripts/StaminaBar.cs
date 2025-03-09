using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private GameObject player;
    private Player playerScript;

    public int numberOfrolls;
    public int rolls;

    public Image[] icon;
    public Sprite fullIcon;
    public Sprite emptyIcon;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }


    private void Update()
    {

        rolls = playerScript.stamina;

        if (rolls > numberOfrolls)
        {
            rolls = numberOfrolls;
        }

        for (int i = 0; i < icon.Length; i++)
        {
            if (i < rolls)
            {
                icon[i].sprite = fullIcon;
            }
            else
            {
                icon[i].sprite = emptyIcon;
            }

            if (i < numberOfrolls)
            {
                icon[i].enabled = true;
            }
            else
            {
                icon[i].enabled = false;
            }
        }



    }
}
