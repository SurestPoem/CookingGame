using UnityEngine;

public class Gun : MonoBehaviour, IUsable
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float cooldownTime = 0.5f;
    private float lastUsedTime = -Mathf.Infinity;
    [SerializeField] private Transform playersCameraTransform;

    public void Use(GameObject user)
    {
        if (Time.time - lastUsedTime < cooldownTime)
        {
            return; // Still in cooldown
        }
        Shoot(user);
    }

    private void Shoot(GameObject gunOwner)
    {
        lastUsedTime = Time.time;
        if (Physics.Raycast(playersCameraTransform.position, playersCameraTransform.forward, out RaycastHit raycastHit, 1000))
        {
            if (raycastHit.transform.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.TakeDamage(damageAmount, gunOwner);
            }
        }
        Debug.Log("Gun shot");
        ShootEffects();
    }

    private void ShootEffects()
    {
        //Add shooting effects here later
    }
}