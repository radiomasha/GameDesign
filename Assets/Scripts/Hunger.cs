using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Hunger : MonoBehaviour
{
    public ScrollingEnvironment environment;
    public MeasureDistance _measureDistance;
    public Slider hunger;
    public TextMeshProUGUI UItext;
    public bool isGameStarted = true;
    [Header("Resource Settings")]
    public float maxValue = 100f;         // Максимальное значение ресурса
    public float currentValue;            // Текущее значение
    public float decreaseRate =5f;       // Уменьшение в секунду
    
    private Coroutine blinkCoroutine;
    private bool isStarvingWarningShown = false;

    //private bool isDead = false;          // Флаг, чтобы смерть происходила только один раз

    void Start()
    {
        hunger.minValue = 0f;
        hunger.maxValue = maxValue;
        currentValue = hunger.minValue;
        hunger.value = currentValue;
        isGameStarted = false;
    }

    void Update()
    {
        if (!isGameStarted) return;

        currentValue += decreaseRate * Time.deltaTime;
        currentValue = Mathf.Min(currentValue, maxValue); // ограничение сверху
        hunger.value = currentValue;

        // Не опускаться ниже 90% (т.е. остаток минимум 10%)
        float normalized = Mathf.InverseLerp(0f, maxValue * 0.9f, currentValue);
        float minSpeedMultiplier = 0.1f; // минимум 10%
        float speedMultiplier = Mathf.Lerp(1f, minSpeedMultiplier, normalized);

        environment.scrollSpeed = speedMultiplier * environment.baseSpeed; // см. ниже

        if (currentValue / maxValue > 0.7f)
        {
            
            if (!isStarvingWarningShown)
            {
                isStarvingWarningShown = true;
                blinkCoroutine = StartCoroutine(BlinkText());
                
            }
        }
        else
        {
            if (isStarvingWarningShown)
            {
                isStarvingWarningShown = false;
                if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
                UItext.text = "";
                UItext.alpha = 1f;
            }
        }
        //Debug.Log($"Energy: {currentValue}, Speed: {environment.scrollSpeed}");
    }


    /// <summary>
    /// Пополнение ресурса, но не выше максимума
    /// </summary>
    public void Refill(float amount)
    {
        //if (isDead) return;
        Debug.Log($"Refilling");
        currentValue -= amount;
        hunger.value = currentValue;
        if (currentValue < hunger.minValue)
            currentValue = hunger.minValue;
    }
    
    IEnumerator SmoothScrollSpeedReduction(ScrollingEnvironment environment, float targetMultiplier, float duration)
    {
        float startSpeed = environment.scrollSpeed;
        float targetSpeed = startSpeed * targetMultiplier;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            environment.scrollSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsed / duration);
            yield return null;
        }

        environment.scrollSpeed = targetSpeed; // гарантируем точное значение в конце
    }
    
    private IEnumerator BlinkText()
    {
        while (true)
        {
            UItext.alpha = 0f;
            yield return new WaitForSeconds(1f);
            UItext.text = "You are starving, find any food";
            UItext.alpha = 1f;
            yield return new WaitForSeconds(1f);
        }
    }


    /// <summary>
    /// Поведение при полном истощении ресурса
    /// </summary>
   /* private void Die()
    {
        isDead = true;
        _measureDistance.FinishRun();
        StartCoroutine(RestartSceneAfterDelay(3f));
    }

    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/
}