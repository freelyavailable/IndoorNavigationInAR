using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireClass{

    float posX;
    float posY;
    float posZ;
    float rotX;
    float rotY;
    float rotZ;
    float rotW;

    public FireClass(Vector3 pos, Quaternion rot)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
        rotW = rot.w;
    }


}
