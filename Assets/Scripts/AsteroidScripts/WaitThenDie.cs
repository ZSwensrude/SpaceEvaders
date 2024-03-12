using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitThenDie : MonoBehaviour
{
    [SerializeField]
    private AudioSource breakSound;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenDestroy());
    }

    IEnumerator WaitThenDestroy()
    {
        breakSound.Play();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

}
