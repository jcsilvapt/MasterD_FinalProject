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
        profile.transform_x = data.currentPosition.x;
        profile.transform_y = data.currentPosition.y;
        profile.transform_z = data.currentPosition.z;
        // Save Player Rotation data
        profile.rotation_x = data.currentRotation.x;
        profile.rotation_y = data.currentRotation.y;
        profile.rotation_z = data.currentRotation.z;
        // Save Player Has Drone
        profile.hasDrone = data.hasDrone;
        // Save Player Has Weapon
        profile.hasWeapon = data.hasWeapon;
        // Save Player currentScene
        profile.currentScene = data.currentScene;

        string savePath = Application.persistentDataPath + "/save.dat";

        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(savePath)) {
            File.Delete(savePath);
        }

        FileStream fs = File.Open(savePath, FileMode.OpenOrCreate);
        bf.Serialize(fs, profile);

        fs.Close();
    }

    public SO_PlayerData Load(SO_PlayerData data) {
        string loadPath = Application.persistentDataPath + "/save.dat";

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Open(loadPath, FileMode.Open);

        profile = bf.Deserialize(fs) as SaveProfile;

        //SO_PlayerData data = ScriptableObject.CreateInstance<SO_PlayerData>();

        // Load Player Position data
        profile.transform_x = data.currentPosition.x;
        profile.transform_y = data.currentPosition.y;
        profile.transform_z = data.currentPosition.z;
        // Load Player Rotation data
        profile.rotation_x = data.currentRotation.x;
        profile.rotation_y = data.currentRotation.y;
        profile.rotation_z = data.currentRotation.z;
        // Load Player Has Drone
        profile.hasDrone = data.hasDrone;
        // Load Player Has Weapon
        profile.hasWeapon = data.hasWeapon;
        // Load Player currentScene
        profile.currentScene = data.currentScene;

        fs.Close();

        return data;
    }

    public bool HasSavedData() {
        string path = Application.persistentDataPath + "/save.dat";

        return File.Exists(path);
    }

}
