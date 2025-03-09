using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rollingPopup : MonoBehaviour
{
    public void rollPrompt()
    {
        gameObject.SetActive(true);
    }

    public void rollunPrompt()
    {
        gameObject.SetActive(false);
    }

}
