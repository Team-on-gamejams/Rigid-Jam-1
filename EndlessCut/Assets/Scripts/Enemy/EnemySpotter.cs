using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemySpotter : MonoBehaviour {
	[SerializeField] float moveSpeed = 2.0f;
	[SerializeField] bool isFacingRight = true;

	[SerializeField] float maxSeeTime = 3.0f;
	Coroutine seePlayerRoutine;

	[SerializeField] string seeText = "I see you!";
	[SerializeField] string notSeeText = "";

	[HideInInspector] [SerializeField] Rigidbody2D rb;
	[HideInInspector] [SerializeField] TextMeshProUGUI textField;

	Vector2[] moveVector;
	Vector3 prevVelosity;

	private void OnValidate() {
		if (textField == null)
			textField = GetComponentInChildren<TextMeshProUGUI>();
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();
	}

	private void Awake() {
		moveVector = new Vector2[] {
			new Vector2(moveSpeed, 0),
			new Vector2(-moveSpeed, 0),
		};

		textField.text = notSeeText;
	}

	public void FixedUpdate() {
		Move(moveVector[isFacingRight ? 0 : 1]);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Wall") {
			Flip();
			prevVelosity = Vector3.zero;
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

		theScale = textField.transform.localScale;
		theScale.x *= -1;
		textField.transform.localScale = theScale;
	}

	public void OnSpotPlayer() {
		textField.text = seeText;
		if(seePlayerRoutine == null) {
			seePlayerRoutine = StartCoroutine(SeePlayerRoutine()); ;
		}
	}

	public void OnPlayerHide() {
		textField.text = notSeeText;
		if (seePlayerRoutine != null) {
			StopCoroutine(seePlayerRoutine);
			seePlayerRoutine = null;
		}
	}

	IEnumerator SeePlayerRoutine() {
		float currSeeTime = 0.0f;

		while(currSeeTime <= maxSeeTime) {
			currSeeTime += Time.deltaTime;
			textField.text = seeText + " "+ (maxSeeTime - currSeeTime).ToString("0.00");
			yield return null;
		}
		yield return null;

		GameManager.instance.player.Die();	
	}
}
