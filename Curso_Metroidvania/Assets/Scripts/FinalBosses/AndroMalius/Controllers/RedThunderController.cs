using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedThunderController : MonoBehaviour
{
    Animator animator;
    [SerializeField] GameObject feedback;
    [SerializeField] GameObject firePrefab;
    [SerializeField] AudioClip thunderSfx;

    [Header("Debug")]
    public bool launch;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.Play("Idle");
    }


    public void LaunchThunder()
    {
        StartCoroutine(_LaunchThunder());
    }
    public IEnumerator _LaunchThunder()
    {
        feedback.SetActive(true);
        yield return new WaitForSeconds(1);
        feedback.SetActive(false);
        AudioManager.SharedInstance.PlaySFX(thunderSfx);
        animator.Play("Thunder");
        yield return new WaitForSeconds(0.4f);
        Instantiate(firePrefab, feedback.transform.position, Quaternion.identity);
        animator.Play("Idle");
        Destroy(this.gameObject);
    }


    private void Update()
    {
        if (launch)
        {
            LaunchThunder();
            launch = false;
        }
    }
}