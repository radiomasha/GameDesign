using UnityEngine;

public class AutoPlayParticle : MonoBehaviour
{
    public ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ps != null)
        {
            ps.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
