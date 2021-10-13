using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETE_GLASSBREAKRESEARCH : MonoBehaviour
{
    private static DELETE_GLASSBREAKRESEARCH instance;

    public List<AudioSource> audios;

    public List<AudioSource> breakGlassAudios;

    private void Awake()
    {
        if(instance != null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            audios.Clear();
            breakGlassAudios.Clear();

            foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
            {
                if(obj.TryGetComponent(out AudioSource source))
                {
                    audios.Add(source);
                }
            }

            foreach (AudioSource source in audios)
            {
                if(source.clip.name == "glass break sound effect")
                {
                    breakGlassAudios.Add(source);
                }
            }
        }
    }
}
