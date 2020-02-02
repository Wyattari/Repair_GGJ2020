using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsReset : MonoBehaviour
{
    [SerializeField] LayerMask outOfBoundsLayer;
    private void OnCollisionEnter(Collision collision) {
        if ((outOfBoundsLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer) {
            foreach (var player in GameManager.Instance.State.Players) {
				if (player.GameObject == gameObject) {
                    GameManager.Instance.Events.PlayerDeath(player.Id);
				}
			}
            Destroy(gameObject);
        }
    }
 
}
