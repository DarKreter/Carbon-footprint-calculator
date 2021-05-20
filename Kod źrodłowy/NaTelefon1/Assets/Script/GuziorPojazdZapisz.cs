using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuziorPojazdZapisz : MonoBehaviour
{
    Collider2D col;
    public UnityEngine.UI.Text[] texts = new UnityEngine.UI.Text[4];
    public int[] indexes = new int[2];
    [HideInInspector] public bool clicked = false;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        
        for(int i=0; i < 2; i++)
            indexes[i] = texts[i].transform.parent.GetComponent<ExpandedListManager>().index;

        if (MainManager.CheckClicked(col))
        { 
            clicked = true;
        }

        bool active = !(indexes[0] == 0);
        transform.parent.Find("Text2").gameObject.SetActive(active);
        transform.parent.Find("ExpandedList2").gameObject.SetActive(active);
        transform.parent.Find("Text3").gameObject.SetActive(active);
        transform.parent.Find("InputField1").gameObject.SetActive(active);
        transform.parent.Find("Text4").gameObject.SetActive(active);
        transform.parent.Find("InputField2").gameObject.SetActive(active);


    }
}
