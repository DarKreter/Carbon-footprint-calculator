using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGuzik : MonoBehaviour
{
    public int targetMenu;
    Collider2D col;
    Slide cameraScript;
    UnityEngine.UI.Text tekst;
    public Sprite activeSprite;
    public Sprite noActiveSprite;

    private void Start()
    {
        cameraScript = GameObject.Find("Main Camera").GetComponent<Slide>();
        col = GetComponent<Collider2D>();
        tekst = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>();

    }

    private void Update()
    {
        if (cameraScript.activeMenu == targetMenu)
            GetComponent<SpriteRenderer>().sprite = activeSprite;
        else GetComponent<SpriteRenderer>().sprite = noActiveSprite;
    }

}
