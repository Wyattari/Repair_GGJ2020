using System;
using UnityEngine;

public class GameEvents {
	public Action OnRespawn;
	public Action<int> OnPlayerJoin;

	// player control
	public Action<int, Vector2> OnPlayerMove;
	public Action<int, Vector2> OnPlayerAim;
	public Action<int> OnPlayerFire;
	public Action<int> OnPlayerFireRelease;
	public Action<int> OnPlayerJump;

	// player events
	public Action<int, Vector3> OnHit;
	public Action<int> OnPlayerDeath;
	public Action<int> OnPlayerWin;

	public void Respawn() {
		OnRespawn?.Invoke();
	}

	public void PlayerJoin(int id) {
		OnPlayerJoin.Invoke(id);
	}

	public void PlayerMove(int id, Vector2 vector) {
		OnPlayerMove?.Invoke(id, vector);
	}

	public void PlayerAim(int id, Vector2 vector) {
		OnPlayerAim?.Invoke(id, vector);
	}

	public void PlayerFire(int id) {
		OnPlayerFire?.Invoke(id);
	}

	public void PlayerFireRelease(int id) {
		OnPlayerFireRelease?.Invoke(id);
	}

	public void PlayerJump(int id) {
		OnPlayerJump?.Invoke(id);
	}

	public void PlayerDeath(int id) {
		OnPlayerDeath?.Invoke(id);
	}

	public void PlayerWin(int id) {
		OnPlayerWin?.Invoke(id);
	}

	public void Hit(int playerId, Vector3 hitPoint) {
		OnHit?.Invoke(playerId, hitPoint);
	}

}