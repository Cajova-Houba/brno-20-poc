using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public Transform rightBorder;

    public Transform leftBorder;

    public SpriteRenderer backgroundSprite;

    float minX, maxX;

    // Start is called before the first frame update
    void Start()
    {
        minX = leftBorder.position.x;
        maxX = rightBorder.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // align x-axis of camera with the player
        Vector3 cameraPos = transform.position;
        cameraPos.x = player.position.x;

        // stop camera at the background border
        float cameraWidth = GetComponent<Camera>().orthographicSize;
        if ((cameraPos.x - cameraWidth) > minX 
            && (cameraPos.x + cameraWidth) < maxX)
        {
            transform.position = cameraPos;
        }
    }
}
