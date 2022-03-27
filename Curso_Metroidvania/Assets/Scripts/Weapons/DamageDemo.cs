using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDemo : MonoBehaviour, ITargetCombat
{
    [SerializeField] private int health;
    [SerializeField] private DamageFeedbackEffect damageFeedbackEffect;

    void ITargetCombat.TakeDamage(int damagePoints)
    {
        damageFeedbackEffect.PlayDamageEffect();
        health -= damagePoints;
    }
}
