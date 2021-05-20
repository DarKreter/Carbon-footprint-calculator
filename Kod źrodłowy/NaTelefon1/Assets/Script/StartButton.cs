using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    Collider2D col;
    private void Start()
    {
        col = GetComponent<Collider2D>();
        MainManager.MainCamera().GetComponent<Slide>().canWeSlide = false;
        MainManager.movingLocked = true;
        MainManager.expandedListManager = 1;
        MainManager.expandedBlocked = true;
    }

    void Update()
    {

        if (MainManager.CheckClicked(col))
        {
            MainManager.movingLocked = false;
            MainManager.expandedListManager = 0;
            MainManager.MainCamera().GetComponent<Slide>().canWeSlide = true;
            MainManager.expandedBlocked = false;
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
