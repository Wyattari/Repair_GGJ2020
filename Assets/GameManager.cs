using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] GameObject player;
    [SerializeField] Transform[] playerSpawns;
    [SerializeField] PlayableDirector mainCamera;

    GameObject[] spawnedPlayers = new GameObject[4];

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start()
    {

        for (int i = 0; i < playerSpawns.Length; i++) {
            spawnedPlayers[i] = Instantiate(player, playerSpawns[i]);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Reset() {
        //play SFX
        foreach (GameObject player in spawnedPlayers) {
            Destroy(player); // add some kind of explosion or smoke poof? death animation
        }

        //rewind camera
        mainCamera.Stop();
        mainCamera.time = 0;

        for (int i = 0; i < playerSpawns.Length; i++) {
            spawnedPlayers[i] = Instantiate(player, playerSpawns[i]);
        }

        mainCamera.Play();

        yield return null;
    }

}
