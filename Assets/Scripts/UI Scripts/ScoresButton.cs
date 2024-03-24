using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoresButton : MonoBehaviour
{
    public void ShowLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }
}
