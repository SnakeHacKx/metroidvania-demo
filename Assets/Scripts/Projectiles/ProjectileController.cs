using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] TagID targetTag;
    [SerializeField] int damagePoints = 10;
    [SerializeField] AudioClip projectileSFX;

    [SerializeField] GameObject explosionPrefab;


    public void SetDirection(Vector2 direction)
    {
        if(direction.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag.ToString()))
        {
            var component = collision.GetComponent<ITargetCombat>();

            if (component != null)
            {
                component.TakeDamage(damagePoints);
            }

            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            }

            AudioManager.SharedInstance.PlaySFX(projectileSFX);
            Destroy(this.gameObject);
        }
    }
}
