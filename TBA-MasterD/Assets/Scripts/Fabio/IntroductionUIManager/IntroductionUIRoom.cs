using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionUIRoom : MonoBehaviour
{
    #region References

    private IntroductionUIManager introductionManager;

    [SerializeField] IntroductionUIManager.Region regionName;

    private bool alreadyIdentified;

    [SerializeField] private bool turnOffAfterIdentifier;

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){

            if (introductionManager == null)
            {
                introductionManager = other.transform.parent.parent.GetChild(3).GetChild(2).Find("RoomName").GetComponent<IntroductionUIManager>();
            }

            if (!alreadyIdentified)
            {
                introductionManager.ShowText(regionName);
                alreadyIdentified = true;
            }
            else
            {
                if (!turnOffAfterIdentifier)
                {
                    introductionManager.ShowText(regionName);
                }
            }
        }
    }
}
