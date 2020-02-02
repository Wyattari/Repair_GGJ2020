using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : BaseBehaviour {
	int playerId;
	public GameObject LaserPrefab;
	public GameObject BallPrefab;
	GameObject laser;
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

	void OnDisable()
	{
		Unsubscribe();
		ball.SetActive(false);
		laser.SetActive(false);
	}

	void Subscribe() {
		Unsubscribe();
		events.OnHit += Events_OnHit;
		events.OnPlayerFireRelease += Events_OnPlayerFireRelease;
	}

	void Unsubscribe() {
		events.OnHit -= Events_OnHit;
		events.OnPlayerFireRelease -= Events_OnPlayerFireRelease;
	}

	void Events_OnHit(int playerId, Vector3 hitPosition) {
		if (playerId != PlayerId) { return; }
		if(laser != null)
		{
			laserBeamManager.ShootBeams(ball.transform.position, hitPosition);
		}
	}

	void Events_OnPlayerFireRelease(int playerId) {
		if (playerId != PlayerId) { return; }
		if (laser == null) { return; }
		Debug.Log("Stopping Beams");
		StartCoroutine(StopBeams());
	}

	IEnumerator StopBeams() {
		yield return new WaitForSeconds(0.25f);
		laserBeamManager.StopBeams();
	}

	void Update() {
		laserBeamManager.SetStartPositions(ball.transform.position);
		ball.transform.position = transform.position;
	}

	void OnDestroy() {
		Unsubscribe();
		Destroy(laser);
		Destroy(ball);
	}
}
