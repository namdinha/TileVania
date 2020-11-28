using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpSpeed = 25f;

    // State
    bool isAlive = true;

    // Cache
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float startGravityScale;
    float startAnimationSpeed;

    private void Start() {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();

        startGravityScale = myRigidbody.gravityScale;
        startAnimationSpeed = myAnimator.speed;
    }

    void Update() {
        if(isAlive) {
            Run();
            Jump();
            Climb();
            FlipSprite();
            HandleLife();
        }
    }

    private void Run() {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool isRunning = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", isRunning);
    }

    private void Jump() {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if(CrossPlatformInputManager.GetButtonDown("Jump")) {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidbody.velocity += jumpVelocityToAdd;
        }
    }

    private void Climb() {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbers"))) {
            myRigidbody.gravityScale = startGravityScale;
            myAnimator.speed = startAnimationSpeed;
            myAnimator.SetBool("Climbing", false);
            return; 
        }
        myRigidbody.gravityScale = 0;
        
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * climbSpeed);
        myRigidbody.velocity = climbVelocity;

        myAnimator.speed = Math.Sign(Mathf.Abs(myRigidbody.velocity.y));
        myAnimator.SetBool("Climbing", true);
    }

    private void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed) {
            transform.localScale = new Vector3(Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleLife() {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Traps"))) {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
