using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyjdzPorada : MonoBehaviour
{
    Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    
    void Update()
    {
        if (MainManager.CheckClicked(col) || Input.GetKey(KeyCode.Escape))
        {
            Destroy(transform.parent.parent.gameObject);
        }    
    }
}
