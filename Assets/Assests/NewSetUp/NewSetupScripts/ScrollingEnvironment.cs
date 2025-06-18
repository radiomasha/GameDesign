using UnityEngine;

public class ScrollingEnvironment : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float scrollSpeed;

    void Start()
    {
        scrollSpeed = baseSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime);
    }
}

