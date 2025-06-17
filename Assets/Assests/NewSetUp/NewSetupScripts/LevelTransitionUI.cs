using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class LevelTransitionUI : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {
        var result = LevelManager.Instance.LastLevelResult;
        resultText.text = $"Level {result.levelNumber} Complete \ntime: {result.timeTaken:F2}s\nStars:{result.stars}";
    }

    public void OnNextLevel() => LevelManager.Instance.AdvanceToNextlevel();
    public void OnReplayLevel() => SceneManager.LoadScene(LevelManager.Instance.CurrentSceneName);
    public void OnQuit()
    {
        Application.Quit();
    }
}
