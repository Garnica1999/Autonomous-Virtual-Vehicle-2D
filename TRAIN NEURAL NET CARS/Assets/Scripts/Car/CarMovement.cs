using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement
{
    public float Velocity
    {
        get;
        private set;
    }

    /// <summary>
    /// The current rotation of the car.
    /// </summary>
    public Quaternion Rotation
    {
        get;
        private set;
    }

    public GameObject Car { 
        get; 
        set; 
    }

    public double[] CarOutputs
    {
        get { return new double[] { horizontalInput, verticalInput }; }
    }

    public double horizontalInput;
    public double verticalInput;

    private const float MAX_VEL = 20f;
    private float ACCELERATION = 8f;
    private const float VEL_FRICT = 2f;
    private const float TURN_SPEED = 90f;


    public CarMovement(GameObject Car)
    {
        this.Car = Car;
    }

    // Start is called before the first frame update
    public void Initialize()
    {
        this.Rotation = Quaternion.Euler(0, 0, 270);
    }

    // Update is called once per frame
    public void MoveCar()
    {
        this.horizontalInput = Input.GetAxis("Horizontal");
        this.verticalInput = Input.GetAxis("Vertical");

        ApplyInput();
        ApplyVelocity();
        ApplyFriction();
        
    }

    public void ApplyInput()
    {
        if (verticalInput > 1)
            verticalInput = 1;
        else if (verticalInput < -1)
            verticalInput = -1;

        if (horizontalInput > 0.5f)
            horizontalInput = 0.5f;
        else if (horizontalInput < -0.5f)
            horizontalInput = -0.5f;

        //Car can only accelerate further if velocity is lower than engineForce * MAX_VEL
        bool canAccelerate = false;
        if (verticalInput < 0)
            canAccelerate = Velocity > verticalInput * MAX_VEL;
        else if (verticalInput > 0)
            canAccelerate = Velocity < verticalInput * MAX_VEL;

        //Set velocity
        if (canAccelerate)
        {
            Velocity += (float)verticalInput * ACCELERATION * Time.deltaTime;

            //Cap velocity
            if (Velocity > MAX_VEL)
                Velocity = MAX_VEL;
            else if (Velocity < -MAX_VEL)
                Velocity = -MAX_VEL;
        }

        //Set rotation
        Rotation = this.Car.transform.rotation;
        Rotation *= Quaternion.AngleAxis((float)-horizontalInput * TURN_SPEED * Time.deltaTime, new Vector3(0, 0, 1));
    }

    public void ApplyVelocity()
    {
        Vector3 direction = new Vector3(0, 1, 0);
        this.Car.transform.rotation = Rotation;
        direction = Rotation * direction;

        this.Car.transform.position += direction * Velocity * Time.deltaTime;
    }

    public void ApplyFriction()
    {
        if (verticalInput == 0)
        {
            if (Velocity > 0)
            {
                Velocity -= VEL_FRICT * Time.deltaTime;
                if (Velocity < 0)
                    Velocity = 0;
            }
            else if (Velocity < 0)
            {
                Velocity += VEL_FRICT * Time.deltaTime;
                if (Velocity > 0)
                    Velocity = 0;
            }
        }
    }
}
