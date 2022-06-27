using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    void Update()
    {
        void Update () {
            RaycastHit hit = new RaycastHit();
            for (int i = 0; i < UnityEngine.Input.touchCount; ++i) 
            {
                if (UnityEngine.Input.GetTouch(i).phase.Equals(TouchPhase.Began)) 
                {
                    Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.GetTouch(i).position);
                    if (Physics.Raycast(ray, out hit)) 
                    {
                        hit.transform.gameObject.SendMessage("OnMouseDown");
                    }
                }
            }
        }
    }
}
