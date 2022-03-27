using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PortalScene : MonoBehaviour
{
    // [SerializeField] SceneID levelFrom;
    [SerializeField] SceneID levelToLoad;
    [SerializeField] Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagID.Player.ToString()))
        {
            SceneHelper.SharedInstance.LoadScene(levelToLoad);
        }
    }

    public SceneID SceneToLoad()
    {
        return levelToLoad;
    }

    public Vector2 GetSpawnPosition()
    {
        return spawnPoint.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for(int i = 0; i < 10; i++)
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(i * 0.3f, i * 0.3f, 0));
        }

        Handles.Label((Vector2) this.transform.position + Vector2.up * 2, "LEVEL: " + levelToLoad.ToString());

        Gizmos.DrawWireSphere(spawnPoint.position, 0.3f);
    }
#endif
}
