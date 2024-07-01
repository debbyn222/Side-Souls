/*Purpose:
Makes the camera follow the player
*/
//Last Edited: 25th of June, 2024 @1:40am
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0, 0, -10);// Offset to keep the camera behind the player
    public float smoothSpeed = 2.0f;//speed at which camera follows player
    void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
            //updated camera's position
        }
    }
}
