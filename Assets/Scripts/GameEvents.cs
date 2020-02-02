using System;
using UnityEngine;

public class GameEvents {
	public Action<int, Vector2> OnPlayerMove;
	public Action<int, Vector2> OnPlayerAim;
	public Action<int> OnPlayerFire;
	public Action<int> OnPlayerJump;

	public void PlayerMove(int id, Vector2 vector) {
		OnPlayerMove?.Invoke(id, vector);
	}

	public void PlayerAim(int id, Vector2 vector) {
		OnPlayerAim?.Invoke(id, vector);
	}

	public void PlayerFire(int id) {
		OnPlayerFire?.Invoke(id);
	}

	public void PlayerJump(int id) {
		OnPlayerJump?.Invoke(id);
	}
}