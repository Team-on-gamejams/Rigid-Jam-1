using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
	[NonSerialized] public ActiveHider currHider;

	[SerializeField] string layerDefault = "Player";
	[SerializeField] string layerHide = "PlayerHided";
	[SerializeField] SortingLayer layer;
	[SerializeField] float moveSpeed = 1.0f;
	[SerializeField] float jumpForce = 1.0f;

	[HideInInspector] [SerializeField] Rigidbody2D rb;
	[HideInInspector] [SerializeField] SpriteRenderer sp;

	Vector2 m_Look;
	Vector2 m_Move;

	private Vector3 m_Velocity = Vector3.zero;
	bool isGrounded = true;
	bool isFacingRight = true;

	int usedHiders = 0;
	bool isUseActiveHider = false;

	void OnValidate() {
		if(rb == null)
			rb = GetComponent<Rigidbody2D>();
		if(sp == null)
			sp = GetComponent<SpriteRenderer>();
	}

	void Awake() {
		GameManager.instance.player = this;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Ground" && collision.transform.position.y < transform.position.y) {
			isGrounded = true;
		}
	}

	void FixedUpdate() {
		Look(m_Look);
		Move(m_Move);
	}

	public void OnMove(InputAction.CallbackContext context) {
		m_Move = context.ReadValue<Vector2>();
		if (m_Move.y < 0)
			m_Move.y = 0;
		if (m_Move.sqrMagnitude > 0.5f && isUseActiveHider) {
			isUseActiveHider = false;
			currHider.UnHidePlayer();
			sp.sortingLayerName = layerDefault;
		}
	}

	public void OnLook(InputAction.CallbackContext context) {
		m_Look = context.ReadValue<Vector2>();
	}

	public void OnInteract(InputAction.CallbackContext context) {
		bool isInteract = context.ReadValueAsButton();
		if (isInteract) {
			if(currHider != null) {
				if (!isUseActiveHider) {
					isUseActiveHider = true;
					currHider.HidePlayer();
					sp.sortingLayerName = layerHide;
				}
			}
		}
	}

	private void Move(Vector2 direction) {
		Vector3 targetVelocity = new Vector2(m_Move.x * moveSpeed, rb.velocity.y);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, .05f);

		if (m_Move.x > 0 && !isFacingRight) {
			Flip();
		}
		else if (m_Move.x < 0 && isFacingRight) {
			Flip();
		}

		if (isGrounded && m_Move.y >= 0.5f) {
			isGrounded = false;
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}

	private void Look(Vector2 rotate) {

	}

	private void Flip() {
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Hide() {
		++usedHiders;
		sp.sortingLayerName = layerHide;
	}

	public void UnHide() {
		--usedHiders;
		if(usedHiders == 0)
			sp.sortingLayerName = layerDefault;
	}

	public bool IsHided() {
		return usedHiders != 0 || isUseActiveHider;
	}

	public void Die() {
		Application.LoadLevel(Application.loadedLevel);
	}
}
