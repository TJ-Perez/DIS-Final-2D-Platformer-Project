using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingPopup : MonoBehaviour
{
    public void prompt()
    {
        gameObject.SetActive(true);
    }

    public void unPrompt()
    {
        gameObject.SetActive(false);
    }

}
