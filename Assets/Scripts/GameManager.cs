using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System;
using State;

public class GameManager : MonoBehaviour {
	public static GameManager Instance { get; private set; }
	public float completionWaitTime = 5;
	[SerializeField] PlayableDirector Segment1Cam;
	[SerializeField] PlayableDirector Segment2Cam;
	[SerializeField] PlayableDirector Segment3Cam;
	[SerializeField] GameObject[] CompleteCams;
	[SerializeField] GameObject[] Segment0Objects;
	[SerializeField] GameObject[] Segment1Objects;
	[SerializeField] GameObject[] Segment2Objects;
	[SerializeField] GameObject[] Segment3Objects;

	public GameState State = new GameState();

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
		Events.OnPlayerWin += Events_OnPlayerWin;
		Events.OnRespawn += Events_OnRespawn;
	}

	void Unsubscribe() {
		Events.OnPlayerJoin -= Events_OnPlayerJoin;
		Events.OnPlayerDeath -= Events_OnPlayerDeath;
		Events.OnPlayerWin -= Events_OnPlayerWin;
		Events.OnRespawn -= Events_OnRespawn;
	}

	void Start() {
		StartCoroutine(Reset());
	}

	private void Update() {
		if (Input.GetKeyDown("`")) { State.Segment = 0; StartCoroutine(Reset()); }
		if (Input.GetKeyDown("1")) { State.Segment = 1; StartCoroutine(Reset()); }
		if (Input.GetKeyDown("2")) { State.Segment = 2; StartCoroutine(Reset()); }
		if (Input.GetKeyDown("3")) { State.Segment = 3; StartCoroutine(Reset()); }
	}

	void Events_OnPlayerJoin(int playerId) {
		State.Players.Add(new Player() {
			Id = playerId,
			IsAlive = false,
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

	void Events_OnPlayerWin(int playerId) {
		State.Players[playerId].HasWon = true;

		foreach (var player in State.Players) {
			if (player.GameObject.activeSelf) { return; }
		}
		if(State.Segment <= 3) {
			StartCoroutine(CompleteCam());
		} else {
			//you win
		}
	}

	IEnumerator CompleteCam() {
		CompleteCams[State.Segment-1].SetActive(true);
		yield return new WaitForSeconds(completionWaitTime);
		CompleteCams[State.Segment - 1].SetActive(false);
		State.Segment++;
		StartCoroutine(Reset());
	}

	public IEnumerator Reset() {
		//play SFX? add some kind of explosion or smoke poof? death animation
		switch (State.Segment) { 
			case 3:
				foreach (GameObject obj in Segment0Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment1Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment2Objects) { obj.SetActive(false); }

				foreach (GameObject obj in Segment3Objects) { obj.SetActive(true); }
				Segment3Cam.Stop();
				Segment3Cam.Play();
				yield return new WaitForSeconds(.5f);
				Events.Respawn();
				break;
			case 2:
				foreach (GameObject obj in Segment0Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment1Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment3Objects) { obj.SetActive(false); }

				foreach (GameObject obj in Segment2Objects) { obj.SetActive(true); }
				Segment2Cam.Stop();
				Segment2Cam.Play();
				yield return new WaitForSeconds(.5f);
				Events.Respawn();
				break;
			case 1:
				foreach (GameObject obj in Segment0Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment2Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment3Objects) { obj.SetActive(false); }


				foreach (GameObject obj in Segment1Objects) { obj.SetActive(true); }
				Segment1Cam.Stop();
				Segment1Cam.Play();
				yield return new WaitForSeconds(.5f);
				Events.Respawn();
				break;
			default:
				foreach (GameObject obj in Segment1Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment2Objects) { obj.SetActive(false); }
				foreach (GameObject obj in Segment3Objects) { obj.SetActive(false); }


				foreach (GameObject obj in Segment0Objects) { obj.SetActive(true); }
				yield return new WaitForSeconds(.5f);
				Events.Respawn();
				break;

		}
	}
}
