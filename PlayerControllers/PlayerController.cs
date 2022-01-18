using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;


public class PlayerController : MonoBehaviour
{
    PlayerStates.States playerState;
    Rigidbody2D playerBody;

    GameActions controls;

    // forces and their multipliers
    float walkingForce;
    float walkingForceMassProportionalConstant = 20;

    float jumpForce;
    float jumpForceMassProportionalConstant = 6;

    float jetpackForce;
    float jetpackForceMassProportionalConstant = 10;

    float maxWalkingSpeed = 5;

    // flag set in update, to tell the physics system that the jump was pressed
    bool jumpWasPressed = false;

    // raycast to detect the floor
    RaycastHit2D floorDetector;

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
        walkingForce = playerBody.mass * walkingForceMassProportionalConstant;
        jumpForce = playerBody.mass * jumpForceMassProportionalConstant;
        jetpackForce = playerBody.mass * jetpackForceMassProportionalConstant;


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

        //----------Look for non-continuous inputs-------------
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
            Debug.Log(string.Format("Player speed: {0}", playerSpeed));
        }

  
        
    }

    // TODO: make jump better. Also lags because of fixedUpdate
    void Jump()
    {
        playerBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Fly(Vector2 movement)
    {

    }

    bool OnGround()
    {
        bool isOnGround = false;

        // Test if we've hit the ground, measuring from the middle of our collider, plus extra
        // TODO: is hitting player, so no good
        float colliderHeight = GetComponent<BoxCollider2D>().size.y;
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, colliderHeight / 2 + 1);

        if (groundHit)
        {
            Debug.Log(groundHit.collider.gameObject.name);
            Debug.Log(colliderHeight);
            if (groundHit.collider.gameObject.tag == "Ground")
            {
                isOnGround = true;
            }
        }

        return isOnGround;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ground"){
            Debug.Log("On the ground");
        }
    }
}