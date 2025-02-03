using UnityEngine;

public class TestDamageble : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damageAmt)
    {
        Debug.Log($"{gameObject.name} took {damageAmt} damage");
    }
}