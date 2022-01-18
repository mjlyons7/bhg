using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public GameObject[] players = new GameObject[5];
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // get reference to player, so can follow them
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) {
            throw new Exception("No game object with player tag found");
        }
        else
        {
            player = players[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
    }
}
