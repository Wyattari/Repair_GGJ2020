﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : BaseBehaviour {
	int playerId;
	public GameObject LaserPrefab;
	GameObject laser;
	public GameObject BallPrefab;
	GameObject ball;

	LaserBeamManager laserBeamManager;
	PlayerColor playerColor;

	public int PlayerId {
		get { return playerId; }
		set {
			playerId = value;
			var player = state.Players[playerId];
			playerColor.Color = player.Color;
			laserBeamManager.PlayerId = playerId;
		}
	}

	void Awake() {
		ball = Instantiate(BallPrefab);
		laser = Instantiate(LaserPrefab);
		laserBeamManager = laser.GetComponent<LaserBeamManager>();
		playerColor = ball.GetComponentInChildren<PlayerColor>();
		Subscribe();
	}

	void Subscribe() {
		Unsubscribe();
		events.OnHit += Events_OnHit;
	}

	void Unsubscribe() {
		events.OnHit -= Events_OnHit;
	}

	void Events_OnHit(int playerId, Vector3 hitPosition) {
		if (playerId != PlayerId) { return; }
		laserBeamManager.ShootBeams(laser.transform.position, hitPosition);
	}

	void Update() {
		laser.transform.position = transform.position;
		ball.transform.position = transform.position;
	}

	void OnDestroy() {
		Unsubscribe();
		Destroy(laser);
		Destroy(ball);
	}
}
