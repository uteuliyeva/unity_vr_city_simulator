using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placeable : MonoBehaviour
{

    public GameObject realProp;

    public float xOffset;
    public float yOffset;
    private Camera playerCam;
    private GameObject player;
    

    private float scaleSpeed = 0.01f;
    private float rotationSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main;
        player = GameObject.Find("Player");
        Debug.Log(gameObject.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, (1 << 10)))
        {
            float scale = gameObject.transform.localScale.x;
            Vector3 forward = gameObject.transform.forward;
            Vector3 right = gameObject.transform.right;
            gameObject.transform.position = hit.point + forward * xOffset * scale + right * yOffset * scale  ;
        }

        if(gamepad.rightShoulder.wasPressedThisFrame){
            Place();
        }
        if(gamepad.leftShoulder.wasPressedThisFrame){
            player.GetComponent<Movement>().SetStatetoWalking();
            Destroy(gameObject);
        }

        Vector2 move = gamepad.leftStick.ReadValue();

        float scaleIncrement = move.y;
        float rotationIncrement = move.x;

        if(scaleIncrement != 0){
            gameObject.transform.localScale += new Vector3(scaleIncrement * scaleSpeed, scaleIncrement * scaleSpeed, scaleIncrement * scaleSpeed);
        }
        if(rotationIncrement != 0){
            // Vector3 rotation_center = gameObject.GetComponent<Renderer>().bounds.center;
            gameObject.transform.Rotate(0.0f, rotationIncrement * rotationSpeed, 0.0f);
        }
    }

    public void Place(){
        Debug.Log("Prop placed...");
        GameObject rp = Instantiate(realProp);
        rp.transform.position = gameObject.transform.position;
        rp.transform.localRotation = gameObject.transform.localRotation;
        rp.transform.localScale = gameObject.transform.localScale;
        rp.tag = "ReticleSelectable";
        Destroy(gameObject);
        player.GetComponent<Movement>().SetStatetoWalking();
    }
}
