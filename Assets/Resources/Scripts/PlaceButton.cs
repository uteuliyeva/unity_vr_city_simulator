using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceButton : MonoBehaviour
{

    public Material idleMaterial;
    public Material selectedMaterial;
    public Image pointer;
    public GameObject player;

    public GameObject Prototype;

    private bool selected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 scale = pointer.GetComponent<RectTransform>().localScale;
        if(selected == true){
            
            if(scale.x < 1.5f){
                Debug.Log(pointer.GetComponent<RectTransform>().localScale);
                pointer.GetComponent<RectTransform>().localScale = scale + new Vector3(0.01f, 0.01f, 0.0f);
            }
            else{
                selected = false;
                pointer.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
                player.GetComponent<Movement>().SetStatetoPlacing();
                Unselect();
                Instantiate(Prototype);
            }
        }
    }

    public void Select(){
        gameObject.GetComponent<MeshRenderer>().material = selectedMaterial;    
        pointer.GetComponent<Image>().color = new Color(0.88f, 0.0f, 1.0f, 0.9f);
        selected = true;
    }

    public void Unselect(){
        gameObject.GetComponent<MeshRenderer>().material = idleMaterial;
        pointer.GetComponent<Image>().color = new Color(0.0f, 1.0f, 1.0f, 0.7f);
        selected = false;
        pointer.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
