using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShadow : MonoBehaviour
{
    public GameObject SpriteShadow;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            SpriteShadow.transform.position = hit.point;
            SpriteShadow.transform.rotation = Quaternion.FromToRotation(transform.forward, hit.normal);

            // SpriteShado
        }
    }
}
