using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLauncherController : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float force;

    public void Launch(Vector2 direction)
    {
        // Quaternion.identity: pa que no rote
        GameObject gameObj = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
        gameObj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        gameObj.GetComponent<ProjectileController>().SetDirection(direction);
    }
}
