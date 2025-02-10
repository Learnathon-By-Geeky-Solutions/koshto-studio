using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float movement;
    public float movementSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement,0f,0f) * Time.fixedDeltaTime * movementSpeed;
        throw new NotImplementedException();
    }
}
