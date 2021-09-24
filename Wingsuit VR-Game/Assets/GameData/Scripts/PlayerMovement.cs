using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;

    [SerializeField] private Vector2 minContraints;
    [SerializeField] private Vector2 maxContraints;

    Transform cam;


    private void Start()
    {
        cam = transform.Find("Main Camera"); 
    }

    private void Update()
    {
        transform.position += transform.forward * movementSpeed * Time.deltaTime;


        transform.rotation = cam.transform.rotation;
    }

}
