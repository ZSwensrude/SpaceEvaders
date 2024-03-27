using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void ToCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
