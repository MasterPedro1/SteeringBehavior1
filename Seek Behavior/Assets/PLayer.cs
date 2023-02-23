using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayer : MonoBehaviour
{
    public float moveSpeed = 10f;

   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector3.Lerp(transform.position, new Vector3(mousePosition.x, mousePosition.y,0), moveSpeed * Time.deltaTime);
    }
}
