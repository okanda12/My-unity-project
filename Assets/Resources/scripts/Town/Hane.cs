using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hane : MonoBehaviour
{

    [SerializeField] Transform hane1;
    [SerializeField] Transform hane2;

    [SerializeField] float rotationSpeed1 = 10f;
    [SerializeField] float rotationSpeed2 = 70f;

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        
        if(hane1 != null)
        {
            hane1.Rotate(Vector3.forward * rotationSpeed1 * Time.deltaTime);
        }

        if (hane2!=null)
        {
            hane2.Rotate(Vector3.forward * rotationSpeed2 * Time.deltaTime);



        }


    }


}
