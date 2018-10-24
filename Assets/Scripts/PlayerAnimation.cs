using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    Animator animator;
    Controller2D controller;
    Player player;
    PlayerInput input;
    SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        player = GetComponent<Player>();
        input = GetComponent<PlayerInput>();
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        // Flip sprite according to direction
        if (controller.collisions.faceDir == 1 && renderer.flipX) {
            renderer.flipX = false;
        }
        else if (controller.collisions.faceDir == -1 && !renderer.flipX) {
            renderer.flipX = true;
        }

        // Player is airborne
        if (!controller.collisions.below) {
            if (!animator.GetBool("jump")) {
                animator.SetBool("jump", true);
            }
            if (animator.GetBool("walk")) {
                animator.SetBool("walk", false);
            }
        }
        // Player is grounded and not moving
        else if (controller.collisions.below && player.directionalInput.x == 0) {
            if (animator.GetBool("jump")) {
                animator.SetBool("jump", false);
            }
            if (animator.GetBool("walk")) {
                animator.SetBool("walk", false);
            }
        }
        // Player is grounded and moving
        else if (controller.collisions.below && (player.directionalInput.x < 0 || player.directionalInput.x > 0) && !animator.GetBool("walk")) {
            if (!animator.GetBool("walk")) {
                animator.SetBool("walk", true);
            }
            if (animator.GetBool("jump")) {
                animator.SetBool("jump", false);
            }
        }

        /*
        if (player.velocity == Vector3.zero && (animator.GetBool("walk") || animator.GetBool("jump"))) {
            animator.SetBool("walk", false);
            animator.SetBool("jump", false);
        }

        if (player.velocity == Vector3.zero  {
            animator.SetBool("walk", false);
            animator.SetBool("jump", false);
        }
        */
    }
}