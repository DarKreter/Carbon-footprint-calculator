using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMenuManager : MonoBehaviour
{
    public GameObject[] opcje;
    [HideInInspector] public ExpandedListManager dad;
    Slide cameraScript;

    [Space] [Header("Ustawienia Wylaczonych opcji")]
    public bool[] disabledOptions = new bool[0];
    public Color disabledColor = Color.red;
    bool[] trueDisabledOptions;

    private void Start()
    {
        cameraScript = GameObject.Find("Main Camera").GetComponent<Slide>();
        if(trueDisabledOptions == null)
        {
            trueDisabledOptions = new bool[opcje.Length];
            for (int i=0; i < trueDisabledOptions.Length;++i)
            {
                trueDisabledOptions[i] = false;
            }
            //GameObject.Find("TextXYZ").GetComponent<UnityEngine.UI.Text>().text += "A";
        }
    }

    public void SetNewDisables(bool[] array)
    {
        if (array == null) return;

        if(array.Length != opcje.Length)
        {
            cameraScript.GetComponent<Camera>().backgroundColor = Color.yellow;
            cameraScript.transform.position = new Vector3(0, -69, 0);
        }

        trueDisabledOptions = new bool[opcje.Length];
        trueDisabledOptions = array;
        //GameObject.Find("TextXYZ").GetComponent<UnityEngine.UI.Text>().text =
        //    trueDisabledOptions[0].ToString() + trueDisabledOptions[1].ToString() + trueDisabledOptions[2].ToString() +
        //    trueDisabledOptions[3].ToString() + trueDisabledOptions[4].ToString() + trueDisabledOptions[5].ToString();
        //GameObject.Find("TextXYZ").GetComponent<UnityEngine.UI.Text>().text += "C";

        for (int i = 0; i < trueDisabledOptions.Length; ++i)
        {
            if (trueDisabledOptions[i])
            {
                opcje[i].GetComponent<SpriteRenderer>().color = disabledColor;
            }
        }

    }

    void Update()
    {
        dad.isOpened = true;

        for (int i = 0; i < opcje.Length; i++)
        {
            if (!trueDisabledOptions[i] &&  MainManager.CheckClicked(opcje[i].GetComponent<Collider2D>()) )
            {
                //GameObject.Find("TextZYX").GetComponent<UnityEngine.UI.Text>().text = trueDisabledOptions[i].ToString();
                dad.CurrentOption( opcje[i].GetComponent<ChooseMenuOption>().Text() );
                dad.index = i;
                cameraScript.canWeSlide = true;
                MainManager.movingLocked = false;
                MainManager.expandedListManager--;
                dad.isOpened = false;
                Destroy(gameObject);
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            cameraScript.canWeSlide = true;
            MainManager.movingLocked = false;
            MainManager.expandedListManager--;
            dad.isOpened = false;
            Destroy(gameObject);
        }

    }
}
