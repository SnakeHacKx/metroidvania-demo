using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlock : MonoBehaviour
{
    [SerializeField] List<GameObject> tilemapBlocks;
    
    public void StartBlock()
    {
        StartCoroutine(StartBlock_());
    }

    private IEnumerator StartBlock_()
    {
        foreach(var block in tilemapBlocks)
        {
            block.SetActive(true);
            block.GetComponent<AlphaBlinkEffect>().PlayBlinkEffect();
        }

        yield return new WaitForSeconds(2);

        foreach (var block in tilemapBlocks)
        {
            block.GetComponent<AlphaBlinkEffect>().StopBlinkEffect();
        }
    }

    public void StartUnlock()
    {
        StartCoroutine(StartUnlockCoroutine());
    }

    private IEnumerator StartUnlockCoroutine()
    {
        foreach (var block in tilemapBlocks)
        {
            block.SetActive(true);
            block.GetComponent<AlphaBlinkEffect>().PlayBlinkEffect();
        }

        yield return new WaitForSeconds(2);

        foreach (var block in tilemapBlocks)
        {
            block.GetComponent<AlphaBlinkEffect>().StopBlinkEffect();
            block.SetActive(false);
        }
    }
}
