using UnityEngine;

public class KeyMapper : MonoBehaviour {

    public static KeyMapper inputKey;

    #region MOUSE

    [Header("Mouse Settings")]
    [Range(0.0f, 10f)]
    [SerializeField] float mouseSensitivity = 1f;

    public float MouseSensitivity {
        get { return mouseSensitivity; }
        set { mouseSensitivity = value; }
    } 

    [SerializeField] bool invertMouse;
    public bool InvertMouse {
        get { return invertMouse; }
        set { invertMouse = value; }
    }

    #endregion

    #region KEY MAPPING

    [Header("Movement")]
    [SerializeField] KeyCode walkFoward;
    public KeyCode WalkFoward {
        get { return walkFoward; }
        set { walkFoward = value; }
    }

    [SerializeField] KeyCode walkBackwards;
    public KeyCode WalkBackwards {
        get { return walkBackwards; }
        set { walkBackwards = value; }
    }

    [SerializeField] KeyCode walkLeft;
    public KeyCode WalkLeft {
        get { return walkLeft; }
        set { walkLeft = value; }
    }

    [SerializeField] KeyCode walkRight;
    public KeyCode WalkRight {
        get { return walkRight; }
        set { walkRight = value; }
    }

    [SerializeField] KeyCode crouch;
    public KeyCode Crouch {
        get { return crouch; }
        set { crouch = value; }
    }

    [SerializeField] KeyCode jump;
    public KeyCode Jump {
        get { return jump; }
        set { jump = value; }
    }

    [SerializeField] KeyCode sprint;
    public KeyCode Sprint {
        get { return sprint; }
        set { sprint = value; }
    }

    [SerializeField] KeyCode save;
    public KeyCode Save {
        get { return save; }
        set { save = value; }
    }

    [SerializeField] KeyCode load;
    public KeyCode Load {
        get { return load; }
        set { save = value; }
    }

    [Header("Drone")]
    [SerializeField] KeyCode droneActivation;
    public KeyCode DroneActivation {
        get { return droneActivation; }
        set { droneActivation = value; }
    }

    [SerializeField] KeyCode droneMoveUp;
    public KeyCode DroneMoveUp {
        get { return droneMoveUp; }
        set { droneMoveUp = value; }
    }

    [SerializeField] KeyCode droneMoveDown;
    public KeyCode DroneMoveDown {
        get { return droneMoveDown; }
        set { droneMoveDown = value; }
    }

    [Header("Interaction")]
    [SerializeField] KeyCode interaction;
    public KeyCode Interaction {
        get { return interaction; }
        set { interaction = value; }
    }

    [SerializeField] KeyCode escape;
    public KeyCode Escape
    {
        get { return escape; }
        set { escape = value; }
    }

    #endregion

    private void Awake() {
        if (inputKey == null) {
            inputKey = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }

}
