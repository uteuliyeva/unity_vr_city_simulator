using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

enum State{
    Walking,
    Selecting,
    Placing,
    Map
}

public class Movement : MonoBehaviour
{
    public GameObject playerCam;
    private CharacterController controller;
    public GameObject tablet;
    public GameObject mapCam;

    public GameObject playerGhost;

    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    private State playerState = State.Walking;

    private GameObject transPrefab;

    private GameObject _gazedAtObject;

    public void SetStatetoPlacing(){
        tablet.SetActive(false);
        playerState = State.Placing;
    }

    public void SetStatetoWalking() {
        tablet.SetActive(false);
        playerState = State.Walking;
    }

    private void Start() {
        controller = gameObject.AddComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;

        mapCam.SetActive(false);
        tablet.SetActive(false);
    }

    private void UpdateMovement(){

        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.
        Vector2 move = gamepad.leftStick.ReadValue();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 forward = playerCam.transform.forward;
        if(forward.magnitude > 0.1){
            forward = Vector3.Normalize(Vector3.Scale(forward, new Vector3(1.0f, 0.0f, 1.0f)));
        }
        else{
            forward = new Vector3(0, 0, 0);
        }

        Vector3 lateral = playerCam.transform.right;
        if(lateral.magnitude > 0.1){
            lateral = Vector3.Normalize(Vector3.Scale(lateral, new Vector3(1.0f, 0.0f, 1.0f)));
        }
        else{
            lateral = new Vector3(0, 0, 0);
        }

        Vector3 forwardMovement = forward * move.y * 5;
        Vector3 lateralMovement = lateral * move.x * 5;

        playerVelocity = forwardMovement + lateralMovement;
        playerVelocity.y += gravityValue;
        controller.Move(playerVelocity * Time.deltaTime * playerSpeed);

    }

    private void UpdateGaze(){
        // float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // xRotationCamera -= mouseY;
        // xRotationCamera = Mathf.Clamp(xRotationCamera, -90f, 90f);

        // yRotationCamera += mouseX;

        // playerCam.transform.localRotation = Quaternion.Euler(xRotationCamera, yRotationCamera, 0f);
    }

    void Update(){
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        if(playerState == State.Walking){
            UpdateMovement();
            UpdateGaze();

            // Check for joystick inputs
            if(gamepad.rightShoulder.wasPressedThisFrame){
                playerState = State.Selecting;

                Vector3 forward = playerCam.transform.forward;
                Vector3 right = playerCam.transform.right;

                forward.y = 0;
                forward.Normalize();

                right.y = 0;
                right.Normalize();

                tablet.transform.localRotation = Quaternion.identity;
                tablet.transform.position = gameObject.transform.position;

                tablet.transform.Rotate(0, playerCam.transform.localEulerAngles.y, 0);
                tablet.transform.Translate(0.0f, 0.0f, 1.7f);
                //tablet.transform.position = new Vector3(0.00f, 0.12f, 3.53f) + gameObject.transform.position;
                Debug.Log("Activated tablet...");
                tablet.SetActive(true);
            }
            else if(gamepad.leftShoulder.wasPressedThisFrame){
                playerState = State.Map;
                playerCam.SetActive(false);
                mapCam.SetActive(true);
            }

            // Raycast for interaction with placed objects
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 10))
            {
                // GameObject detected in front of the camera.
                if (_gazedAtObject != hit.transform.gameObject)
                {
                    if(_gazedAtObject != null)
                        _gazedAtObject?.GetComponent<SelectableProp>()?.Unselect();
                    if(hit.transform.gameObject?.tag == "ReticleSelectable"){
                        _gazedAtObject = hit.transform.gameObject;
                        _gazedAtObject?.GetComponent<SelectableProp>()?.Select();
                    }
                    else{
                        _gazedAtObject = null;
                    }
                }
                
            }
            else
            {
                if(_gazedAtObject != null)    
                    _gazedAtObject?.GetComponent<SelectableProp>()?.Unselect();
                _gazedAtObject = null;
            }
        }
        else if(playerState == State.Selecting){
            UpdateGaze();

            // Raycast for popup tablet
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 10))
            {
                // GameObject detected in front of the camera.
                if (_gazedAtObject != hit.transform.gameObject)
                {
                    _gazedAtObject?.GetComponent<PlaceButton>()?.Unselect();
                    if(hit.transform.gameObject?.tag == "ReticleSelectable"){
                        _gazedAtObject = hit.transform.gameObject;
                        _gazedAtObject?.GetComponent<PlaceButton>()?.Select();
                    }
                    else{
                        _gazedAtObject = null;
                    }
                }
                
            }
            else
            {
                _gazedAtObject?.GetComponent<PlaceButton>()?.Unselect();
                _gazedAtObject = null;
            }

            if(gamepad.rightShoulder.wasPressedThisFrame){
                playerState = State.Walking;
                tablet.SetActive(false);
            }
        }
        else if(playerState == State.Placing){
            // We are doing this from the Placeable.cs script
            // UpdateGaze();
            // if(gamepad.rightShoulder.wasPressedThisFrame){
            //     playerState = State.Walking;
            // }
        }
        else if(playerState == State.Map){
            UpdateGaze();
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Road");
            if(gamepad.leftShoulder.wasPressedThisFrame){
                playerState = State.Walking;
                mapCam.SetActive(false);
                playerCam.SetActive(true);
                playerGhost.SetActive(false);
            }
            else if (Physics.Raycast(mapCam.transform.position, mapCam.transform.forward, out hit, Mathf.Infinity,mask))
            {
                playerGhost.SetActive(true);
                playerGhost.transform.position = hit.point + new Vector3(0.0f, 2.0f, 0.0f);
                if(gamepad.rightShoulder.wasPressedThisFrame){
                    gameObject.transform.position = hit.point + new Vector3(0.0f, 2.0f, 0.0f);
                    mapCam.SetActive(false);
                    playerCam.SetActive(true);
                    playerGhost.SetActive(false);
                    playerState = State.Walking;
                }
                
            }

        }
        
    }
}
