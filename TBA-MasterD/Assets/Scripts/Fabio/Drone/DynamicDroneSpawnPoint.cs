using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDroneSpawnPoint : MonoBehaviour
{
    #region References

    [SerializeField] private Transform standardSpawnPosition;
    [SerializeField] private Transform closeSpawnPosition;

    #endregion

    #region Control Variables

    private float spawnPositionLerp;

    #endregion

    private void Update()
    {
        Debug.DrawLine(closeSpawnPosition.position, standardSpawnPosition.position, Color.black);
        if (Physics.Linecast(closeSpawnPosition.position, standardSpawnPosition.position, out RaycastHit hit)){
            spawnPositionLerp = 0;
        }
        else
        {
            spawnPositionLerp = 1;
        }

        transform.position = Vector3.Lerp(closeSpawnPosition.position, standardSpawnPosition.position, spawnPositionLerp);
    }
}
