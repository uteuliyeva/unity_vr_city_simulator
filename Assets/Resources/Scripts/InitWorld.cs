using UnityEngine;
using System.Collections.Generic;
public class InitWorld : MonoBehaviour 
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject Road;
    public GameObject sidewalk;
    public int worldWidth;

    private int assetOffset = 1;

    private List<GameObject> buildings;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        LoadBuildingAssets();

        // Instantiate starting pavement
        Instantiate(sidewalk, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        // Instantiate starting buildings
        for(int i = 1; i < worldWidth; i++){
            int buildingID = Random.Range(0, buildings.Count);
            Vector3 buildingFootprint = buildings[buildingID].GetComponent<Renderer>().bounds.size;
            Instantiate(buildings[buildingID], 
                        new Vector3((int)buildingFootprint.x * (i + assetOffset), 0, 0), 
                        Quaternion.identity);

            buildingID = Random.Range(0, buildings.Count);
            buildingFootprint = buildings[buildingID].GetComponent<Renderer>().bounds.size;
            Instantiate(buildings[buildingID], 
                        new Vector3((int)(-buildingFootprint.x) * (i - assetOffset), 0, 0), 
                        Quaternion.identity);
            
        }

        // Instantiate road
        int roadFootPrint = (int)Road.GetComponent<Renderer>().bounds.size.x;
        int roadSize = Random.Range(2, 5);
        for(int row = 0; row < roadSize; row++){
            for(int i = -worldWidth + 1; i < worldWidth; i++){
                Instantiate(Road, new Vector3(roadFootPrint * (i + assetOffset), 0, roadFootPrint * row), Quaternion.Euler(0, 90, 0));
            }
        }

        // Instantiate opposing buildings
        for(int i = 1; i < worldWidth; i++){
            int buildingID = Random.Range(0, buildings.Count);
            Vector3 buildingFootprint = buildings[buildingID].GetComponent<Renderer>().bounds.size;
            Instantiate(buildings[buildingID], 
                        new Vector3((int)buildingFootprint.x * i, 0, roadSize * roadFootPrint), 
                        Quaternion.Euler(0, 180, 0));

            buildingID = Random.Range(0, buildings.Count);
            buildingFootprint = buildings[buildingID].GetComponent<Renderer>().bounds.size;
            Instantiate(buildings[buildingID], 
                        new Vector3((int)(-buildingFootprint.x) * i, 0, roadSize * roadFootPrint), 
                        Quaternion.Euler(0, 180, 0));
            
        }

        // Opposing sidewalk
        Instantiate(sidewalk, new Vector3(0.0f, 0.0f, (roadSize + 1) * roadFootPrint), Quaternion.identity);
        
    }

    void LoadBuildingAssets(){
        buildings = new List<GameObject>();
        buildings.Add(Resources.Load("Buildings/SM_Bld_Shop_01") as GameObject);
        buildings.Add(Resources.Load("Buildings/SM_Bld_Shop_02") as GameObject);
        buildings.Add(Resources.Load("Buildings/SM_Bld_Shop_04") as GameObject);
        buildings.Add(Resources.Load("Buildings/SM_Bld_Shop_05") as GameObject);
        buildings.Add(Resources.Load("Buildings/SM_Bld_Shop_06") as GameObject);
        buildings.Add(Resources.Load("Buildings/SM_Bld_Shop_Corner_01") as GameObject);
        buildings.Add(Resources.Load("Buildings/SM_Bld_Shop_Corner_02") as GameObject);
    }
}