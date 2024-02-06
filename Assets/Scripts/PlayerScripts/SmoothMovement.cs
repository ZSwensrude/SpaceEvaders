using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    public float moveDistance = 2f;
    public float gridUnitLength = 2f;
    public float moveSpeed = 25f;

    [SerializeField]
    private bool showDebug = false;

    Vector3 transVec = new Vector3(0, 0, 0);

    int horizontal = 0;
    int vertical = 0;

    void Update()
    {

        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
        {
            if (horizontal > -1)
            {
                transVec[0] -= moveDistance;
                horizontal--;
            }

        }

        if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
        {
            if (horizontal < 1)
            {
                transVec[0] += moveDistance;
                horizontal++;
            }
        }

        if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
        {
            if (vertical < 1)
            {
                transVec[1] += moveDistance;
                vertical++;
            }
        }

        if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
        {
            if (vertical > -1)
            {
                transVec[1] -= moveDistance;
                vertical--;
            }
        }

        if (showDebug) Debug.Log("vertical: " + vertical + "horizontal: " + horizontal + "transVec: " + transVec);

        float step = Time.deltaTime * moveSpeed;

        transform.position = Vector3.MoveTowards(transform.position, transVec, step);


    }
}
