using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpotArea : MonoBehaviour {
	[HideInInspector] [SerializeField] EnemySpotter spotter;

	bool isPlayerInSpot = false;
	bool isSeePlayerInSpot = false;

	private void OnValidate() {
		if (spotter == null)
			spotter = GetComponentInParent<EnemySpotter>();
	}


	private void OnTriggerEnter2D(Collider2D collision) {
		if (!GameManager.instance.player.IsHided()) {
			isPlayerInSpot = true;
		}
	}

	private void OnTriggerStay2D(Collider2D collision) {
		bool isSeeNow = !GameManager.instance.player.IsHided();
		if (isSeeNow != isSeePlayerInSpot) {
			isSeePlayerInSpot = isSeeNow;
			if(isSeePlayerInSpot)
				spotter.OnSpotPlayer();
			else
				spotter.OnPlayerHide();
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (isSeePlayerInSpot)
			spotter.OnPlayerHide();
		isSeePlayerInSpot = isPlayerInSpot = false;
	}
}
