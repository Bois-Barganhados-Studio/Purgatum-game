using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMoving : MonoBehaviour
{
    public double xMod = 0.0;
    public double yMod = 0.0;
    public double x_start;
    public double y_start;

    public double movingRate;

    // Start is called before the first frame update
    void Start()
    {
        x_start = transform.position.x;
        y_start = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        xMod = (mousePos.x - Screen.width / 2) / Screen.width;
        xMod *= movingRate;

        yMod = (mousePos.y - Screen.height / 2) / Screen.height;
        yMod *= movingRate;


        transform.position = new Vector3((float)(x_start + xMod), (float)(y_start + yMod), 0);

    }
}
