using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    public void originToSelected(Transform selectedObject) {

        transform.position = selectedObject.position;
    }

    public void resetOrigin()
    {
        transform.position = Vector3.zero;
    }
}
