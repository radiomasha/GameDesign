using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SceneResetManager : MonoBehaviour
{
    public static SceneResetManager Instance;

    public TextMeshProUGUI messageText; // можно не задавать, если не нужно

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RestartSceneAfterDelay(float delay )
    {
        StartCoroutine(RestartCoroutine(delay));
    }

    private IEnumerator RestartCoroutine(float delay)
    {
        

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}