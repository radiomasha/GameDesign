using System;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    private GeneralObjPool pool;
    public GameObject explosionparticles;
    public AudioSource explosionaudio;
    public int damageAmount = 25;

    private ParticleSystem explosion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        explosion = explosionparticles.GetComponent<ParticleSystem>();
        explosionaudio = explosionparticles.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (!other.CompareTag("Player")) return;

        if (explosionparticles != null)
        {
            explosionparticles.transform.SetParent(null);
        


            if (explosionaudio != null && explosionaudio != null)
            {
                explosion.Play();              // particle system starts
                explosionaudio.Play();           // audio plays immediately (if object is already active)
            
                // destroy explosion GameObject after particles finish
                Destroy(explosionparticles, explosion.main.duration + explosion.main.startLifetime.constantMax);
            }
        }

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
        }

        ReturnToPool();
    }


    private void ReturnToPool()
    {
        enabled = false;
        Destroy(gameObject);
        //pool.Return(gameObject);
    }
    // Update is called once per frame

}
