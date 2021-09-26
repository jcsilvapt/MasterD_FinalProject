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

        profile.x = data.currentPosition.x;
        profile.y = data.currentPosition.y;
        profile.z = data.currentPosition.z;
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

    public SO_PlayerData Load() {
        string loadPath = Application.persistentDataPath + "/save.dat";

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Open(loadPath, FileMode.Open);

        profile = bf.Deserialize(fs) as SaveProfile;

        SO_PlayerData data = ScriptableObject.CreateInstance<SO_PlayerData>();

        data.currentPosition.x = profile.x;
        data.currentPosition.y = profile.y;
        data.currentPosition.z = profile.z;
        data.currentScene = profile.currentScene;

        fs.Close();

        return data;
    }

    public bool HasSavedData() {
        string path = Application.persistentDataPath + "/save.dat";

        return File.Exists(path);
    }

}
