using UnityEngine;

public class AvtivationDoughnut : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject gameObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            gameObject.SetActive(false);
        if (!ps.isPlaying)
        {
            ps.Play();
        }
        }
        
    }
}
