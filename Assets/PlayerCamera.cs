using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    Vector3 distance;
    float cameraSpeed = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(42.271f, 0, 0);
        distance = new Vector3(-0.81f, 9.55f, -7.06f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position+distance, Time.deltaTime*cameraSpeed);
    }
}
