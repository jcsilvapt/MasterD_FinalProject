using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface created to give easly acess to the SceneController Methods
/// </summary>
public interface ISceneControl {

    List<GameObject> GetObjects();

    bool GetTypeOfSwitch();

    bool GetIsToExecute();

    void ToogleTypeOfSwitch();

    void ToogleExecute();

}
