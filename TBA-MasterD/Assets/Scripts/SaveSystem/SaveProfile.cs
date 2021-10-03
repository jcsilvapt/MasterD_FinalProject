using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveProfile {

    public int currentScene;
    public float transform_x, transform_y, transform_z;
    public float rotation_x, rotation_y, rotation_z;
    public bool hasDrone;
    public bool hasWeapon;

}
