﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
	GameEvents events;

	GameObject[] spawnedPlayers = new GameObject[4];
	[SerializeField] Transform[] playerSpawns;
	[SerializeField] GameObject player;

	void Start() {
		events = GameManager.Instance.Events;
		Subscribe();
	}

	void Subscribe() {
		Unsubscribe();
		events.OnRespawn += Events_OnRespawn;
	}

	void Unsubscribe() {
		events.OnRespawn -= Events_OnRespawn;
	}

	void Events_OnRespawn() {
		foreach (GameObject player in spawnedPlayers) {
			Destroy(player);
		}

		for (int i = 0; i < GameManager.Instance.PlayerCount; i++) {
			spawnedPlayers[i] = Instantiate(player, playerSpawns[i]);
			spawnedPlayers[i].GetComponent<MovingSphere>().PlayerId = i;
		}
	}

}
