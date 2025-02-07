using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    protected GameObject cam;

    [SerializeField] private float parallaxEffect_X;
    [SerializeField] private float parallaxEffect_Y;

    private float xPosition;
    private float yPosition;
    private float length;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }


    void Update()
    {
        float distanceToMove_X = cam.transform.position.x * parallaxEffect_X;
        float distanceMoved_X = cam.transform.position.x * (1 - parallaxEffect_X);

        if(distanceMoved_X > xPosition + length)
        {
            xPosition += length;
        }
        else if(distanceMoved_X < xPosition - length)
        {
            xPosition -= length;
        }

        float distanceToMove_Y = cam.transform.position.y * parallaxEffect_Y;

        transform.position = new Vector3(xPosition + distanceToMove_X, yPosition + distanceToMove_Y);
    }
}
