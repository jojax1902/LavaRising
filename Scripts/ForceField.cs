using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float shrinkWaitTime;
    public float shrinkAmount;
    public float shrinkDuration;
    public float minShrinkAmount;

    public int playerDamage;

    private float lastShrinkEndTime;
    private bool shrinking;
    private float targetHeight;
    private float lastPlayerCheckTime;

    void Start()
    {
        lastShrinkEndTime = Time.time;
        targetHeight = transform.localScale.x;
    }

    void Update()
    {
        if (shrinking)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * targetHeight, (shrinkAmount / shrinkDuration) * Time.deltaTime);
            if (transform.localScale.x == targetHeight)
                shrinking = false;
        }
        else
        {
            if (Time.time - lastShrinkEndTime >= shrinkWaitTime && transform.localScale.x > minShrinkAmount)
                Shrink();
        }

        CheckPlayers();
    }

    void Shrink()
    {
        shrinking = true;

        if (transform.localScale.x - shrinkAmount > minShrinkAmount)
            targetHeight -= shrinkAmount;
        else
            targetHeight = minShrinkAmount;

        lastShrinkEndTime = Time.time + shrinkDuration;
    }

    void CheckPlayers()
    {
        if (Time.time - lastPlayerCheckTime > 1.0f)
        {
            lastPlayerCheckTime = Time.time;
            foreach(PlayerController player in GameManager.instance.players)
            {
                if (!player || player.dead)
                {
                    continue;
                }
                if (Vector3.Distance(Vector3.zero, player.transform.position) >= transform.localScale.x)
                {
                    player.photonView.RPC("TakeDamage", player.photonPlayer, 0, playerDamage);
                }
            }
        }
    }
}
