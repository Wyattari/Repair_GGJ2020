using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    [SerializeField] LayerMask checkPointLayer;
    private void OnCollisionEnter(Collision collision) {
        if ((checkPointLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer) {
            foreach (var player in GameManager.Instance.State.Players) {
                if (player.GameObject == gameObject) {
                    gameObject.SetActive(false);
                    GameManager.Instance.Events.PlayerWin(player.Id);
                }
            }
        }
    }
}
