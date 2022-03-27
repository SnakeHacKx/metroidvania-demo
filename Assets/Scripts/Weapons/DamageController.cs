using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int damagePoints = 10;

    [SerializeField] TagID targetTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag.ToString()))
        {
            var component = collision.GetComponent<ITargetCombat>();

            if (component != null)
            {
                component.TakeDamage(damagePoints);
            }
        }
    }
}
