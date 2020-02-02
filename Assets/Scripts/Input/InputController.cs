using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class InputController : MonoBehaviour {
	GameEvents events;
	List<Player> players = new List<Player>();

	[NonSerialized]
	Dictionary<int, int> playerMap = new Dictionary<int, int>();

	void Start() {
		for (int i = 0; i < 4; i++) {
			players.Add(ReInput.players.GetPlayer(i));
		}

		events = GameManager.Instance.Events;
	}

	void Update() {
		foreach (var player in players) {
			if (!playerMap.ContainsKey(player.id)) {
				var wantsJoin = player.GetButtonDown("Fire") || player.GetButtonDown("Jump") || player.GetAxis("MoveHorizontal") + player.GetAxis("MoveVertical") + player.GetAxis("AimHorizontal") + player.GetAxis("AimVertical") > 0.2f;
				if (!wantsJoin) { continue; }
				playerMap[player.id] = playerMap.Count;
				events.PlayerJoin(playerMap[player.id]);
			}

			var id = playerMap[player.id];

			// left stick events
			events.PlayerMove(id, new Vector2(
				player.GetAxis("MoveHorizontal"),
				player.GetAxis("MoveVertical")
			));

			// right stick events
			events.PlayerAim(id, new Vector2(
				player.GetAxis("AimHorizontal"),
				player.GetAxis("AimVertical")
			));

			// right shoulder event
			if (player.GetButtonDown("Fire")) {
				events.PlayerFire(id);
			}
			if (player.GetButtonUp("Fire")) {
				events.PlayerFireRelease(id);
			}
			// left shoulder event
			if (player.GetButtonDown("Jump")) {
				events.PlayerJump(id);
			}
		}
	}
}
