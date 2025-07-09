using System;
using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    public AudioSource eatingsound;
    public float refillAmount = 25f; // Сколько восстанавливает

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Hunger meter = other.GetComponent<Hunger>();
        eatingsound = other.GetComponent<AudioSource>();
        if (meter != null)
        {
            eatingsound.Play();
            meter.Refill(refillAmount);
            Destroy(gameObject); // Убираем предмет после использования
        }
    }
}