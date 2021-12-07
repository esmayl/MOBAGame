using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerInput playerInputs;
    public static InputHandler instance;

    void Awake()
    {
        instance = this;

        playerInputs = GetComponent<PlayerInput>();
    }
}
