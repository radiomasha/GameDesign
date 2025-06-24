using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Meta.XR.ImmersiveDebugger.UserInterface;
using TMPro;

public class Health : MonoBehaviour
{
    // Maximum health points the object starts with
    //public float maxHP;
    public MeasureDistance _measureDistance;
    public Slider health;
    public TextMeshProUGUI text;
    private float maxHP = 100f;
    // Tracks the current health during gameplay
    private float currentHP;

    private void Awake()
    {
        
    
    }

    // Initialize health when the object is created
    void Start()
    {
        health.minValue = 0f;
        health.maxValue = maxHP;
        currentHP = maxHP;
        health.value = currentHP;
        
    }

    private void Update()
    {
       // text.text = $"{currentHP}"; 
    }
    // Public method to apply damage to this object
    public void TakeDamage(float amount)
    {
        Debug.Log("DamageTaken");

        // Reduce the current health by the damage amount
        currentHP -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {currentHP}");
        health.value = currentHP;
        // Check if the object has run out of HP
        if (currentHP <= health.minValue)
        {
            Die();
        }
    }

    // Handles what happens when HP reaches zero
    private void Die()
    {
        _measureDistance.FinishRun();
        Debug.Log($"{gameObject.name} has been destroyed.");

        // Tell the GameManager to restart the scene after a short delay
        //GameManager.Instance.RestartSceneAfterDelay(3f);

        // Deactivate the object to remove it from the scene (but keep it alive for coroutine to finish)
        gameObject.SetActive(false);
    }
}