using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    public static int blockTop = 0;
    public static int blockBottom = 0;

    public static bool BlockTop()
    {
        return blockTop > 0;
    }

    public static bool BlockBottom()
    {
        return blockBottom > 0;
    }

    public bool amITop = false;
    [Tooltip("Od 1 do 5 (wiem niecywilizowanie)")] public int index;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(amITop && collision.name == "topMenu")
        {
            blockTop++;
        }
        else if(!amITop && collision.name == "downMenu")
        {
            blockBottom++;
        }
        //if(collision.name.Substring(0,6) == "Border" && int.Parse(collision.name.Substring(6, 1)) == index)
        //{
        //    if (amITop) blockTop = true;
        //    else blockBottom = true;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.name.Substring(0, 6) == "Border" && int.Parse(collision.name.Substring(6, 1)) == index)
        //{
        //    if (amITop) blockTop = false;
        //    else blockBottom = false;
        //}
        if (amITop && collision.name == "topMenu")
        {
            blockTop--;
        }
        else if (!amITop && collision.name == "downMenu")
        {
            blockBottom--;
        }
    }
}
