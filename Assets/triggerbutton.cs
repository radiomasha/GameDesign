using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TriggerButton : MonoBehaviour
{

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnTriggerEnter(Collider other)
    {

        button.onClick.Invoke();

    }
}