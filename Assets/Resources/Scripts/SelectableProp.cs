using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableProp : MonoBehaviour
{

    public GameObject Prototype;
    private GameObject pointer;
    private GameObject player;

    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        pointer = GameObject.Find("Image");
        player = GameObject.Find("Player");
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
                GameObject proto = Instantiate(Prototype, gameObject.transform.position, gameObject.transform.rotation);
                proto.transform.localScale = gameObject.transform.localScale;
                Destroy(gameObject);
            }
        }
    }

    public void Select(){
        selected = true;
        pointer.GetComponent<Image>().color = new Color(0.88f, 0.0f, 1.0f, 0.9f);
    }

    public void Unselect(){
        selected = false;
        pointer.GetComponent<Image>().color = new Color(0.0f, 1.0f, 1.0f, 0.7f);
        pointer.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
