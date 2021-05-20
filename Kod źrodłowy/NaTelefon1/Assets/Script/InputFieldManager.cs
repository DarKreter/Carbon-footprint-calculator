using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class InputFieldManager : MonoBehaviour
{
    Collider2D col;
    InputField inputField;
    Text yourText;
    Slide cameraScript;
    public bool disable = false; 
    public bool blockZero = false;
    //public bool disableWithSlide = true;
    public int minELM = 0;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        inputField = GetComponent<InputField>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<Slide>();
    }

    private void Update()
    {
//        MainManager.movingLocked = true;
        if(transform.childCount >= 1)
            yourText = transform.Find("Text").GetComponent<Text>();
        if(inputField != null)
        {
            //GameObject.Find("Text1AAA").GetComponent<Text>().text =
            //    MainManager.slideStarted + " " + MainManager.scrollStarted + " " + (disableWithSlide && !cameraScript.canWeSlide) + " " + disable;
            if (MainManager.slideStarted || MainManager.scrollStarted || MainManager.expandedListManager > minELM || disable)
                inputField.interactable = false;
            else
                inputField.interactable = true;
        }

        //if(transform.childCount >= 1)
        //    Debug.Log(transform.Find("Text").GetComponent<Text>().text);


        if (transform.childCount >= 1 && yourText.text == "" && !inputField.isFocused)
        { /// nie jestem pewien czy potrzeba tyle warunkow
            if (blockZero)
                inputField.text = "1";
            else inputField.text = "0";

        }

        if (blockZero && yourText.text == "0" && !inputField.isFocused)
        {
            inputField.text = "1";
        }

        while (transform.childCount >= 1 && !inputField.isFocused && transform.childCount >= 1 && yourText.text.Substring(0, 1) == "0" && yourText.text.Length > 1)
        {
            inputField.text = yourText.text.Remove(0, 1);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended)
            {
                inputField.interactable = true;
            }
            
        }
    }
}
