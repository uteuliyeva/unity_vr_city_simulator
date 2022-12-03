using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnResetClick(){
        Debug.Log("Reset click!");
    }

    public void OnPointerEnter(){
        Debug.Log("Reset enter!");
    }

    public void OnPointerExit(){
        Debug.Log("Reset exit!");
    }
}
