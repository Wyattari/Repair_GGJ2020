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
    GameObject camera;
    Camera mainCam;

    private Vector2 m_Look;

    private void Start() {
        camera = GameObject.Find("cm");
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    void OnLook(InputValue value) {
        m_Look = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        Look(m_Look);
    }

    void OnShoot()
    {
        var canv = screenCanvas.GetComponent<RectTransform>();

        var screen_pos = reticle.localPosition - new Vector3(canv.rect.min.x, canv.rect.min.y, 0);

        var screen_ray = Camera.main.ScreenPointToRay(screen_pos);

        RaycastHit hit;
        bool collided = Physics.Raycast(screen_ray.origin, screen_ray.direction, out hit);
        Debug.Log("Reticle: " + screen_pos);
        if(collided)
        {
            var parent = hit.collider.transform.parent;
            if(parent && parent.GetComponent<RockRotator>())
            {
                var rock_rotator = parent.GetComponent<RockRotator>();
                rock_rotator.HoldRocks();
                Debug.Log("Hit");
            }
            else
            {
                Debug.Log("Miss");
            }
        }
    }


    private void Look(Vector2 looking) {
        Vector3 newPos = new Vector3(looking.x, looking.y, 0);
        reticle.localPosition += newPos*sensitivity;
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
