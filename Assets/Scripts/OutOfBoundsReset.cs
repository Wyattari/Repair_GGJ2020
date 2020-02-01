using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsReset : MonoBehaviour
{
    [SerializeField] LayerMask outOfBoundsLayer;

    private void OnTriggerEnter(Collider other) {
        
    }
    private void OnCollisionEnter(Collision collision) {
        if ((outOfBoundsLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer) {
            StartCoroutine(GameManager.Instance.Reset());
        }
    }
 
}
