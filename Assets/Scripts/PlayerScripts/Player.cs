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
        Destroy(collision.gameObject);
        Destroy(gameObject);
        StartCoroutine(GameOver());
    }

    public IEnumerator GameOver()
    {
        gameOverScreen.GetComponent<CanvasGroup>().alpha = 1;
        Time.timeScale = 0;

        if(Input.anyKey)
        {
            Debug.Log("User pressed any key");
            Time.timeScale = 1;
            gameOverScreen.GetComponent<CanvasGroup>().alpha = 0;
            SceneManager.LoadScene("TitleScreen");
            yield return null;
        }
    
    }

}