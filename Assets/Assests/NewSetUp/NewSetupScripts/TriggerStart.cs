
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TriggerStart : MonoBehaviour
{
    private Button button;
    public UnityEvent onPressed;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {

        button.onClick.Invoke();
        onPressed.Invoke();
        
    }
}

