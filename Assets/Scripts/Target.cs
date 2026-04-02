using UnityEngine;

public class Target : MonoBehaviour, IDamagable
{
    [SerializeField] private float health = 100f;
    public void TakeDamage(float amount, GameObject damageSource)
    {
        health -= amount;
        if (health <= 0f)
        {
            DestroyTarget();
        }
    }

    private void DestroyTarget()
    {
        //Add destruction effects here later
        Destroy(gameObject);
    }
}
