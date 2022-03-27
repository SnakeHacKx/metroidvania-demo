using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] WaypointsManager waypointsManager;

    private Vector2 currentPosition;

    private void Awake()
    {
        if(waypointsManager != null)
            currentPosition = waypointsManager.GetNextPoint();  
    }

    void Update()
    {
        if (Vector2.Distance(this.transform.position, currentPosition) > 0.1f)
        {
            var direction = currentPosition - (Vector2) this.transform.position;
            this.transform.Translate(direction.normalized * speed * Time.deltaTime);
        }
        else
        {
            currentPosition = waypointsManager.GetNextPoint();
        }
    }
}
