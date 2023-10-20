using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    private void Update()
    {
        //Debug.Log(Input.mousePosition); //Screen 좌표
        //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); //ViewPort 좌표

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100, Color.green, 1.0f);

            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                Debug.Log($"Raycast Camera @{hit.collider.gameObject}");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Collision @ {other.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
    }
}
