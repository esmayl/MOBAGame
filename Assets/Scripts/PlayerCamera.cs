using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(PlayerInput))]
public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    Vector3 distance;
    float cameraSpeed = 20f;
    float zoom = 1;
    Vector3 zoomedDistance;

    void Start()
    {

        InputAction scrollAction = InputHandler.instance.playerInputs.actions.FindAction("Zoom");
        scrollAction.performed += SetZoom;

        transform.eulerAngles = new Vector3(50, 0, 0);
        distance = new Vector3(-0.81f, 9.55f, -7.06f);
        zoomedDistance = distance;

    }

    void Update()
    {
        //transform.eulerAngles = new Vector3(50/zoomedDistance.y, 0, 0);

        transform.position = Vector3.Lerp(transform.position, playerTransform.position + zoomedDistance, Time.deltaTime * cameraSpeed);
    }

    public void SetZoom(InputAction.CallbackContext context)
    {
        zoom -= context.ReadValue<Vector2>().y * 0.0025f;
        zoomedDistance = distance;
        zoomedDistance.y *= zoom;
        zoomedDistance.y = Mathf.Clamp(zoomedDistance.y, distance.y, distance.y + 5);

    }
}
