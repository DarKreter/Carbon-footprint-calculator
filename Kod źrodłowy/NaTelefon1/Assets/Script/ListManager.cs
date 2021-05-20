using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{
    public GameObject dodaniePojazdu_p;
    GuziorPojazdZapisz stworzonyGuzior;
    public GameObject newElement_p;
    public GameObject rectangle;
    public GameObject canvas;
    public Transform theRest;
    public Transform border;
    public Transform howMuchExpandBorder;
    public Transform originalBorder;
    
    public float odlMiedzyWierszami;
    public float odlOdGory;
    public float odlOdDolu;

    [HideInInspector] public List<Pojazd> pojazdy = new List<Pojazd>();
    Collider2D col;
    GameObject camera;
    [Space]
    public Color normal;
    public Color disabled;

    private void Start()
    {

        col = GetComponent<Collider2D>();
        camera = GameObject.Find("Main Camera");
        //ExpandBorder(2);
        //Pojazd temp = new Pojazd("Sredni",3,"benzyna",0,20f,30f);
        //pojazdy.Add(temp);
        //Pojazd temp2 = new Pojazd("Duzy", 4, "benzyna", 0, 12f, 13f);
        //pojazdy.Add(temp2);
        //DrawList();
    }

    void Update()
    {
        if(pojazdy.Count == 6)
        {
            GetComponent<SpriteRenderer>().color = disabled;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = normal;
        }

        if (MainManager.CheckClicked(col) && pojazdy.Count < 6 )
        {
            OpenAddingMenu();
        }
        
        if(stworzonyGuzior != null )
        {
            
            if (stworzonyGuzior.clicked)
            {
                foreach (Text text in stworzonyGuzior.texts)
                {
                    if (text.text == null)
                        text.text = "0";
                }

                Pojazd temp = new Pojazd(stworzonyGuzior.texts[0].text, stworzonyGuzior.indexes[0], stworzonyGuzior.texts[1].text, stworzonyGuzior.indexes[1], float.Parse(stworzonyGuzior.texts[2].text), float.Parse(stworzonyGuzior.texts[3].text));
                pojazdy.Add(temp);
                Destroy(stworzonyGuzior.transform.parent.parent.gameObject);
                stworzonyGuzior = null;

                DrawList();
                MainManager.movingLocked = false;
            }
            else if (stworzonyGuzior.transform.parent.Find("ExpandedList2").GetComponent<ExpandedListManager>().isOpened || stworzonyGuzior.transform.parent.Find("ExpandedList1").GetComponent<ExpandedListManager>().isOpened)
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

        }

        if (Input.GetKey(KeyCode.Escape) && stworzonyGuzior != null)
        {
            Destroy(stworzonyGuzior.transform.parent.parent.gameObject);
            stworzonyGuzior = null;

            DrawList();
            MainManager.movingLocked = false;
        }
    }

    void OpenAddingMenu()
    {
        MainManager.movingLocked = true;
        stworzonyGuzior = Instantiate(dodaniePojazdu_p, camera.transform).transform.GetChild(0).GetChild(0).GetComponent<GuziorPojazdZapisz>();
        bool[] tempBools = new bool[6];

        for(int i=0; i < tempBools.Length ; ++i)
        {
            tempBools[i] = false;
        }

        foreach(Pojazd poj in pojazdy)
        {
            tempBools[poj.indexes[0]] = true;
        }

        stworzonyGuzior.transform.parent.Find("ExpandedList1").GetComponent<ExpandedListManager>().disableOptions = tempBools;

        for(int i=1; i < tempBools.Length;++i)
            if(tempBools[i-1] == true && tempBools[i] != true )
            {
                ExpandedListManager script = stworzonyGuzior.transform.parent.Find("ExpandedList1").GetComponent<ExpandedListManager>();
                script.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = script.newMenu.GetComponent<ChooseMenuManager>().opcje[i].transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text;
                script.index = i;
            }
        // zabezpieczenie przed zajetymi wszystkimi opcjami jest wczesniej :)
    }

    public void DrawList()
    {
        //Czyszczenie Ekranu
        for(int idx = 0 ; idx < canvas.transform.childCount ; ++idx)
        {
            GameObject temp = canvas.transform.GetChild(idx).gameObject;
            if(temp.name == newElement_p.name + "(Clone)")
            {
                Destroy(temp);
            }
        }
        //zwiekszanie prostokatu
        Transform rect = rectangle.transform;
        Transform element = newElement_p.transform;
        float halfOfRect = rect.localScale.y / 2;
        float rectY = pojazdy.Count * (odlMiedzyWierszami + element.localScale.y) + odlOdGory;
        rect.localScale = new Vector3(rect.localScale.x, rectY, 1);
        rect.localPosition -= new Vector3(0, rectY / 2 - halfOfRect, 0)  ;

        // dodawanie elementow
        for (int i = 0; i < pojazdy.Count; ++i)
        {
            Transform obj = Instantiate(newElement_p, canvas.transform).transform;

            float elementY = rect.localPosition.y + rect.localScale.y / 2 - obj.localScale.y / 2 - (odlMiedzyWierszami + obj.localScale.y) * i - odlOdGory;
            obj.localPosition = new Vector3(rect.localPosition.x, elementY, -1);

            obj.GetChild(0).GetChild(0).GetComponent<Text>().text = pojazdy[i].rodzajPojazdu;
            if (pojazdy[i].indexes[0] != 0)
            {
                obj.GetChild(0).GetChild(1).GetComponent<Text>().text = pojazdy[i].rodzajPaliwa;
                obj.GetChild(0).GetChild(2).GetComponent<Text>().text = pojazdy[i].litry100km.ToString() + "l/km" ;
                obj.GetChild(0).GetChild(3).GetComponent<Text>().text = pojazdy[i].kmtydzien.ToString() + "km /\n tydzień" ;
            }
            else
            {
                obj.GetChild(0).GetChild(1).GetComponent<Text>().text = "-";
                obj.GetChild(0).GetChild(2).GetComponent<Text>().text = "-";
                obj.GetChild(0).GetChild(3).GetComponent<Text>().text = "-";
            }

            obj.GetComponent<ListsElementManager>().listManager = this;
            //obj.GetComponent<ListsElementManager>().index = i;
        }

        //ExpandBorder(pojazdy.Count);

        // przesuwanie calej reszty nizej
        float restY = rect.localPosition.y - rect.localScale.y / 2 - odlOdDolu;
        theRest.localPosition = new Vector3(0, restY, 0);

    }

    void ExpandBorder(int n)
    {
        border.localScale = originalBorder.localScale;
        border.localPosition = originalBorder.localPosition;
        for(int i=0; i < n; ++i)
        {
            border.localScale += new Vector3(0, howMuchExpandBorder.localScale.y ,0);
            border.localPosition -= new Vector3(0,(howMuchExpandBorder.localScale.y/2),0);
        }
    }

    //void ReduceBorder()
    //{
    //    border.localScale -= new Vector3(0, howMuchExpandBorder.lossyScale.y, 0);
    //    border.localPosition += new Vector3(0, (howMuchExpandBorder.lossyScale.y / 2), 0);
    //}

    //void DrawList() // Pustkowia cierpienia
    //{
    //    /// Dodawanie Elementow
    //    for (int i = 0; i < pojazdy.Count; ++i)
    //    {
    //        GameObject temp = Instantiate(newElement_p, list.transform);
    //        //float elementY = 0.15f - (newElement_p.transform.localScale.y+0.05f) * i;
    //        //float elementY = -(newElement_p.GetComponent<RectTransform>().up.y + 0.05f) * i;
    //        //float elementY = -(temp.transform.localScale.y + wspolczynnikY + odlMiedzyWierszami) * i - odlOdGory;
    //        float elementY = (temp.transform.localScale.y + odlMiedzyWierszami) * i + odlOdGory;

    //        //temp.transform.localPosition = new Vector3(0, elementY, 0);
    //        temp.transform.position -= new Vector3(0, elementY, 0);
    //    }

    //    /// Ustawianie Canvasu
    //    //1.9
    //    Debug.Log(pojazdy.Count);
    //    Debug.Log(wspolczynnikY);
    //    Debug.Log(odlMiedzyWierszami);
    //    float newY = pojazdy.Count * (wspolczynnikY + odlMiedzyWierszami);// - list.GetComponent<RectTransform>().sizeDelta.y;
    //    list.GetComponent<RectTransform>().sizeDelta = new Vector2(1, newY);

    //    list.transform.position = new Vector3(list.transform.position.x, list.transform.position.y - newY * 0.5f, 0);

    //    ///// Ustawianie Tla
    //    //rectangle.localScale = new Vector3(rectangle.localScale.x,(wspolczynnikY + odlMiedzyWierszami ) * pojazdy.Count, 1);
    //}

    public struct Pojazd
    {
        public string rodzajPojazdu;
        public string rodzajPaliwa;
        public int[] indexes;
        public float litry100km;
        public float kmtydzien;

        public Pojazd(string a,int a2,string b,int b2,float c,float d)
        {
            indexes = new int[2];
            
            rodzajPojazdu = a;
            indexes[0] = a2;
            rodzajPaliwa = b;
            indexes[1] = b2;
            litry100km = c;
            kmtydzien = d;
        }
    }
}
