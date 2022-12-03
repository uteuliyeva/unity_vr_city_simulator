using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
enum WorldTime{
    Day,
    Night
}
public class Switch : MonoBehaviour
{
    public Material sunMaterial;
    public Material sunMaterialSelected;
    public Material moonMaterial;
    public Material moonMaterialSelected;

    public Material daySkyboxMaterial;
    public Material nightSkyboxMaterial;
    public Image pointer;
    public GameObject playerCam;
    public Color nightColor;

    public Material buildingMaterial;

    private bool selected = false;
    private WorldTime currTime = WorldTime.Day;
    // Start is called before the first frame update
    void Start()
    {
        currTime=WorldTime.Day;
    }
    void Update()
    {
        Vector3 scale = pointer.GetComponent<RectTransform>().localScale;
        LayerMask mask = LayerMask.GetMask("Switch");
        RaycastHit hit;
        
        if(selected == true){
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 2,mask)==false)
            {
                this.Unselect();
            }
            else if(scale.x < 1.5f){
                pointer.GetComponent<RectTransform>().localScale = scale + new Vector3(0.01f, 0.01f, 0.0f);
            }
            else{
                selected = false;
                pointer.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
                if (currTime==WorldTime.Day)
                {
                    currTime=WorldTime.Night;
                    gameObject.GetComponent<MeshRenderer>().material = moonMaterial;
                    RenderSettings.skybox=nightSkyboxMaterial;
                    RenderSettings.ambientLight = nightColor;
                    buildingMaterial.EnableKeyword("_EMISSION");
                    buildingMaterial.SetColor("_EmissionColor", Color.yellow); 

                }
                else{
                    currTime=WorldTime.Day;
                    gameObject.GetComponent<MeshRenderer>().material = sunMaterial;
                    RenderSettings.skybox=daySkyboxMaterial;
                    RenderSettings.ambientLight = Color.white;
                    buildingMaterial.DisableKeyword("_EMISSION");
                }
            }
        }
        else if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 2,mask)){
            this.Select();
        }
    }
    
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void Select(){
        if (currTime==WorldTime.Day){
            gameObject.GetComponent<MeshRenderer>().material = sunMaterialSelected;
        }
        else{
            gameObject.GetComponent<MeshRenderer>().material = moonMaterialSelected;
        }
        pointer.GetComponent<Image>().color = new Color(0.88f, 0.0f, 1.0f, 0.9f);
        selected = true;
    }

    public void Unselect(){
        if (currTime==WorldTime.Day){
            gameObject.GetComponent<MeshRenderer>().material = sunMaterial;
        }
        else{
            gameObject.GetComponent<MeshRenderer>().material = moonMaterial;
        }
        selected = false;
        pointer.GetComponent<Image>().color = new Color(0.0f, 1.0f, 1.0f, 0.7f);
        pointer.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
