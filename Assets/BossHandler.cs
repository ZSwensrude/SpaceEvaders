using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossHandler : MonoBehaviour
{

    private Vector3 targetPosition;
    private float smoothTime = 3F;
    private Vector3 velocity = Vector3.zero;
    private int movementTime = 13;


    public void FlyAway()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        targetPosition = new Vector3(0, 90, 110);
        StartCoroutine("Disappear");
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(movementTime);

        gameObject.SetActive(false);
    }

    public void FlyIn()
    {
        transform.position = new Vector3(0, -90, 110);
        targetPosition = new Vector3(0, 0, 110);
        StartCoroutine("StartAnim");
    }

    private IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(movementTime);

        gameObject.GetComponent<Animator>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
