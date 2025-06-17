using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        float time = Time.timeSinceLevelLoad;
        int stars = 0; //depending on the quality of going through level
        LevelManager.Instance.SetLastResult(time, stars);
        SceneManager.LoadScene("LevelTransitionMenu");
   }
}
