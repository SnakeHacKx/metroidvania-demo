using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamageTeleport : MonoBehaviour
{
    [SerializeField] private int damagePoints = 1;
    [SerializeField] TagID targetTag;
    /*private ITargetCombat component;

    private void Start()
    {
        component = GetComponent<ITargetCombat>();
    }*/

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag.ToString()))
        {
            if (component != null)
            {
                component.TakeDamage(damagePoints);

                if (HeroController.SharedInstance.Health > 0)
                {
                    HeroController.SharedInstance.UpdatePosition(HeroController.SharedInstance.LastPositionOnGround);
                }
            }
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag.ToString()))
        { 
            if (HeroController.SharedInstance != null)
            {
                HeroController.SharedInstance.TakeSpikeDamage(damagePoints);
            }
        }
    }

    public void SendPlayerToRespawnPosition()
    {
        Invoke(nameof(SendPlayerToLastGroundPosition), 0.5f);
    }

    private void SendPlayerToLastGroundPosition()
    {
        if (HeroController.SharedInstance.Health > 0)
        {
            
            HeroController.SharedInstance.UpdatePosition(HeroController.SharedInstance.LastPositionOnGround);
            Debug.Log("Debio respawnear en la posicion: " + HeroController.SharedInstance.LastPositionOnGround);
        }
    }

    /*public void ApplySpikeDamage()
    {
        StartCoroutine(ApplySpikeDamageCoroutine());
    } */

    /*IEnumerator ApplySpikeDamageCoroutine()
    {
        HeroController.SharedInstance.TakeDamage(damagePoints);



        if (HeroController.SharedInstance.Health > 0)
        {
            HeroController.SharedInstance.UpdatePosition(HeroController.SharedInstance.LastPositionOnGround);
        }
    }*/
}
