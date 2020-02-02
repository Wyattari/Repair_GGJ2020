using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioController : MonoBehaviour {

	[EventRef] public string Fire;
	[EventRef] public string Hit;
	[EventRef] public string Jump;
	[EventRef] public string Land;

	EventInstance fire;
	EventInstance hit;
	EventInstance jump;
	EventInstance land;

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
		GameManager.Instance.Events.OnPlayerFire += Events_OnPlayerFire;
	}

	void Unsubscribe() {
		GameManager.Instance.Events.OnPlayerFire -= Events_OnPlayerFire;
	}

	void Events_OnPlayerFire(int playerId) {
		Debug.Log("PLAYING FIRE SOUND KAMAN");
		fire.start();
	}
}
