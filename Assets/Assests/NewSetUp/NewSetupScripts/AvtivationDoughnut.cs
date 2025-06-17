using UnityEngine;

public class AvtivationDoughnut : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject gameObject;

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
        if (!ps.isPlaying)
        {
            ps.Play();
        }
    }
}
