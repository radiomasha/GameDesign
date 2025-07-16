using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Health : MonoBehaviour
{
    public GameObject env;
    public Slider health;
    public TextMeshProUGUI text;
    public GameObject explosionobject; // Must contain a ParticleSystem
    public GameObject bgmusicobject;
    private float maxHP = 100f;
    private float currentHP;

    private void Start()
    {
        health.minValue = 0f;
        health.maxValue = maxHP;
        currentHP = maxHP;
        health.value = currentHP;
    }

    private void Update()
    {
        // Optional display of HP as text
        // text.text = $"{currentHP}";
    }

    public void TakeDamage(float amount)
    {
        if (gameObject.GetComponent<Hunger>().isBuffed) return;
        currentHP -= amount;
        health.value = currentHP;

        if (currentHP <= health.minValue)
        {
            Die();
        }
    }

    private void Die()
    {
        if (explosionobject != null)
        {
            explosionobject.transform.SetParent(null);
            explosionobject.SetActive(true);

            AudioSource explosionSFX = explosionobject.GetComponent<AudioSource>();
            ParticleSystem ps = explosionobject.GetComponent<ParticleSystem>();
            AudioSource backgroundmusic = bgmusicobject.GetComponent<AudioSource>();

            if (ps != null)
            {
                env.GetComponent<ScrollingEnvironment>().scrollSpeed = 0;
                ps.Play();
                explosionSFX.Play();
                backgroundmusic.Stop();
                text.text = "Ooops! Next time be more careful!";
                Destroy(explosionobject, ps.main.duration + ps.main.startLifetime.constantMax);

                // 🔁 запуск корутины ДО SetActive(false)
                SceneResetManager.Instance.RestartSceneAfterDelay(3f);


                // теперь можно выключать объект
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Explosion object has no ParticleSystem.");
            }
        }
        else
        {
            Debug.LogWarning("Explosion GameObject is not assigned.");
        }
    }

    
    
}