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

    private void Start()
    {
        introductionManager = GameObject.Find("RoomName").GetComponent<IntroductionUIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
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
