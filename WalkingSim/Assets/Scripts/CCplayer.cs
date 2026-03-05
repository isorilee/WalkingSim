
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CCplayer : MonoBehaviour
{
    public float walkSpeed = 5;
    public float runSpeed = 9;

    public float jumpHeight = 5;
    public Transform cameraTransform;
    public float lookSensitivity = 1f;

    private CharacterController cc;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalVelocity; //current upward/downward speed 
    private float gravity = -20f; //constant downward acceleration 
    private float pitch; //up and down 

    //interaction varibales 
    private GameObject currentTarget;
    public Image reticleImage;
    private bool interactPressed;
    public static event Action<NPCData> OnDialogueRequested;
    //this is our event that the other scripts will be listening for public static event Action<NPCData> OnDialogueRequested;
    private Interactable currentInteractable;

    private bool isRunning;
    private bool isJumping;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //private void Start()
    //{
    //    reticleImage = GameObject.Find("Reticle").GetComponent<Image>();

    //}

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        //optional cursor locking 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //find the reticle 
        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(0, 0, 0, .7f); //slightly transparent black
    }

    // Update is called once per frame
    void Update()
    {
        //if (cameraTransform == null) return;
        HandleLook();
        HandleMovement();
        CheckInteract();
        HandleInteract(); 
        
    }

    private void HandleLook()
    {
        //horizontal mouse movement rotates player 
        float yaw = lookInput.x * lookSensitivity; 
        //vertical mouse movement rotates camera 
        float pitchDelta = lookInput.y * lookSensitivity;

        transform.Rotate(Vector3.up * yaw);

        //accumulate vertical rotation
        pitch -= pitchDelta;
        //clamp it so we dont flip upside down 
        pitch = Mathf.Clamp(pitch, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    private void HandleMovement()
    {
        //updating our bool to be true or false if the player is grounded 
        bool grounded = cc.isGrounded;
        Debug.Log("is grounded: " + grounded);

        //this keeps the cc snapped to the ground 
        if (grounded && verticalVelocity <= 0)
        {
            verticalVelocity = -2f;
        }

        float currentSpeed = walkSpeed; 

        //if running is true set the current speed to run speed 

        if (isRunning)
        {
            currentSpeed = runSpeed; 
        } //if it is false set it back to walk speed 

        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 move = transform.right * moveInput.x * currentSpeed + transform.forward * moveInput.y * currentSpeed; 

        //if jumping is true and we are grounded 

        if(isJumping && grounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }

        else
        {
            isJumping = false; 
        }
        //apply gravity to every frame 
        verticalVelocity += gravity * Time.deltaTime;

        //convert verticity into movement vector 
        Vector3 velocity = Vector3.up * verticalVelocity;

        //now we are finally moving our player 
        cc.Move((move + velocity) * Time.deltaTime); 

    }
    void CheckInteract()
    {
        //reset reticle image to normal color first 
        if (reticleImage != null) reticleImage.color = new Color(0, 0, 0, .7f);
        currentInteractable  = null;
        //make a ray that goes straight out of the camera (center of screen) 
        //players eyesight 
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        //asking unity if it hit something within 3 units 
        //hit stores what we hit like the collider 
        //bool didHit = Physics.Raycast(ray, out hit, 3);
        //if (!didHit) return;
        // if we didnt hit anything start here 
        //if we hit something tagged interactable 

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            //store the object so we can destroy or do whatever when the player clicks 

            currentInteractable = hit.collider.GetComponentInParent<Interactable>();
            Debug.Log("Current interactable: " + currentInteractable);
            if (currentInteractable != null && reticleImage != null)
            {
                reticleImage.color = Color.red;
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
            else

            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);

            }
        }
    }

    void HandleInteract()
    {
        //if the player did not press interact this frame do nothing 
        if (!interactPressed) return;
        //consume the input so one click only trigger one interactions 
        //this changes next frame 
        interactPressed = false;
        if (currentInteractable == null) return;
        //clear target reference after destroying
        currentInteractable.Interact(this);
    }

    public void OnMOve (InputAction.CallbackContext context) 
        {
            moveInput = context.ReadValue<Vector2>();
        }
    public void OnLook (InputAction.CallbackContext context)
    
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump (InputAction.CallbackContext context)
    {
        //if we are actually hitting the key is jumping equals true!
        if (context.performed) isJumping = true; 
    }

    public void OnSprint (InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton(); 
    }

    public void OnInteract (InputAction.CallbackContext context)
    {
        if (context.performed) interactPressed = true;
        Debug.Log("OnInteract fired. performed = " + context.performed);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log ("CC Colided with: " + hit.gameObject.name);  
    }

    public void RequestDialogue (NPCData npcData)
    {
        OnDialogueRequested?.Invoke(npcData);

    }

}



