using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] TagID playerTag;
    [SerializeField] TagID groundTag;

    [SerializeField] AudioClip pickSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag.ToString()))
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (collision.gameObject.CompareTag(playerTag.ToString()))
        {
            HeroController.SharedInstance.GiveCoin();
            AudioManager.SharedInstance.PlaySFX(pickSFX);
            Destroy(this.gameObject);
        }
    }
}
