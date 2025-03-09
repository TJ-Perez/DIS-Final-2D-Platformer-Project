using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{

    public Text Valuetxt;
    public Text Timetxt;

    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("EventSystem").GetComponent<GameController>();
        Valuetxt.text = gameController.points.ToString();
        Timetxt.text = gameController.timePlayed.ToString();
    }

    private void Update()
    {
        Valuetxt.text = gameController.points.ToString();
        Timetxt.text = gameController.timePlayed.ToString();
    }


}
