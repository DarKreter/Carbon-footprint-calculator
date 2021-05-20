using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackUstawienia : MonoBehaviour
{
    Collider2D col;
    GameObject camera;
    public GameObject okienkoUstawienia;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        camera = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        
        if (MainManager.CheckClicked(col) || Input.GetKey(KeyCode.Escape))
        {
            MainManager.expandedBlocked = false;
            GameObject.Destroy(transform.parent.parent.gameObject);
        }
            
    }
}
