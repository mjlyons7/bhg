using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteEffects : MonoBehaviour
{
    GameObject player;
    PlayerStates.States playerState;

    GameActions controls;

    public GameObject smokeParticle;
    float spawnRate = 0.1f;

    private void Awake()
    {
        controls = new GameActions();
    }
    // enable the controller
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        // get references to player state and smoke
        player = GameObject.FindGameObjectWithTag("Player");

    }



    // Update is called once per frame
    void Update()
    {
        
        Vector2 direction;

        // If player in flight, get movement values, spawn smoke in opposite direction
        // TODO: shouldn't do polling; could probably get signal from player
        playerState = player.GetComponent<PlayerController>().playerState;
        if (playerState == PlayerStates.States.IN_AIR)
        {
            // If player using jetpack, spawn smoke in opposite direction
            direction = controls.Player.Move.ReadValue<Vector2>();
            if (direction.magnitude > 0) spawnSmoke(direction);
        }
    }

    void spawnSmoke(Vector2 direction)
    {
        GameObject currentSmokeParticle;
        Rigidbody2D smokeRB;
        Vector2 smokeDirection;
        float smokeForce = 0.5f;

        // spawn at player's position
        currentSmokeParticle = Instantiate(smokeParticle);
        currentSmokeParticle.transform.position = player.transform.position;

        // apply force in opposite direction of motion
        smokeDirection = -direction.normalized;
        smokeRB = currentSmokeParticle.GetComponent<Rigidbody2D>();
        smokeRB.AddForce(smokeDirection * smokeForce, ForceMode2D.Impulse);

        // apply random force for better believability


    }
}
