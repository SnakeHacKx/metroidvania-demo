using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] Vector3 parallaxFactor;
    [SerializeField] float speed;
    [SerializeField] float offsetZ;

    Vector3 lastPosition;

    void Start()
    {
        lastPosition = Camera.main.transform.position;
        lastPosition.z = 0;
        this.transform.localPosition = new Vector3(0, 0, offsetZ);
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.localPosition = new Vector3(0, 0, offsetZ);
        /* var cameraParallax = new Vector3(
             Camera.main.transform.position.x * parallaxFactor.x,
             Camera.main.transform.position.y * parallaxFactor.y,
             0
             );

         this.transform.position -= (lastPosition - cameraParallax) * speed;

         lastPosition = cameraParallax;*/
    }
}
