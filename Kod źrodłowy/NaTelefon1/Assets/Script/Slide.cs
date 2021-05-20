using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    Transform me;
    [HideInInspector] public bool canWeSlide = true;
    bool scrollAllowed = true;
    [HideInInspector] public bool moveStarted = false, scrollStarted = false;
    [HideInInspector] public int activeMenu = 1;
    Vector2 startPosition;
    Vector2 startedPosition;
    Vector2 lastPosition;
    Vector2 lastedPosition;
    Vector2 trueStartPosition;
    public Collider2D[] guziory = new Collider2D[5];
    Camera cam;
    float height;

    public GameObject[] menus = new GameObject[5];
    Transform[] borders = new Transform[5];

    private void Start()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        height = edgeVector.y * 2;

        cam = GetComponent<Camera>();
        //Debug.Log(height = edgeVector.y * 2);

        me = GetComponent<Transform>();

        //for (int i = 0; i < 5; i++)
        //{
        //    //menus[i] = GameObject.Find("menu" + (i + 1).ToString());
        //    //guziory[i] = GameObject.Find("Guzik" + (i + 1).ToString()).GetComponent<Collider2D>();
        //    borders[i] = GameObject.Find("Border" + (i + 1).ToString()).transform;

        //}
    }

    private void Update()
    {
        //Debug.Log(BoundsManager.blockTop);
        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touchPosition;
                lastPosition = touchPosition;

                startedPosition = touch.position;
                lastedPosition = touch.position;
                trueStartPosition = touch.position;
            }
            //me.position = new Vector3(menus[activeMenu - 1].transform.position.x + (startPosition.x - touchPosition.x), me.position.y, me.position.z);
            else if (touch.phase == TouchPhase.Moved && canWeSlide && !MainManager.movingLocked)
            {

                //GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text
                //    = "(" + (menus[0].transform.position.x).ToString() + ") " +
                //    "(" + (menus[1].transform.position.x).ToString() + ")";

                if (!scrollStarted && Mathf.Abs(startedPosition.x - touch.position.x) > (Screen.width / 7))
                    moveStarted = true;

                if (!moveStarted && Mathf.Abs(startedPosition.y - touch.position.y) > (Screen.height / 30))
                    scrollStarted = true;

                if ((moveStarted && lastedPosition.x - touch.position.x > (Screen.width / 100) && (activeMenu != 5 || startedPosition.x - touch.position.x < 0))
                     || (moveStarted && touch.position.x - lastedPosition.x > (Screen.width / 100) && (activeMenu != 1 || touch.position.x - startedPosition.x < 0))
                     && canWeSlide)
                {

                    lastedPosition = new Vector2(touch.position.x, lastedPosition.y);

                    me.position = new Vector3(menus[activeMenu - 1].transform.position.x + (startedPosition.x - lastedPosition.x) * 0.05f / (Screen.width / 100), me.position.y, me.position.z);
                    scrollAllowed = false;
                }
                else if (scrollStarted)
                {
                    //if (startPosition.y > touchPosition.y && height / 2f + transform.position.y >= borders[0].position.y + borders[0].lossyScale.y / 2)
                    //{

                    //}
                    //if ((startPosition.y > touch.position.y && height/2f+ transform.position.y <= borders[0].position.y + borders[0].lossyScale.y/2 ) || startPosition.y < touch.position.y
                     //*|| (startPosition.y > touch.position.y && transform.position.y - height / 2f  >= borders[0].position.y - borders[0].lossyScale.y / 2)*/)
                    if(!(( BoundsManager.BlockTop() && startPosition.y > touchPosition.y ) || (BoundsManager.BlockBottom() && startPosition.y < touchPosition.y)))
                    {
                        Transform activedMenu = menus[activeMenu - 1].transform;
                        float newY = activedMenu.position.y - (lastPosition.y - touchPosition.y);
                        activedMenu.position = new Vector3(activedMenu.position.x, newY, activedMenu.position.z);

                        lastPosition = touchPosition;
                    }
                    
                }
            }
            else if (touch.phase == TouchPhase.Ended && canWeSlide && !MainManager.movingLocked)
            {
                //GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text
                //    = "[" + (startedPosition.x - touch.position.x).ToString() + "] " +
                //    "[" + (touch.position.x).ToString() + "]";

                if (startedPosition.x - touch.position.x > (Screen.width * 0.25f) && activeMenu != 5 && moveStarted)
                {
                    //GameObject.Find("Guzik1").GetComponent<SpriteRenderer>().color = Color.magenta;
                    SlideTo(activeMenu + 1);
                }
                else if (touch.position.x - startedPosition.x > (Screen.width * 0.25f) && activeMenu != 1 && moveStarted)
                {
                    SlideTo(activeMenu - 1);
                }
                else
                {
                    bool check = true;

                    if (!moveStarted && !scrollStarted)
                    {
                        Vector2 touchPosition2 = Camera.main.ScreenToWorldPoint(touch.position);
                        for (int i = 0; i < 5; i++)
                        {
                            if (Physics2D.OverlapPoint(touchPosition2) == guziory[i])
                            {
                                SlideTo(i + 1);
                                check = false;
                                break;
                            }
                        }
                    }


                    if (check == true)
                        SlideTo(activeMenu);
                }

                moveStarted = false;
                scrollStarted = false;

                scrollAllowed = true;
            }
        }
    }

    public void SlideTo(int targetPosition)
    {
        if (targetPosition != activeMenu)
            canWeSlide = false;
        StartCoroutine(ActualSliding(targetPosition));
    }

    IEnumerator ActualSliding(int targetPosition)
    {

        activeMenu = targetPosition;
        float i = 0;
        Vector3 start = me.position;
        Vector3 end = new Vector3(menus[targetPosition - 1].transform.position.x, 0, me.position.z); /// (targetPosition - 1) * 5.644f
        //end += new Vector3(0, 0, -12);

        while (i <= 1)
        {
            me.position = Vector3.Lerp(start, end, i);
            i += 0.05f;
            yield return new WaitForFixedUpdate();
        }

        me.position = new Vector3(menus[targetPosition - 1].transform.position.x, me.position.y, me.position.z);

        canWeSlide = true;
    }
}
