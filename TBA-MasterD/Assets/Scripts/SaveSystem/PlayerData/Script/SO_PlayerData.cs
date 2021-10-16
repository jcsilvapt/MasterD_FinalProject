using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Data", menuName ="Game/Create Player Data")]
[System.Serializable]
public class SO_PlayerData : ScriptableObject {

    [Header("Current Player Status")]
    public int currentScene;
    public Vector3 currentPosition;
    public Vector3 currentRotation;
    public bool hasDrone;
    public bool hasWeapon;

    [Header("Input Settings")]
    public KeyCode walkFoward;
    public KeyCode walkBackwards;
    public KeyCode walkLeft;
    public KeyCode walkRight;
    public KeyCode crouch;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode droneActivation;
    public KeyCode droneMoveUp;
    public KeyCode droneMoveDown;
    public KeyCode interaction;
    public KeyCode escape;

    [Header("Mouse Settings")]
    public float mouseSensitivity;
    public bool invertMouse;
}
