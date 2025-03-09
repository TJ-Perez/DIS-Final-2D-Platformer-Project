using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public GameObject player;
    public Player PlayerScript;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        PlayerScript = player.GetComponent<Player>();
    }

    public void setup()
    {
        gameObject.SetActive(true);

    }

    public void restartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AdvanceButton()
    {

        SceneManager.LoadScene("Dungeon");
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SurvivalButton()
    {
        SceneManager.LoadScene("survival");
    }
}
