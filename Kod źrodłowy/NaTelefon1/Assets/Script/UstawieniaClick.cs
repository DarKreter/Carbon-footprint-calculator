using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UstawieniaClick : MonoBehaviour
{
    Collider2D col;
    GameObject camera;
    public GameObject okienkoUstawienia;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        camera = GameObject.Find("Main Camera");
        //Instantiate(okienkoUstawienia, camera.transform);
    }

    private void Update()
    {
        
        if (MainManager.CheckClicked(col) )
        {
            MainManager.expandedBlocked = true;
            Instantiate(okienkoUstawienia, camera.transform);
        }
    }
}
