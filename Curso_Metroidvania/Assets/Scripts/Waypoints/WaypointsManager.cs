using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    int indexPoint = 0;

    public Vector2 GetNextPoint()
    {
        // si el indice llegó al numero máximo de hijos que tiene
        if(indexPoint >= this.transform.childCount)
        {
            indexPoint = 0;
        }

        var position = this.transform.GetChild(indexPoint).transform.position;
        indexPoint++;

        return position;
    }

    public Vector2 GetRandomPoint()
    {
        return this.transform.GetChild(Random.Range(0, this.transform.childCount)).transform.position;
    }

#if UNITY_EDITOR // Nos evita problemas de compilación, ya que le estamos diciendo al script que
    // sólo compile esta parte en el editor de unity

    // nos permite visualizar mejor lo que estamos haciendo
    private void OnDrawGizmos()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Gizmos.DrawWireSphere(this.transform.GetChild(i).transform.position, 0.3f);
            Handles.Label(this.transform.GetChild(i).transform.position, "Point " + (i + 1));
        }
    }
#endif
}
