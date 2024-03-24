using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputScore;
    [SerializeField]
    private TMP_InputField inputName;

    private int highScore;

    public UnityEvent<string, int> submitScoreEvent;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        inputScore.text = highScore.ToString();
    }

    public void SubmitScore ()
    {
        submitScoreEvent.Invoke(inputName.text, highScore);
    }
}
