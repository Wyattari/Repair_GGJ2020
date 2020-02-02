using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour {
	GameEvents events;
	List<Player> players = new List<Player>();

	Dictionary<int, int> playerMap = new Dictionary<int, int>();

	// Start is called before the first frame update
	void Start() {
		for (int i = 0; i < 4; i++) {
			players.Add(ReInput.players.GetPlayer(i));
		}

		Subscribe();
		events = GameManager.Instance.Events;
	}

	void Unsubscribe() {
		ReInput.ControllerConnectedEvent -= ReInput_ControllerConnectedEvent;
	}

	void Subscribe() {
		Unsubscribe();
		ReInput.ControllerConnectedEvent += ReInput_ControllerConnectedEvent;
	}

	void ReInput_ControllerConnectedEvent(ControllerStatusChangedEventArgs args) {
		Debug.Log($"Controller connected. controllerId={args.controllerId}");
	}

	void Update() {
		foreach (var player in players) {
			if (!playerMap.ContainsKey(player.id)) {
				var wantsJoin = player.GetButtonDown("Fire") || player.GetButtonDown("Jump");
				if (!wantsJoin) { return; }
				playerMap[player.id] = playerMap.Count;
				events.PlayerJoin(playerMap[player.id]);
				GameManager.Instance.PlayerCount++;
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
				Debug.Log($"id={id} MoveHorizontal={player.GetAxis("MoveHorizontal")} MoveVertical={player.GetAxis("MoveVertical")} AimHorizontal={player.GetAxis("AimHorizontal")} AimVertical={player.GetAxis("AimVertical")} Fire={player.GetButton("Fire")} Jump={player.GetButton("Jump")}");
			}
			// left shoulder event
			if (player.GetButtonDown("Jump")) {
				events.PlayerJump(id);
			}
		}
	}
}
