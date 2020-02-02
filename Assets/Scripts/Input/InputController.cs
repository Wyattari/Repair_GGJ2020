using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour {
	GameEvents events;
	List<Player> players = new List<Player>();

	// Start is called before the first frame update
	void Start() {
		for (int i = 0; i < 4; i++) {
			players.Add(ReInput.players.GetPlayer(i));
		}

		events = GameManager.Instance.Events;
	}

	void Update() {
		foreach (var player in players) {
			// left stick events
			events.PlayerMove(player.id, new Vector2(
				player.GetAxis("MoveHorizontal"),
				player.GetAxis("MoveVertical")
			));

			// right stick events
			events.PlayerAim(player.id, new Vector2(
				player.GetAxis("AimHorizontal"),
				player.GetAxis("AimVertical")
			));

			// right shoulder event
			if (player.GetButtonDown("Fire")) {
				events.PlayerFire(player.id);
				Debug.Log($"id={player.id} MoveHorizontal={player.GetAxis("MoveHorizontal")} MoveVertical={player.GetAxis("MoveVertical")} AimHorizontal={player.GetAxis("AimHorizontal")} AimVertical={player.GetAxis("AimVertical")} Fire={player.GetButton("Fire")} Jump={player.GetButton("Jump")}");
			}
			// left shoulder event
			if (player.GetButtonDown("Jump")) {
				events.PlayerJump(player.id);
			}
		}
	}
}
