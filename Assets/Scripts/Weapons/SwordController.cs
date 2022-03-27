using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private int damagePoints = 10;
    public string swordName;

    [SerializeField] TagID targetTag;
    private Collider2D _collider;

    [Header("Audio")]
    [SerializeField] private AudioClip damageSFX;


    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }

    public void Attack(float delay, float attackDuration)
    { 
        StartCoroutine(_Attack(delay, attackDuration));
    }
    
    private IEnumerator _Attack(float delay, float attackDuration)
    {
        yield return new WaitForSeconds(delay);
        _collider.enabled = true;

        yield return new WaitForSeconds(attackDuration);
        _collider.enabled = false;
        yield return new WaitForSeconds(attackDuration);

        Awake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag.ToString()))
        {
            var component = collision.GetComponent<ITargetCombat>();

            if(component != null)
            {
                component.TakeDamage(damagePoints);
            }

            AudioManager.SharedInstance.PlaySFX(damageSFX);
        }
    }
}
