using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lava : MonoBehaviour
{

    //public Sprite[] lavaList;
    //public Sprite Lava2;
    //public Sprite Lava3;
    //public Sprite rightPrefab;
    public Sprite[] _myOthersSprites;
    private Image[] _images;

    // Start is called before the first frame update
    void Start()
    {
        _images = gameObject.GetComponentsInChildren<Image>();
        StartCoroutine(Count());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Count()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            var spriteNumber = Random.Range(0, _myOthersSprites.Length - 1);
            _images[i].sprite = _myOthersSprites[spriteNumber];
        }
        yield return new WaitForSeconds(2);
        //Application.LoadLevel(0);
    }
}
