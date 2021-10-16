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

    public int walkFoward;
    public int walkBackwards;
    public int walkLeft;
    public int walkRight;
    public int crouch;
    public int jump;
    public int sprint;
    public int droneActivation;
    public int droneMoveUp;
    public int droneMoveDown;
    public int interaction;
    public int escape;

    public float mouseSensitivity;
    public bool invertMouse;


}
