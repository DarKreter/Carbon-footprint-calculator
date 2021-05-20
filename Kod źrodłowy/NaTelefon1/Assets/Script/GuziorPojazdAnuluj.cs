using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuziorPojazdAnuluj : MonoBehaviour
{
    Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        
        if ( MainManager.CheckClicked(col) )
        {
            MainManager.movingLocked = false;
            Destroy(transform.parent.parent.gameObject);
        }
            
    }
}
