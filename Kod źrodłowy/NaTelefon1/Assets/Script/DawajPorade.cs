using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DawajPorade : MonoBehaviour
{
    Collider2D col;
    Transform cam;
    public GameObject porada;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        cam = GameObject.Find("Main Camera").transform;
    }

    
    void Update()
    {
        if (MainManager.CheckClicked(col))
        {
            Instantiate(porada, cam);
        }
    }
}
