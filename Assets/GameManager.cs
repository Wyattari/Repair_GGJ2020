using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] GameObject player;
    [SerializeField] Transform[] playerSpawns;

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

        for (int i = 0; i < playerSpawns.Length; i++) {
            spawnedPlayers[i] = Instantiate(player, playerSpawns[i]);
        }

        yield return null;
    }

}
