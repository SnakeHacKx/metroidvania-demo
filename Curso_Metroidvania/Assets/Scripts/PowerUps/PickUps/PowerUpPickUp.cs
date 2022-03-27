using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    [SerializeField] PowerUpID powerUpID;
    [SerializeField] TagID playerTag;
    [SerializeField] TagID groundTag;

    [SerializeField] AudioClip pickSFX;
    [SerializeField] int maxAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag.ToString()))
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (collision.gameObject.CompareTag(playerTag.ToString()))
        {
            var amount = Random.Range(5, maxAmount);
            
            GameManager.SharedInstance.UpdatePowerUp(powerUpID, GetComponentInChildren<SpriteRenderer>().sprite, amount);
            HeroController.SharedInstance.ChangePowerUp(powerUpID, amount);
            AudioManager.SharedInstance.PlaySFX(pickSFX);
            Destroy(this.gameObject);
        }
    }
}
