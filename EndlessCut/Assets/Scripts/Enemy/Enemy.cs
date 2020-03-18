using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[SerializeField] float moveSpeed = 2.0f;
	[SerializeField] bool isFacingRight = true;

	[HideInInspector] [SerializeField] Rigidbody2D rb;
	[HideInInspector] [SerializeField] Animator anim;

	Vector2[] moveVector;
	Vector3 prevVelosity;

	private void OnValidate() {
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();
		if (anim == null)
			anim = GetComponent<Animator>();
	}

	private void Awake() {
		moveVector = new Vector2[] {
			new Vector2(moveSpeed, 0),
			new Vector2(-moveSpeed, 0),
		};
	}

	public void FixedUpdate() {
		if(moveSpeed != 0.0f)
			Move(moveVector[isFacingRight ? 0 : 1]);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Wall") {
			Flip();
			prevVelosity = Vector3.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.transform.tag == "Damage" && moveSpeed != 0.0f) {
			anim.SetTrigger("Die");
			moveSpeed = 0.0f;
			rb.velocity = Vector3.zero;
			LeanTween.delayedCall(1.0f, () => { Destroy(gameObject); });
		}
	}

	private void Move(Vector2 direction) {
		Vector3 targetVelocity = new Vector2(direction.x, rb.velocity.y);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref prevVelosity, .05f);
	}

	private void Flip() {
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
