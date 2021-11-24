using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public CarMovement car;
    public Vector3 source;
    public Vector3[] pointsToVisit;

    private int iteration;
    void Start()
    {
        source = car.gameObject.transform.localPosition;
        iteration = 0;
        car.source = source;
        car.goal = pointsToVisit[iteration];
        /*
        Debug.Log(car.source);
        Debug.Log(car.goal);
        */
    }


    void Update()
    {
        if (iteration < pointsToVisit.Length)
        {
            car.goal = pointsToVisit[iteration];
            car.moveCar();
            if (car.keepMoving() == false)
            {
                car.source = pointsToVisit[iteration];
                iteration++;
                if (iteration < pointsToVisit.Length)
                {
                    car.goal = pointsToVisit[iteration];
                    car.resetMovement();
                }
            }
        }
    }
}
