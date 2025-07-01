using System;
using System.Collections;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using TMPro;
using UnityEngine;


public class Menu: MonoBehaviour
{
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private Hunger _hunger;
    public GameObject SpawnZone;
    public TextMeshProUGUI RunText;
    private float seconds = 3.0f;
    private bool isStarted = false;
    

    public void StartRun()
    {
        if (!SpawnZone.activeSelf)
        {
            StartCoroutine(RunCountdown());
            _spawnManager.GetComponent<SpawnManager>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!SpawnZone.activeSelf)
        {
            StartCoroutine(RunCountdown());
            
        }
    }

    private IEnumerator RunCountdown()
    {
        float countdown = seconds;

        while (countdown > 0)
        {
            RunText.text = Mathf.CeilToInt(countdown).ToString();
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        RunText.text = "GO!";
        yield return new WaitForSeconds(1f);
        RunText.text = "";
        SpawnZone.SetActive(true);
        _spawnManager.enabled = true;
        _hunger.isGameStarted = true;
    }
}
