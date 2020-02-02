using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	[FMODUnity.EventRef] public string Fire;
	[FMODUnity.EventRef] public string Hit;
	[FMODUnity.EventRef] public string Jump;
	[FMODUnity.EventRef] public string Land;

	FMOD.Studio.EventInstance fire;
	FMOD.Studio.EventInstance hit;
	FMOD.Studio.EventInstance jump;
	FMOD.Studio.EventInstance land;

	// Start is called before the first frame update
	void Start() {
		Subscribe();
		fire = FMODUnity.RuntimeManager.CreateInstance(Fire);
		hit = FMODUnity.RuntimeManager.CreateInstance(Hit);
		jump = FMODUnity.RuntimeManager.CreateInstance(Jump);
		land = FMODUnity.RuntimeManager.CreateInstance(Land);
	}

	void Subscribe() {
		Unsubscribe();
		GameManager.Instance.Events.OnPlayerJump += Events_OnPlayerJump;
	}

	void Unsubscribe() {
		GameManager.Instance.Events.OnPlayerJump -= Events_OnPlayerJump;
	}

	void Events_OnPlayerJump(int playerId) {
		jump.start();
	}
}
