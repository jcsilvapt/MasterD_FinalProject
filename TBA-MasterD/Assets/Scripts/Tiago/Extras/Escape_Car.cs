using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape_Car : MonoBehaviour
{
    //player and camera to switch for animation
    [SerializeField] GameObject player;
    [SerializeField] GameObject endCamera;

    //car animator and Garage Manager
    private Animator anim;
    public Garage_Manager gm;

    //Car effects and sounds
    [SerializeField] GameObject frontLights;
    [SerializeField] GameObject smoke;
    [SerializeField] GameObject tireEffect;
    private AudioSource audioS;
    [SerializeField] AudioClip turnOn;
    [SerializeField] AudioClip moveCar;
    [SerializeField] AudioClip thankYou;

    //music sources
    [SerializeField] AudioSource stealth;
    [SerializeField] AudioSource action;
    [SerializeField] AudioSource endMusic;

    //end screen
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject thankYouScreen;
    [SerializeField] GameObject credits;
    void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        gm = GameObject.FindGameObjectWithTag("Manager").GetComponent<Garage_Manager>();
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform.parent.parent.gameObject;
            player.GetComponent<charController>().EnableInteractionUI(true);
            if (Input.GetKeyDown(KeyMapper.inputKey.Interaction))
            {
                StartCoroutine(ThankYouScreen());
                if (gm.isStealth == false)
                {
                    gm.EndingEnemies();
                }
            }
        }
    }

    public void EngineTurnOnSound()
    {
        audioS.PlayOneShot(turnOn);

    }

    public void EngineSound()
    {
        audioS.PlayOneShot(moveCar);
        endMusic.Play();
    }

    public void TurnSmokeOn()
    {
        frontLights.SetActive(true);
        smoke.SetActive(true);
    }

    public void EndScreen()
    {
        endScreen.SetActive(true);
        audioS.Stop();
    }

     IEnumerator ThankYouScreen()
    {
        player.SetActive(false);
        stealth.Stop();
        action.Stop();
        endCamera.SetActive(true);        
        thankYouScreen.SetActive(true);
        audioS.PlayOneShot(thankYou);
        yield return new WaitForSeconds(4);
        thankYouScreen.SetActive(false);
        anim.SetTrigger("hasEscaped");
        yield return new WaitForSeconds(19);
        thankYouScreen.SetActive(false);
        credits.SetActive(true);
    }

    public void TiresSmoke()
    {
        tireEffect.SetActive(true);
    }
}
