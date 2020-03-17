using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveHider : MonoBehaviour {
	[SerializeField] GameObject tutorial;

	Quaternion startPlayerRot;

	private void Awake() {
		tutorial?.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			GameManager.instance.player.currHider = this;
			tutorial?.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Player") {
			if (GameManager.instance.player.currHider == this)
				GameManager.instance.player.currHider = null;
			tutorial?.SetActive(false);
		}
	}

	public void UnHidePlayer() {
		GameManager.instance.player.transform.rotation = startPlayerRot;
		tutorial?.SetActive(true);
	}

	public void HidePlayer() {
		startPlayerRot = GameManager.instance.player.transform.rotation;
		GameManager.instance.player.transform.rotation = Quaternion.Euler(0, 0, -33f);
		GameManager.instance.player.transform.position = new Vector3(transform.position.x, GameManager.instance.player.transform.position.y, GameManager.instance.player.transform.position.z);
		tutorial?.SetActive(false);
	}
}
