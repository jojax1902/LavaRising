using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Lava : MonoBehaviour
{

    public GameObject lava;
    public float lastPlayerCheckTime = 0f;
    public int playerDamage = 25;
    public float risingTime;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= risingTime)
        lava.transform.localScale += (new Vector3(0,0.01f,0));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time - lastPlayerCheckTime > 1.0f)
        {
            lastPlayerCheckTime = Time.time;
            foreach (PlayerController player in GameManager.instance.players)
            {
                if (!player || player.dead)
                {
                    continue;
                }
                if (player == collision.gameObject.GetComponent<PlayerController>())
                {
                    player.photonView.RPC("TakeDamage", player.photonPlayer, 0, playerDamage);
                }
            }
        }
    }
}
