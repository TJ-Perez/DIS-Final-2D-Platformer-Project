using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{

    public Text Valuetxt;
    public Text Timetxt;

    [SerializeField] private GameControler gameControler;

    // Start is called before the first frame update
    void Start()
    {
        Valuetxt.text = gameControler.points.ToString();
        Timetxt.text = gameControler.timePlayed.ToString();
    }

    private void Update()
    {
        Valuetxt.text = gameControler.points.ToString();
        Timetxt.text = gameControler.timePlayed.ToString();
    }


}
