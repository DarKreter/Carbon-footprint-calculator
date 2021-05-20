using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleManager : MonoBehaviour
{

    public GameObject[] opcje;
    int activedIndex;

    void Update()
    {
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if(touch.phase == TouchPhase.Ended)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                for(int i = 0 ; i < opcje.Length ; i++)
                {
                    if (touchedCollider == opcje[i].GetComponent<Collider2D>())
                    {
                        opcje[i].GetComponent<SpriteRenderer>().color = new Color(250f / 255f, 235f / 255f, 171f / 255f);
                        activedIndex = i;
                        break;
                    }
                }

                foreach(GameObject opcja in opcje)
                {
                    if(opcje[activedIndex] != opcja)
                    {
                        opcja.GetComponent<SpriteRenderer>().color = new Color(159f / 255f, 129f / 255f, 2f / 255f);
                    }
                }
            }
        }
    }
}
