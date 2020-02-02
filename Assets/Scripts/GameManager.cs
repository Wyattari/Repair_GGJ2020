using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] GameObject player;
    [SerializeField] PlayableDirector mainCamera;

    [NonSerialized] public GameEvents Events;
    [NonSerialized] public int PlayerCount = 0;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            Events = new GameEvents();
        }
        Subscribe();
    }

	void Subscribe() {
        Unsubscribe();
        Events.OnPlayerJoin += Events_OnPlayerJoin;
	}

	void Unsubscribe() {
        Events.OnPlayerJoin -= Events_OnPlayerJoin;
    }

    void Start(){
        StartCoroutine(Reset());
    }

	void Events_OnPlayerJoin(int playerId) {
        Debug.Log($"Resetting {playerId}");
        StartCoroutine(Reset());
	}

    public IEnumerator Reset() {
        //play SFX? add some kind of explosion or smoke poof? death animation

        //rewind camera
        mainCamera.Stop();
        mainCamera.Play();

        //wait for camera to get back to starting position
        yield return new WaitForSeconds(.5f);

        Debug.Log($"RESPAWNING");

        Events.Respawn();
    }
}
