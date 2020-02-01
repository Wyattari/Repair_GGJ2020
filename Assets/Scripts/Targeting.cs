using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Targeting : MonoBehaviour
{
    [SerializeField] RectTransform reticle;
    [SerializeField] RectTransform screenCanvas;
    Vector2 startPosition;
    [SerializeField] float sensitivity = 0.2f;

    private Vector2 m_Look;


    // Update is called once per frame
    void Update()
    {
        //m_Look.x = Input.GetAxis("RightHoriz");
        //m_Look.y = Input.GetAxis("RightVert");
        //Debug.Log(m_Look);
    }

    private void Look(Vector2 looking) {
        Debug.Log(looking);
        reticle.localPosition = looking;
        startPosition = reticle.position;
    }
}
