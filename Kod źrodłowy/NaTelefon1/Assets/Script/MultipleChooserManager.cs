using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleChooserManager : MonoBehaviour
{

    public Collider2D[] elements;
    [HideInInspector] public bool[] indexes;

    public Sprite clicked;
    public Sprite clickedNot;

    public Color fontColor;
    public Color notFontColor;

    private void Start()
    {
        indexes = new bool[elements.Length];
        for(int i=0; i < indexes.Length ;++i)
        {
            indexes[i] = false;
        }
        
    }

    void Update()
    {
        for(int i=0; i < elements.Length; ++i)
        {
            if( MainManager.CheckClicked(elements[i]) )
            {
                //GameObject.Find("TextAAA").GetComponent<UnityEngine.UI.Text>().text = i.ToString();
                indexes[i] = !indexes[i];
                elements[i].GetComponent<SpriteRenderer>().sprite = indexes[i] ? clicked : clickedNot;
                elements[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = indexes[i] ? fontColor : notFontColor;
            }
        }
    }
}
