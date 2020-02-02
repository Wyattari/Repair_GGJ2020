using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Targeting : MonoBehaviour {
	[SerializeField] RectTransform reticle;
	[SerializeField] RectTransform screenCanvas;
	Vector2 startPosition;
	[SerializeField] float sensitivity = 0.2f;
	Camera mainCam;

	Color[] playerColors = new Color[] {
		Color.magenta,
		Color.cyan,
		Color.yellow,
		Color.green
	};

	int playerId;
	public int PlayerId {
		get { return playerId; }
		set {
			playerId = value;
			reticle.GetComponent<Image>().color = playerColors[value];
		}
	}

	private Vector2 look;

	private void Start() {
		mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
		Subscribe();
	}

	void OnDestroy() {
		Unsubscribe();		
	}

	void Unsubscribe() {
		GameManager.Instance.Events.OnPlayerAim -= Events_OnPlayerAim;
		GameManager.Instance.Events.OnPlayerFire -= Events_OnPlayerFire;
	}

	void Subscribe() {
		Unsubscribe();
		GameManager.Instance.Events.OnPlayerAim += Events_OnPlayerAim;
		GameManager.Instance.Events.OnPlayerFire += Events_OnPlayerFire;
	}

	void Events_OnPlayerAim(int playerId, Vector2 vector) {
		if (playerId != PlayerId) { return; }
		look = vector;	
	}

	void Events_OnPlayerFire(int playerId) {
		if (playerId != PlayerId) { return; }

		var canv = screenCanvas.GetComponent<RectTransform>();

		var screen_pos = reticle.localPosition - new Vector3(canv.rect.min.x, canv.rect.min.y, 0);

		var screen_ray = Camera.main.ScreenPointToRay(screen_pos);

		RaycastHit hit;
		bool collided = Physics.Raycast(screen_ray.origin, screen_ray.direction, out hit);
		Debug.Log("Reticle: " + screen_pos);
		if (collided) {
			var parent = hit.collider.transform.parent;
            while(parent)
            {
			    if (parent.GetComponent<RockRotator>()) {
				    var rock_rotator = parent.GetComponent<RockRotator>();
				    rock_rotator.HoldRocks();
			    }
                parent = parent.transform.parent;
            }
		}
	}

	void Update() {
		Look(look);
	}


	private void Look(Vector2 looking) {
		Vector3 newPos = new Vector3(looking.x, looking.y, 0);
		reticle.localPosition += newPos * sensitivity;
		reticle.localPosition = KeepFullyOnScreen(reticle.localPosition);
		// need to add https://docs.unity3d.com/ScriptReference/Camera.ViewportPointToRay.html --- but need to normalize the reticle position D:

	}

	Vector3 KeepFullyOnScreen(Vector3 newPos) {
		var ret = reticle.GetComponent<RectTransform>();
		var canv = screenCanvas.GetComponent<RectTransform>();

		float minX = (screenCanvas.sizeDelta.x - reticle.sizeDelta.x) * -0.5f;
		float maxX = (screenCanvas.sizeDelta.x - reticle.sizeDelta.x) * 0.5f;
		float minY = (screenCanvas.sizeDelta.y - reticle.sizeDelta.y) * -0.5f;
		float maxY = (screenCanvas.sizeDelta.y - reticle.sizeDelta.y) * 0.5f;

		newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
		newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

		return newPos;
	}
}
