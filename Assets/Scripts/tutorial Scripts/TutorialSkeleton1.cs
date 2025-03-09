using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSkeleton1 : MonoBehaviour
{
    public GameObject prompt;

    // Start is called before the first frame update
    void Awake()
    {
        prompt.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            prompt.SetActive(true);

            StartCoroutine(wait());

        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        prompt.SetActive(false);

    }
}
