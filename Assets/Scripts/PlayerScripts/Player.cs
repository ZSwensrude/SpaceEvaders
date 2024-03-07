using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverScreen;

    public void OnCollisionEnter(Collision collision) 
    {
        // if not on visual layer (asteroid bits mostly)
        if(collision.gameObject.layer != 7)
        {
            Destroy(collision.gameObject);
            this.transform.localScale = new Vector3(0, 0, 0);
            gameOverScreen.GetComponent<CanvasGroup>().alpha = 1;
            Time.timeScale = 0;
            StartCoroutine(GameOver());
        }
    }

    public IEnumerator GameOver()
    {
        while(!Input.anyKeyDown) 
        {
            yield return null;

        }

        Debug.Log("User pressed any key");
        Time.timeScale = 1;
        gameOverScreen.GetComponent<CanvasGroup>().alpha = 0;
        SceneManager.LoadScene("TitleScreen");

    
    }

}