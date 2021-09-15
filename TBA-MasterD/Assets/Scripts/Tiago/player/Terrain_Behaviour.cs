using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_Behaviour : MonoBehaviour
{

    [Header("Terrain Sounds")]  // audio clips with sounds for various types of terrain

    [SerializeField] AudioClip[] stoneClips;
    [SerializeField] AudioClip[] metalClips;
    [SerializeField] AudioClip[] woodClips;


    [Header("Sound Components")] // Components of the player used to change the sounds

    public AudioClip clip;
    [SerializeField] AudioSource audioSource;
    RaycastHit hit;
    Ray ray;
    charController charController;

    private void Start()
    {
        charController = GetComponent<charController>();
    }
    private void Step()
    {
        // função(CLIP) que muda o som do pe esquerdo e direito

        audioSource.PlayOneShot(clip);
    }
    private void Update()
    {
        CheckTerrain();
    }

    private void CheckTerrain()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.transform.gameObject.tag == "Wood")
            {
                clip = woodClips[Random.Range(0, woodClips.Length)];
            }
            else if (hit.transform.gameObject.tag == "Metal")
            {
                clip = metalClips[Random.Range(0, metalClips.Length)];
            }
            else
            {
                clip = stoneClips[Random.Range(0, stoneClips.Length)];
            }          
        }
    }
}
