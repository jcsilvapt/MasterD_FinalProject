using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Data", menuName ="Game/Create Player Data")]
[System.Serializable]
public class SO_PlayerData : ScriptableObject {

    [Header("Current Status")]
    public int currentScene;
    public Vector3 currentPosition;
    public Vector3 currentRotation;
    public bool hasDrone;
    public bool hasWeapon;

    // adicionar inimigos que coiso...

}
