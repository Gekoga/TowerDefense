using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    //Sensitivity of the move
    public float moveSensitivityX = 1.0F;
    public float moveSensitivityY = 1.0F;
    public bool useMouse = true;
    //zoomSpeedModifier modifies the move/swipe speed with the camera zoom
    public float zoomSpeedModifier = 0.1F;
    //Min and Max values for movement
    public float maxX = 500;
    public float minX = -500;
    public float maxZ = 500;
    public float minZ = -500;
    public float maxFov = 60;
    public float minFov = 5;
    //Zooming speed
    public float zoomSpeed = 0.5F;
    //Wether to invert X or Y swipes
    public bool invertY = false;
    public bool invertX = false;
    //Decrease of the speed after a swipe
    public float decreaseCamSpeedBy = 100.0F;
    //x & y speeds to move the cam with
    private float xSpeed = 0.0F;
    private float ySpeed = 0.0F;
    //Mouse Position
    private Vector2 mouseCurrentPosition = new Vector3(0, 0);
    private Vector2 mouseDeltaPosition = new Vector3(0, 0);
    private Vector2 mouseLastPosition = new Vector3(0, 0);
    // Update is called once per frame
    void Update()
    {
        //get the camera
        Camera cam = GetComponent<Camera>();
        //Save mousePosition and mouseDeltaPosition
        mouseCurrentPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mouseDeltaPosition = mouseCurrentPosition - mouseLastPosition;
        float xValue;
        float yValue;
        float zValue;
        //Decrease the speed if it's not 0
        if (xSpeed > 0)
        {
            if (xSpeed - (decreaseCamSpeedBy * Time.deltaTime) > 0)
            {
                xSpeed -= decreaseCamSpeedBy * Time.deltaTime;
            }
            else {
                xSpeed = 0;
            }
        }
        else if (xSpeed < 0)
        {
            if (xSpeed + (decreaseCamSpeedBy * Time.deltaTime) < 0)
            {
                xSpeed += decreaseCamSpeedBy * Time.deltaTime;
            }
            else
            {
                xSpeed = 0;
            }
        }
        if (ySpeed > 0)
        {
            if (ySpeed - (decreaseCamSpeedBy * Time.deltaTime) > 0)
            {
                ySpeed -= decreaseCamSpeedBy * Time.deltaTime;
            }
            else
            {
                ySpeed = 0;
            }
        }
        else if (ySpeed < 0)
        {
            if (ySpeed + (decreaseCamSpeedBy * Time.deltaTime) < 0)
            {
                ySpeed += decreaseCamSpeedBy * Time.deltaTime;
            }
            else
            {
                ySpeed = 0;
            }
        }
        //Mouse Controls
        if (useMouse)
        {
            //Mouse Zoom, using scroll wheel
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * -1, minFov, maxFov);
            //Mouse move, using drag
            if (Input.GetMouseButton(0))
            {
                //Muliply delta x with the xSensitity
                float positionX = mouseDeltaPosition.x * moveSensitivityX;
                //Run the invert if the bool is true for x
                positionX = invertX ? positionX : positionX * -1;
                //Muliply delta y with the ySensitity
                float positionY = mouseDeltaPosition.y * moveSensitivityY;
                //Run the invert if the bool is true for x
                positionY = invertY ? positionY : positionY * -1;
                //Set the speed values.
                xSpeed = positionX * (cam.fieldOfView * zoomSpeedModifier);
                ySpeed = positionY * (cam.fieldOfView * zoomSpeedModifier);
            }
        }

        //Move the camera according to the speeds
        xValue = Mathf.Clamp(transform.position.x + xSpeed * Time.deltaTime, minX, maxX);
        yValue = transform.position.y;
        zValue = Mathf.Clamp(transform.position.z + ySpeed * Time.deltaTime, minZ, maxZ);
        transform.position = new Vector3(xValue, yValue, zValue);
        //Save the mousePosition as lastMousePosition for the next update
        mouseLastPosition = mouseCurrentPosition;
    }
}