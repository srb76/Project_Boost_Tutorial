using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Mover : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 0f);
    [SerializeField] Vector3 rotationVector;
    [SerializeField] [Range(0,1)] float movementFactor;
    [SerializeField] [Range(0, 1)] float rotationFactor;
    [SerializeField] float period = 2f;

    Vector3 startingPos;
    Vector3 offset;

    const float tau = Mathf.PI * 2f;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles * tau);
        //move object
        movementFactor = rawSinWave / 2f + .5f;

        offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
