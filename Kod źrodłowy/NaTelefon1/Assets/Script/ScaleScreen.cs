using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleScreen : MonoBehaviour
{
    public GameObject everything;
    public Camera mainCam;

    void Start()
    {
        ScaleEverything();
    }

    void ScaleEverything()
    {
        float DeviceScreenAspect = (float)Screen.width / (float)Screen.height;
        //Debug.Log(Screen.width + "/" + Screen.height + "=" + DeviceScreenAspect);
        mainCam.aspect = DeviceScreenAspect;

        float camHeight = mainCam.orthographicSize * 2f ;
        float camWidth = (camHeight * DeviceScreenAspect)  ;

        //Debug.Log("camHeight:" + camHeight);
        //Debug.Log("camWidth:" + camWidth);

        float PicHeight = 10.11158f;
        float PicWidth = 5.644f;

        everything.transform.localScale = new Vector3(camWidth / PicWidth, camHeight / PicHeight, 1);

    }
}
