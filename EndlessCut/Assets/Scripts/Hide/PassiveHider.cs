using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveHider : MonoBehaviour {
	[SerializeField] GameObject tutorial;

	private void Awake() {
		tutorial?.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			GameManager.instance.player.Hide();
			tutorial?.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Player") {
			GameManager.instance.player.UnHide();
			tutorial?.SetActive(false);
		}
	}
}
