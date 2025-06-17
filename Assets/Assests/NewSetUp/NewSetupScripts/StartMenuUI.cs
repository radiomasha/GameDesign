using UnityEngine;


public class StartMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.Instance.ResetProgress();
        LevelManager.Instance.LoadCurrentLevel();

    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
