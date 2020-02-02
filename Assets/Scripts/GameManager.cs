using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System;
using State;

public class GameManager : MonoBehaviour {
	public static GameManager Instance { get; private set; }

	[SerializeField] PlayableDirector mainCamera;

	public GameState State = new GameState();
	Color[] playerColors = new Color[] {
		Color.magenta,
		Color.cyan,
		Color.yellow,
		Color.green
	};

	[NonSerialized] public GameEvents Events;
	public int PlayerCount {
		get { return State.Players.Count; }
	}

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this.gameObject);
		} else {
			Instance = this;
			Events = new GameEvents();
		}
		Subscribe();
	}

	void Subscribe() {
		Unsubscribe();
		Events.OnPlayerJoin += Events_OnPlayerJoin;
		Events.OnPlayerDeath += Events_OnPlayerDeath;
		Events.OnRespawn += Events_OnRespawn;
	}

	void Unsubscribe() {
		Events.OnPlayerJoin -= Events_OnPlayerJoin;
		Events.OnPlayerDeath -= Events_OnPlayerDeath;
		Events.OnRespawn -= Events_OnRespawn;
	}

	void Start() {
		StartCoroutine(Reset());
	}

	void Events_OnPlayerJoin(int playerId) {
		State.Players.Add(new Player() {
			Id = playerId,
			IsAlive = false,
			Color = playerColors[playerId],
			GameObject = null
		});
		StartCoroutine(Reset());
	}

	void Events_OnRespawn() {
		foreach (var player in State.Players) {
			player.IsAlive = true;
		}
	}

	void Events_OnPlayerDeath(int playerId) {
		State.Players[playerId].IsAlive = false;

		foreach (var player in State.Players) {
			if (player.IsAlive) { return; }
		}
		StartCoroutine(Reset());
	}

	public IEnumerator Reset() {
		//play SFX? add some kind of explosion or smoke poof? death animation

		//rewind camera
		mainCamera.Stop();
		mainCamera.Play();

		//wait for camera to get back to starting position
		yield return new WaitForSeconds(.5f);

		Events.Respawn();
	}
}
