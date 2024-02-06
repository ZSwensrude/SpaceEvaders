using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{

    public float moveDistance = 2f;
    public float gridUnitLength = 2f;

    void Update()
    {
        Vector3 transVec = new Vector3(0, 0, 0);
        
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a")) {

            transVec[0] = -moveDistance;
        }

        if (Input.GetKeyDown("right") || Input.GetKeyDown("d")) {
            transVec[0] = moveDistance;
        }

        if (Input.GetKeyDown("up") || Input.GetKeyDown("w")) {
            transVec[1] = moveDistance;
        }

        if (Input.GetKeyDown("down") || Input.GetKeyDown("s")) {
            transVec[1] = -moveDistance;
        }

        Vector3 tempVec = transVec + transform.position;

        if (tempVec.x < gridUnitLength*1.5 && tempVec.x > -gridUnitLength*1.5 && tempVec.y < gridUnitLength*1.5 && tempVec.y > -gridUnitLength*1.5) {

            transform.Translate(transVec);
        
        }

    }
}
