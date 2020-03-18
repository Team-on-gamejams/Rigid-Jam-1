using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
	[NonSerialized] public ActiveHider currHider;

	[SerializeField] string layerDefault = "Player";
	[SerializeField] string layerHide = "PlayerHided";
	[SerializeField] SortingLayer layer;
	[SerializeField] float moveSpeed = 1.0f;
	[SerializeField] float jumpForce = 1.0f;
	[SerializeField] GameObject canvas = null;
	[SerializeField] Slider hpSlider = null;
	[SerializeField] GameObject cutPrefab = null;
	[SerializeField] Transform cutPos = null;
	float hp = 1.0f;

	[SerializeField] float attackDelay = 0.5f;
	[SerializeField] float currAttackDelay = 0.5f;

	[HideInInspector] [SerializeField] Rigidbody2D rb;
	[HideInInspector] [SerializeField] SpriteRenderer sp;
	[HideInInspector] [SerializeField] Animator anim;

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
		if (anim == null)
			anim = GetComponent<Animator>();
	}

	void Awake() {
		GameManager.instance.player = this;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Ground" && collision.transform.position.y < transform.position.y) {
			isGrounded = true;
			anim.SetBool("IsGrounded", true);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.transform.tag == "Damage") {
			TakeHit();
		}
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.transform.tag == "Ground" && collision.transform.position.y < transform.position.y) {
			anim.SetBool("IsGrounded", false);
		}
	}

	private void Update() {
		currAttackDelay -= Time.deltaTime;

		if (Input.GetKey(KeyCode.Space) && currAttackDelay < 0) {
			anim.SetTrigger("Attack");
			Instantiate(cutPrefab, cutPos.position, Quaternion.identity, null);
			currAttackDelay = attackDelay;
		}
	}

	void FixedUpdate() {
		Look(m_Look);
		Move(m_Move);
	}

	public void TakeHit() {
		anim.SetTrigger("TakeHit");
		hp -= 0.1f;
		hpSlider.value = hp;
	}

	public void OnMove(InputAction.CallbackContext context) {
		m_Move = context.ReadValue<Vector2>();
		if (m_Move.y < 0)
			m_Move.y = 0;
		if (m_Move.sqrMagnitude > 0.5f) {
			anim.SetBool("IsMoving", true);
		}
		else {
			anim.SetBool("IsMoving", false);
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
			anim.SetTrigger("Jump");
			anim.SetBool("IsGrounded", false);
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

		theScale = canvas.transform.localScale;
		theScale.x *= -1;
		canvas.transform.localScale = theScale;
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
