using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroductionUIManager : MonoBehaviour
{
    #region References

    private TextMeshProUGUI roomName;

    #endregion

    #region Region Identifier

    public enum Region
    {
        Labs,
        WC,
        Lab20,
        Lab21,
        ControlRoom,
        Cafeteria,
        Lab22,
        ElevatorControlRoom
    }

    private Region lastRegionPresented;

    #endregion

    #region Control Variables

    private bool isShowing;

    [SerializeField] private float durationTheNameIsDisplayed;

    private float currentTimerWhenNameShowedUp;

    #region Transition Controller

    private bool isTransitioning;

    [SerializeField] private float durationWhenNameShowedUp;

    private float timestampTheNameIsDisplayed;

    #endregion

    #endregion

    private void Awake()
    {
        roomName = GetComponent<TextMeshProUGUI>();
        lastRegionPresented = Region.ElevatorControlRoom;
        //Quando o jogo começa, esta variável vai ter o último valor do mapa. Assim, a primeira Region vai ser mostrada garantidamente.

        roomName.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isShowing)
        {
            return;
        }

        currentTimerWhenNameShowedUp += Time.deltaTime;

        if(!isTransitioning && currentTimerWhenNameShowedUp >= durationWhenNameShowedUp)
        {
            isTransitioning = true;
            timestampTheNameIsDisplayed = Time.time;
        }

        if (isTransitioning)
        {
            Transition();
        }
    }

    public void ShowText(Region currentRegion)
    {
        if(currentRegion == lastRegionPresented)
        {
            return;
        }

        SetName(currentRegion);
        ResetVariables();

        isShowing = true;
        
        roomName.gameObject.SetActive(true);

        lastRegionPresented = currentRegion;
    }

    private void Transition()
    {
        float timeSinceStarted = Time.time - timestampTheNameIsDisplayed;
        float percentangeComplete = timeSinceStarted / durationTheNameIsDisplayed;

        roomName.color = new Color(1, 0.8493133f, 0, Mathf.Lerp(1, 0, percentangeComplete));
        roomName.faceColor = new Color(8, 8, 8, Mathf.Lerp(1, 0, percentangeComplete));
        roomName.outlineColor = new Color(0, 0, 0, Mathf.Lerp(1, 0, percentangeComplete));
        roomName.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color(766.9961f, 546.1334f, 0, Mathf.Lerp(1, 0, percentangeComplete)));

        if (percentangeComplete >= 1)
        {
            ResetVariables();
            roomName.gameObject.SetActive(false);
        }
    }

    private void ResetVariables()
    {
        isShowing = false;
        isTransitioning = false;
        currentTimerWhenNameShowedUp = 0;
        timestampTheNameIsDisplayed = 0;

        roomName.color = new Color(1, 0.8493133f, 0, 1);
        roomName.faceColor = new Color(8, 8, 8, 1);
        roomName.outlineColor = new Color(0, 0, 0, 1);
        roomName.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color(766.9961f, 546.1334f, 0, 1));

    }

    private void SetName(Region currentRegion)
    {
        switch (currentRegion)
        {
            case Region.Labs:
                roomName.text = "Labs";
                break;
            case Region.WC:
                roomName.text = "WC";
                break;
            case Region.Lab20:
                roomName.text = "Lab 20";
                break;
            case Region.Lab21:
                roomName.text = "Lab 21";
                break;
            case Region.ControlRoom:
                roomName.text = "Control Room";
                break;
            case Region.Cafeteria:
                roomName.text = "Cafeteria";
                break;
            case Region.Lab22:
                roomName.text = "Lab 22";
                break;
            case Region.ElevatorControlRoom:
                roomName.text = "Elevator Control Room";
                break;
            default:
                break;
        }
    }
}
