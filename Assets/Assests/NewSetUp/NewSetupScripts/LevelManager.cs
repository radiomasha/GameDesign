using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<string> levelSceneNames = new List<string> { "Level_1", "Level_2", "Level_3" };
    
    public int currentLevelIndex = 0;
    public string CurrentSceneName => levelSceneNames[currentLevelIndex];
    public LevelResult LastLevelResult { get; private set; }
    public List<LevelResult> allResults = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void LoadCurrentLevel() => SceneManager.LoadScene(levelSceneNames[currentLevelIndex]);

    public void AdvanceToNextlevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levelSceneNames.Count)
        {
            LoadCurrentLevel();
        }
        else
            Debug.Log("Game Finished");
    }

    public void ResetProgress()
    {
        currentLevelIndex = 0;
        allResults.Clear();
    }

    public void SetLastResult(float timeTaken, int stars)
    {
        LastLevelResult = new LevelResult
        {
            levelNumber = currentLevelIndex + 1,
            timeTaken = timeTaken,
            stars = stars
        };

        allResults.Add(LastLevelResult);
    }
}

public class LevelResult
{
    public int levelNumber;
    public float timeTaken;
    public int stars; //none damage taken over the flight
}
