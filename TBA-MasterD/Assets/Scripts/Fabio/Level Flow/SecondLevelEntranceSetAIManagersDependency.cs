using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLevelEntranceSetAIManagersDependency : MonoBehaviour
{
    #region References

    [SerializeField] private SceneSwitcher sceneSwitcher;

    #endregion

    #region

    private bool hasBeenActivated;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasBeenActivated)
            {
                sceneSwitcher = GameObject.Find("TriggerLocatedAtTestesFabio").GetComponent<SceneSwitcher>();
                sceneSwitcher.SetAIManagersArray();
                hasBeenActivated = true;
            }
        }
    }
}
