using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandedListManager : MonoBehaviour
{

    public GameObject newMenu;
    public bool disabled = false;
    [HideInInspector] public bool isOpened = false;
    Collider2D col;
    [HideInInspector] public UnityEngine.UI.Text textBox;
    Slide cameraScript;


    public bool[] disableOptions = null;
    [Tooltip("tylko dla tego elementu")] public Color disableColor = Color.red;
    Color normalColor;

    [HideInInspector] public int index = 0;

    private void Start()
    {
        //disableOptions = new bool[newMenu.GetComponent<ChooseMenuManager>().opcje.Length];
        //for (int i = 0; i < disableOptions.Length; ++i)
        //{
        //    disableOptions[i] = false;
        //}

        col = GetComponent<Collider2D>();
        textBox = transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<Slide>();
        normalColor = GetComponent<SpriteRenderer>().color;
        //Instantiate(newMenu, cameraScript.transform).GetComponent<ChooseMenuManager>().dad = this;
        //cameraScript.canWeSlide = false;
    }

    void Update()
    {
        GetComponent<SpriteRenderer>().color = disabled ? disableColor : normalColor;
        
        if( !disabled && MainManager.CheckClicked(col) && !MainManager.expandedBlocked )
        {
            //GameObject.Find("TextXYZ").GetComponent<UnityEngine.UI.Text>().text ="(";
            ChooseMenuManager temp = Instantiate(newMenu,cameraScript.transform).GetComponent<ChooseMenuManager>();
            temp.dad = this;
            cameraScript.canWeSlide = false;
            MainManager.movingLocked = true;
            MainManager.expandedListManager++;
            isOpened = true;
            temp.SetNewDisables(disableOptions); /// TU WYWALA PROGRAM, NIE PISZ LINIJEK KODU POD TYM
           
            
        }
  
    }

    public void CurrentOption(string newOption)
    {
        textBox.text = newOption;
    }

    public string CurrentOption()
    {
        return textBox.text;
    }
}
