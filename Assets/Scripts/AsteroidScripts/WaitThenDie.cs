using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitThenDie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenDestroy());
    }

    IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(3);

        foreach (Transform asteroidBit in gameObject.GetComponent<Transform>())
        {
            Destroy(asteroidBit.gameObject);
        }
    }

}
