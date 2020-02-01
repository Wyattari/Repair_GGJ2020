using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour {
	List<Player> players = new List<Player>();

	// Start is called before the first frame update
	void Start() {
		for (int i = 0; i < 4; i++) {
			players.Add(ReInput.players.GetPlayer(i));
		}
	}

	// Update is called once per frame
	void Update() {
		foreach (var player in players) {
			if (player.GetButtonDown("Fire")) {
				Debug.Log($"id={player.id} MoveHorizontal={player.GetAxis("MoveHorizontal")} MoveVertical={player.GetAxis("MoveVertical")} AimHorizontal={player.GetAxis("AimHorizontal")} AimVertical={player.GetAxis("AimVertical")} Fire={player.GetButton("Fire")} Jump={player.GetButton("Jump")}");
			}
		}
	}
}
