using UnityEngine;

public class ScrollingEnvironment : MonoBehaviour
{
    public float scrollSpeed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime);
    }
}
