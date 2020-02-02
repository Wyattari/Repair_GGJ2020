using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioController : BaseBehaviour {

	[EventRef] public string Fire;
	[EventRef] public string Hit;
	[EventRef] public string Jump;
	[EventRef] public string Land;
	[EventRef] public string Respawn;
	[EventRef] public string Death;

	EventInstance fire;
	EventInstance hit;
	EventInstance jump;
	EventInstance land;
	EventInstance respawn;
	EventInstance death;

	public static AudioController Instance;

	// Start is called before the first frame update
	void Start() {
		if (Instance != null) { throw new System.Exception("Tried to create two AudioControllers"); }
		Instance = this;
		Subscribe();
		fire = RuntimeManager.CreateInstance(Fire);
		hit = RuntimeManager.CreateInstance(Hit);
		jump = RuntimeManager.CreateInstance(Jump);
		land = RuntimeManager.CreateInstance(Land);
		respawn = RuntimeManager.CreateInstance(Respawn);
		death = RuntimeManager.CreateInstance(Death);
	}

	public void PlayJump() {
		jump.start();
	}

	public void PlayLand() {
		land.start();
	}

	public void PlayHit() {
		fire.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		hit.start();
	}

	void Subscribe() {
		Unsubscribe();
		events.OnPlayerFire += Events_OnPlayerFire;
		events.OnPlayerDeath += Events_OnPlayerDeath;
		events.OnRespawn += Events_OnRespawn;
	}

	void Unsubscribe() {
		events.OnPlayerFire -= Events_OnPlayerFire;
		events.OnPlayerDeath -= Events_OnPlayerDeath;
		events.OnRespawn -= Events_OnRespawn;
	}

	void Events_OnPlayerFire(int playerId) {
		fire.start();
	}

	void Events_OnPlayerDeath(int playerId) {
		death.start();
	}

	void Events_OnRespawn() {
		respawn.start();
	}
}
