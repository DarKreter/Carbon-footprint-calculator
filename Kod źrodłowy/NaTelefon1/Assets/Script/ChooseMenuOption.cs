using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMenuOption : MonoBehaviour
{
    
    public string Text()
    {
        return transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text;
    }
}
