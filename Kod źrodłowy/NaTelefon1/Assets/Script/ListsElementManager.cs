using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListsElementManager : MonoBehaviour
{
    public GameObject editMenu_p;
    Collider2D col;
    GuziorPojazdZapisz stworzonyGuzior;
    [HideInInspector] public ListManager listManager;
    //[HideInInspector] public int myIndex;
    //[HideInInspector] public int index;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        //stworzonyGuzior = Instantiate(editMenu_p, MainManager.MainCamera()).transform.GetChild(0).GetChild(0).GetComponent<GuziorPojazdZapisz>(); ;
    }

    private void Update()
    {
        if (MainManager.CheckClicked(col))
        {
            MainManager.movingLocked = true;
            stworzonyGuzior = Instantiate(editMenu_p, MainManager.MainCamera()).transform.GetChild(0).GetChild(0).GetComponent<GuziorPojazdZapisz>();
            ListManager.Pojazd temp = listManager.pojazdy.Find(x => x.rodzajPojazdu == transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
            stworzonyGuzior.texts[0].transform.parent.GetComponent<ExpandedListManager>().index = temp.indexes[0];
            stworzonyGuzior.texts[1].transform.parent.GetComponent<ExpandedListManager>().index = temp.indexes[1];

            stworzonyGuzior.texts[0].text = transform.GetChild(0).GetChild(0).GetComponent<Text>().text;
            stworzonyGuzior.texts[0].transform.parent.GetComponent<ExpandedListManager>().disabled = true;
            stworzonyGuzior.texts[1].text = transform.GetChild(0).GetChild(1).GetComponent<Text>().text;
            stworzonyGuzior.texts[2].transform.parent.GetComponent<InputField>().text = transform.GetChild(0).GetChild(2).GetComponent<Text>().text.Remove(transform.GetChild(0).GetChild(2).GetComponent<Text>().text.Length - 4,4);
            stworzonyGuzior.texts[3].transform.parent.GetComponent<InputField>().text = transform.GetChild(0).GetChild(3).GetComponent<Text>().text.Remove(transform.GetChild(0).GetChild(3).GetComponent<Text>().text.Length - 13, 13);
            
        }

        if(stworzonyGuzior != null)
        {
            if (stworzonyGuzior.transform.parent.Find("ExpandedList2").GetComponent<ExpandedListManager>().isOpened)
            { 
                //stworzonyGuzior.transform.GetChild(0).GetComponent<Text>().text =
                //stworzonyGuzior.transform.parent.Find("ExpandedList2").GetComponent<ExpandedListManager>().isOpened.ToString();
                //stworzonyGuzior.transform.GetChild(0).GetComponent<Text>().text = stworzonyGuzior.transform.parent.Find("ExpandedList2").GetComponent<ExpandedListManager>().isOpened.ToString();
                stworzonyGuzior.transform.parent.Find("InputField1").GetComponent<InputFieldManager>().disable = true;
                stworzonyGuzior.transform.parent.Find("InputField2").GetComponent<InputFieldManager>().disable = true;

            }
            else
            {
                //stworzonyGuzior.transform.GetChild(0).GetComponent<Text>().text =
                //stworzonyGuzior.transform.parent.Find("ExpandedList1").GetComponent<ExpandedListManager>().isOpened.ToString();
                //stworzonyGuzior.transform.GetChild(0).GetComponent<Text>().text = stworzonyGuzior.transform.parent.Find("ExpandedList2").GetComponent<ExpandedListManager>().isOpened.ToString();
                stworzonyGuzior.transform.parent.Find("InputField1").GetComponent<InputFieldManager>().disable = false;
                stworzonyGuzior.transform.parent.Find("InputField2").GetComponent<InputFieldManager>().disable = false;
            }

            if (stworzonyGuzior.clicked || Input.GetKey(KeyCode.Escape))
            {
                transform.GetChild(0).GetChild(0).GetComponent<Text>().text = stworzonyGuzior.texts[0].text;
                transform.GetChild(0).GetChild(1).GetComponent<Text>().text = stworzonyGuzior.texts[1].text;
                transform.GetChild(0).GetChild(2).GetComponent<Text>().text = stworzonyGuzior.texts[2].text + "l/km";
                transform.GetChild(0).GetChild(3).GetComponent<Text>().text = stworzonyGuzior.texts[3].text + "km /\n tydzień";

                int idx = listManager.pojazdy.FindIndex(x => x.rodzajPojazdu == transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                ListManager.Pojazd temp = new ListManager.Pojazd(stworzonyGuzior.texts[0].text, stworzonyGuzior.indexes[0],
                    stworzonyGuzior.texts[1].text, stworzonyGuzior.indexes[1], float.Parse(stworzonyGuzior.texts[2].text),
                    float.Parse(stworzonyGuzior.texts[3].text
                ));

                listManager.pojazdy[idx] = temp;
                //listManager.pojazdy[idx].indexes[0] = stworzonyGuzior.indexes[0];
                //listManager.pojazdy[idx].indexes[1] = stworzonyGuzior.indexes[1];
                //listManager.pojazdy[idx].rodzajPaliwa = stworzonyGuzior.texts[1].text;
                //listManager.pojazdy[idx].litry100km = float.Parse(stworzonyGuzior.texts[2].text);
                //listManager.pojazdy[idx].kmtydzien = float.Parse(stworzonyGuzior.texts[3].text);

                MainManager.movingLocked = false;

                Destroy(stworzonyGuzior.transform.parent.parent.gameObject);
            }

            if (stworzonyGuzior.transform.parent.GetChild(1).GetComponent<GuziorPojazdUsun>().clicked)
            {
                listManager.pojazdy.RemoveAt(listManager.pojazdy.FindIndex(x => x.rodzajPojazdu == transform.GetChild(0).GetChild(0).GetComponent<Text>().text));
                listManager.DrawList();
                MainManager.movingLocked = false;
                Destroy(stworzonyGuzior.transform.parent.parent.gameObject);
            }

        }

        
    }
}
