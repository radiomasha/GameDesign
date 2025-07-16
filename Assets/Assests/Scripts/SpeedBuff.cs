using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    [SerializeField] private ScrollingEnvironment environment;
    [SerializeField] private Hunger hunger;
    [SerializeField] private GameObject Buff;
    [SerializeField] private GameObject SpeedBuffVFX;
    [SerializeField] private AudioSource SpeedBuffSFX;
    [SerializeField] private AudioSource SwallowSFX;
    private float timer = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buff"))
        {
            SpeedBuffSFX.Play();
            SpeedBuffSFX.Play();
            ActivateSpeedBuff();
            Destroy(other.gameObject);
            
        } 
    }

    public void ActivateSpeedBuff()
    {
        
        environment.scrollSpeed = 7f;
        hunger.isBuffed = true;
        Buff.SetActive(true);
        SpeedBuffVFX.GetComponent<ParticleSystem>().Play();
        StartCoroutine(SpeedBuffCoroutine(timer));
    }

    IEnumerator SpeedBuffCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        hunger.isBuffed = false;
        Buff.SetActive(false);
        
    }

}
