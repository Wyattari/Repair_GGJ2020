using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	int playerId;
	public int PlayerId {
		get { return playerId; }
		set {
			playerId = value;
			var player = GameManager.Instance.State.Players[playerId];
			Debug.Log($"Setting color to {player.Color}");
		}
	}

	GameObject laserBeam;
	GameObject movingSphere;

	void Awake() {
		Debug.Log("We out here");
	}
}
