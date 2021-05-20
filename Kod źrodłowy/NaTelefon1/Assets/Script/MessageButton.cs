using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageButton : MonoBehaviour
{
    public GameObject wiadomosc;
    public float time;
    public Vector2 where;
    GameObject referencja;
    Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        //referencja = Instantiate(wiadomosc, transform.position + (Vector3)where, transform.rotation, transform);
        //Destroy(referencja, time);
    }

    
    void Update()
    {
        if (MainManager.CheckClicked(col) )
        {
            referencja = Instantiate(wiadomosc, transform.position + (Vector3)where, transform.rotation, transform);
            Destroy(referencja, time);
        }    
    }
}

