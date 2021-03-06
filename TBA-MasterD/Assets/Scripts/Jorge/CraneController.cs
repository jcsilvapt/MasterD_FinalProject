using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour, IDamage {

    [Tooltip("Add Here all the moving parts objects in the scene.")]
    [SerializeField] GameObject[] movingParts;
    [Tooltip("Add the GameObjects Tag that are allowed to use this.")]
    [SerializeField] List<string> allowedTagsToInteract = new List<string> { "Player", "PlayerParent" };

    [SerializeField] bool interactionWithBullet = false;
    [SerializeField] bool isActive = false;

    [SerializeField] KeyCode keyToInteract;

    [SerializeField] Modal modalObjective;

    private charController player = null;
    private bool hasBeenInteracted = false;

    [Header("Change Material Settings")]
    [SerializeField] private bool hasMaterialChanges;
    private Renderer meshRenderer;
    [SerializeField] private float durationBetweenMaterialChanges;
    private float timerBetweenMaterialChanges;
    [SerializeField] private Material[] materials;
    private Material[] rendererMaterials;

    private void Start() {
        if (!isActive) {
            ShutDownCrane();
        } else {
            ActivateCrane();
        }

        if (hasMaterialChanges)
        {
            meshRenderer = transform.GetChild(1).GetComponent<Renderer>();
            timerBetweenMaterialChanges = durationBetweenMaterialChanges;

            rendererMaterials = meshRenderer.materials;
        }
    }


    private void Update() {
        
        if (hasBeenInteracted)
        {
            return;
        }

        if(!hasBeenInteracted && player != null) {
            if (Input.GetKeyDown(keyToInteract)) {
                ActivateCrane();
                player.EnableInteractionUI(false);
                hasBeenInteracted = true;

                if (hasMaterialChanges)
                {
                    SetRendererMaterials(materials[2]);
                }
            }
        }

        if (hasMaterialChanges)
        {
            timerBetweenMaterialChanges -= Time.deltaTime;

            if (timerBetweenMaterialChanges <= 0 && !hasBeenInteracted)
            {
                if (meshRenderer.sharedMaterials[1].name == "eye1 (Instance)" || meshRenderer.sharedMaterials[1].name == "eye1")
                {
                    SetRendererMaterials(materials[1]);
                }
                else if (meshRenderer.sharedMaterials[1].name == "DoorLocked")
                {
                    SetRendererMaterials(materials[0]);
                }

                timerBetweenMaterialChanges = durationBetweenMaterialChanges;
            }
        }
    }

    private void ActivateCrane() {
        foreach (GameObject g in movingParts) {
            Animator b = g.GetComponent<Animator>();
            b.enabled = true;
        }
        if(modalObjective != null) {
            modalObjective.ShowModal();
        }
        isActive = true;
    }

    private void ShutDownCrane() {
        foreach (GameObject g in movingParts) {
            Animator b = g.GetComponent<Animator>();
            b.enabled = false;
        }
        isActive = false;
    }

    private void OnTriggerStay(Collider other) {
        if (!isActive) {
            if (allowedTagsToInteract.Contains(other.tag)) {
                player = other.transform.parent.parent.GetComponent<charController>();
                player.EnableInteractionUI(true);
            }
        }

    }

    private void OnTriggerExit(Collider other) {
        if (!isActive) {
            if (allowedTagsToInteract.Contains(other.tag)) {
                player.EnableInteractionUI(false);
                player = null;
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (interactionWithBullet) {
            if (!isActive) {
                foreach (string tag in allowedTagsToInteract) {
                    if (other.gameObject.tag == tag) {
                        ActivateCrane();
                    }
                }
            }
        }
    }

    public void TakeDamage() {
        ActivateCrane();
    }

    private void SetRendererMaterials(Material toBeChanged)
    {
        rendererMaterials[1] = toBeChanged;
        meshRenderer.materials = rendererMaterials;
    }
}
