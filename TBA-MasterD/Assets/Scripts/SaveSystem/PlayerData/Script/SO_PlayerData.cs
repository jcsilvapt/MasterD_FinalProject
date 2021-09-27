using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Data", menuName ="Game/Create Player Data")]
[System.Serializable]
public class SO_PlayerData : ScriptableObject {

    [Header("Current Status")]
    public int currentScene;
    public Vector3 currentPosition;

    // Adicionar checkpoints index
    // Adicionar se tem arma e o drone...
    // adicionar inimigos que coiso...

}
