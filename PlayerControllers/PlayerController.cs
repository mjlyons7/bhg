using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;


public class PlayerController : MonoBehaviour
{
    public PlayerStates.States playerState;
    Rigidbody2D playerBody;

    GameActions controls;

    // forces and their multipliers
    float walkingForce;
    float walkingAcceleration = 20;

    float jumpForce;
    float jumpAcceleration = 6;

    float jetpackForce;
    float jetpackAcceleration = 20;

    float maxWalkingSpeed = 5;

    // flag set in update, to tell the physics system that the jump was pressed
    bool jumpWasPressed = false;

    // Awake is called before start
    private void Awake()
    {
        controls = new GameActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerStates.States.ON_GROUND;
        playerBody = GetComponent<Rigidbody2D>();

        // calculate forces to apply based on player's weight
        walkingForce = playerBody.mass * walkingAcceleration;
        jumpForce = playerBody.mass * jumpAcceleration;
        jetpackForce = playerBody.mass * jetpackAcceleration;


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

    // Update is called once per frame
    void Update()
    { 

        // determine state of player
        if (OnGround()){
            playerState = PlayerStates.States.ON_GROUND;
        }
        else
        {
            playerState = PlayerStates.States.IN_AIR;
        }

        //----------Poll for non-continuous, non-physics inputs-------------
        if (controls.Player.Jump.WasPerformedThisFrame())
            jumpWasPressed = true;

    }

    // Do physics moves. Physics engine runs at different framerate then the graphics
    private void FixedUpdate()
    {
        // check for movement inputs
        Move();
        //clear input booleans
        jumpWasPressed = false;
    }



    // Movement controls
    void Move()
    {
        Vector2 movement = controls.Player.Move.ReadValue<Vector2>();

        // respond to movement appropriately based on state of player
        switch (playerState)
        {
            case PlayerStates.States.ON_GROUND:
                Walk(movement);
                if (jumpWasPressed) {
                    Jump();
                }
                break;

            case PlayerStates.States.IN_AIR:
                Fly(movement);
                break;

            default:
                throw new Exception("Player in undeclared state");
        }
    }

    void Walk(Vector2 movement)
    {
        float horizontalInput = movement.x;
        float playerSpeed = Mathf.Abs(playerBody.velocity.x);

        // only add more force if below max speed
        // TODO: if we're above max speed, allow player to slow down
        if (playerSpeed < maxWalkingSpeed)
        {
            playerBody.AddForce(Vector2.right * horizontalInput * walkingForce);
        }
    }

    // TODO: make jump better. Also lags because of fixedUpdate
    void Jump()
    {
        playerBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Fly(Vector2 movement)
    {
        // allow jetpack movement
        playerBody.AddForce(Vector2.right * movement.x * jetpackForce);
        playerBody.AddForce(Vector2.up * movement.y * jetpackForce);

    }

    bool OnGround()
    {
        bool isOnGround; // return value
        float colliderHeight;
        RaycastHit2D rayHit;
        float rayLength;

        // Test if we've hit the ground, measuring from the middle of our collider, plus extra
        colliderHeight = GetComponent<BoxCollider2D>().size.y;
        rayLength = (colliderHeight / 2.0f) + .05f;

        rayHit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, LayerMask.GetMask("Ground"));

        //Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red);

        isOnGround = false;
        if (rayHit)
        {
            Debug.Log(rayHit.collider.gameObject.name);
            if (rayHit.collider.gameObject.tag == "Ground")
            {
                isOnGround = true;
            }
        }

        return isOnGround;
    }

    // TODO: why does player lose speed when colliding with the ground?
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: check force of collision, apply damage
    }
}
