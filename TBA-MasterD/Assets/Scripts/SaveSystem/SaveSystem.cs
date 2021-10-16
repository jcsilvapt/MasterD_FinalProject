using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem {

    public SaveProfile profile;

    public void Save(SO_PlayerData data) {
        if (profile == null) {
            profile = new SaveProfile();
        }
        // Save Player Position data
        profile.transform_x         = data.currentPosition.x;
        profile.transform_y         = data.currentPosition.y;
        profile.transform_z         = data.currentPosition.z;
        // Save Player Rotation data
        profile.rotation_x          = data.currentRotation.x;
        profile.rotation_y          = data.currentRotation.y;
        profile.rotation_z          = data.currentRotation.z;
        // Save Player Has Drone
        profile.hasDrone            = data.hasDrone;
        // Save Player Has Weapon
        profile.hasWeapon           = data.hasWeapon;
        // Save Player currentScene
        profile.currentScene        = data.currentScene;
        // Save Player Inputs
        profile.walkFoward          = (int)data.walkFoward;
        profile.walkBackwards       = (int)data.walkBackwards;
        profile.walkLeft            = (int)data.walkLeft;
        profile.walkRight           = (int)data.walkRight;
        profile.crouch              = (int)data.crouch;
        profile.jump                = (int)data.jump;
        profile.sprint              = (int)data.sprint;
        profile.droneActivation     = (int)data.droneActivation;
        profile.droneMoveUp         = (int)data.droneMoveUp;
        profile.droneMoveDown       = (int)data.droneMoveDown;
        profile.interaction         = (int)data.interaction;
        profile.escape              = (int)data.escape;
        profile.mouseSensitivity    = data.mouseSensitivity;
        profile.invertMouse         = data.invertMouse;

        string savePath = Application.persistentDataPath + "/save.fjt";

        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(savePath)) {
            File.Delete(savePath);
        }

        FileStream fs = File.Open(savePath, FileMode.OpenOrCreate);
        bf.Serialize(fs, profile);

        fs.Close();
    }

    public SO_PlayerData Load(SO_PlayerData data) {
        string loadPath = Application.persistentDataPath + "/save.fjt";

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Open(loadPath, FileMode.Open);

        profile = bf.Deserialize(fs) as SaveProfile;

        // Load Player Position data
        data.currentPosition.x      = profile.transform_x;
        data.currentPosition.y      = profile.transform_y;
        data.currentPosition.z      = profile.transform_z;
        // Load Player Rotation data
        data.currentRotation.x      = profile.rotation_x;
        data.currentRotation.y      = profile.rotation_y;
        data.currentRotation.z      = profile.rotation_z;
        // Load Player Has Drone
        data.hasDrone               = profile.hasDrone;
        // Load Player Has Weapon
        data.hasWeapon              = profile.hasWeapon;
        // Load Player currentScene
        data.currentScene           = profile.currentScene;
        // Load Player Inputs
        data.walkFoward             = (KeyCode)profile.walkFoward;
        data.walkBackwards          = (KeyCode)profile.walkBackwards;
        data.walkLeft               = (KeyCode)profile.walkLeft;
        data.walkRight              = (KeyCode)profile.walkRight;
        data.crouch                 = (KeyCode)profile.crouch;
        data.jump                   = (KeyCode)profile.jump;
        data.sprint                 = (KeyCode)profile.sprint;
        data.droneActivation        = (KeyCode)profile.droneActivation;
        data.droneMoveUp            = (KeyCode)profile.droneMoveUp;
        data.droneMoveDown          = (KeyCode)profile.droneMoveDown;
        data.interaction            = (KeyCode)profile.interaction;
        data.escape                 = (KeyCode)profile.escape;
        data.mouseSensitivity       = profile.mouseSensitivity;
        data.invertMouse            = profile.invertMouse;

        fs.Close();

        return data;
    }

    public bool HasSavedData() {
        string path = Application.persistentDataPath + "/save.fjt";

        return File.Exists(path);
    }

}
