using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public GameObject car;
    public float z = -10f;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 carPosition = this.car.gameObject.transform.position;
        this.transform.position = new Vector3(carPosition.x, carPosition.y, z); 
    }
}
