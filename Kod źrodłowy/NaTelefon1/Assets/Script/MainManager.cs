using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    static public Collider2D beginTouch;
    static private Collider2D endTouch;
    static Slide script;
    static public bool slideStarted = false, scrollStarted = false;
    static public int expandedListManager = 0;
    static public bool expandedBlocked = false;

    static public bool movingLocked = false;

    static public Transform MainCamera() {
        return GameObject.Find("Main Camera").transform;
    }

    static public bool CheckEndTouch(Collider2D col)
    {
        if(col == endTouch)
        {
            endTouch = null;
            return true;
        }
        return false;
    }

    static public bool CheckClicked(Collider2D col) ///Is collider was begin and end
    {
        if (col == endTouch)
        {
            endTouch = null;
            //GameObject.Find("TextWynik").GetComponent<UnityEngine.UI.Text>().text += script.moveStarted + " " + script.scrollStarted + ".";
            if (col == beginTouch && !slideStarted && !scrollStarted)
            {
                slideStarted = false;
                scrollStarted = false;
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        script = GameObject.Find("Main Camera").GetComponent<Slide>();
    }

    void Update()
    {
        
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                beginTouch = Physics2D.OverlapPoint(touchPosition);
                slideStarted = false;
                scrollStarted = false;
            }
            if( touch.phase == TouchPhase.Ended)
            {
                endTouch = Physics2D.OverlapPoint(touchPosition);
                
            }
            if (touch.phase == TouchPhase.Moved)
            {
                slideStarted = script.moveStarted;
                scrollStarted = script.scrollStarted;

            }
        }
    }
}
