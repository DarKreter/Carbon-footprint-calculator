using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuziorPojazdUsun : MonoBehaviour
{
    Collider2D col;
    [HideInInspector] public bool clicked = false;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (MainManager.CheckClicked(col))
        {
            clicked = true;
        }
    }
}
