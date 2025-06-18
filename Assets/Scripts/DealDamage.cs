using UnityEngine;

public class DealDamage : MonoBehaviour
{
    private ObjectPool pool;
    public int damageAmount = 10;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;

        if (!other.CompareTag("Player")) return; // ✅ Проверка тега

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
            ReturnToPool();
        }
    }
    private void ReturnToPool()
    {
        enabled = false;
        pool.Return(gameObject);
    }
    // Update is called once per frame

}
