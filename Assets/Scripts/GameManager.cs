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
    [SerializeField] Transform[] playerSpawns;
    [SerializeField] PlayableDirector mainCamera;

    [NonSerialized] public GameEvents Events;

    GameObject[] spawnedPlayers = new GameObject[4];

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            Events = new GameEvents();
        }
    }

    void Start(){
        for (int i = 0; i < playerSpawns.Length; i++) {
            spawnedPlayers[i] = Instantiate(player, playerSpawns[i]);
        }


    }

    public IEnumerator Reset() {
        //play SFX? add some kind of explosion or smoke poof? death animation

        //rewind camera
        mainCamera.Stop();
        mainCamera.Play();

        //wait for camera to get back to starting position
        yield return new WaitForSeconds(.5f);

        foreach (GameObject player in spawnedPlayers) {
            Destroy(player); 
        }

        for (int i = 0; i < playerSpawns.Length; i++) {
            spawnedPlayers[i] = Instantiate(player, playerSpawns[i]);
        }
        

    }

}
