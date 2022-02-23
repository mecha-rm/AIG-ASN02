using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Mouse class for mouse interactions with colliders in a 2D space.
 * This is an adaptation of a file I made for other projects, which has had most 3D components of it removed or altered.
 * I am likely still using some version of this class for other projects, some of which are likely to be group projects.
 * As such, please be aware of similiarites btween this code in other programs I or my group members have/am working on.
 * since this is a 2D camera, only orthographic camera functions are kept.
 */

public class Mouse2D : MonoBehaviour
{
    // the mouse key for mouse operations. The default is Keycode.Mouse0, which is the left mouse button.
    public KeyCode mouseKey = KeyCode.Mouse0;

    // the world position of the mouse.
    public Vector3 mouseWorldPosition;

    // the object the mouse is hovering over.
    [Tooltip("The object the cursor is hovering over.")]
    public GameObject hoveredObject = null;

    // the object that has been clicked and held on.
    // when the mouse button is released, this is set to null. This variable gets set to null when the mouse button is released.
    [Tooltip("The object the cursor is being held on. Becomes null once the mouse button is released.")]
    public GameObject heldObject = null;

    // the last object that was clicked on. The next time someone clicks on something, this will be set to null.
    [Tooltip("The object the cursor last clicked on.")]
    public GameObject lastClickedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        // checks type of camera.
        if (!Camera.main.orthographic)
            Debug.LogWarning("The camera is not orthographic. Mouse2D results will not be accurate.");

    }

    // checks to see if the cursor is in the window.
    public static bool MouseInWindow()
    {
        return MouseInWindow(Camera.main);
    }

    // checks to see if the cursor is in the window.
    public static bool MouseInWindow(Camera cam)
    {
        // checks area
        bool inX, inY;

        // gets the viewport position
        Vector3 viewPos = cam.ScreenToViewportPoint(Input.mousePosition);

        // check horizontal an vertical.
        inX = (viewPos.x >= 0 && viewPos.x <= 1.0);
        inY = (viewPos.y >= 0 && viewPos.y <= 1.0);

        return (inX && inY);
    }

    // gets the mouse position in world space using the main camera.
    public static Vector3 GetMousePositionInWorldSpace()
    {
        return GetMousePositionInWorldSpace(Camera.main);
    }

    // gets the mouse cursor position in world space.
    public static Vector3 GetMousePositionInWorldSpace(Camera cam)
    {
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }

    // gets the mosut target position in world space using the main camera.
    public static Vector3 GetMouseTargetPositionInWorldSpace(GameObject refObject)
    {
        return GetMouseTargetPositionInWorldSpace(Camera.main, refObject.transform.position);
    }

    // gets the mouse target position in world space.
    public static Vector3 GetMouseTargetPositionInWorldSpace(Camera cam, GameObject refObject)
    {
        return GetMouseTargetPositionInWorldSpace(Camera.main, refObject.transform.position);
    }

    // gets the mouse target position in world space with a reference vector.
    public static Vector3 GetMouseTargetPositionInWorldSpace(Vector3 refPos)
    {
        return GetMouseTargetPositionInWorldSpace(Camera.main, refPos);
    }

    // gets the mouse target position in world space with a reference vector.
    public static Vector3 GetMouseTargetPositionInWorldSpace(Camera cam, Vector3 refPos)
    {
        Vector3 camWPos = GetMousePositionInWorldSpace(cam);
        Vector3 target = camWPos - refPos;
        return target;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target; // ray's target
        Ray ray; // ray object
        RaycastHit hitInfo; // info on hit.
        bool rayHit; // true if the ray hit.

        // gets the mouse position.
        mouseWorldPosition = GetMousePositionInWorldSpace();

        // tries to get a hit with the ray. Since it's orthographic, the ray goes straight forward.
        target = Camera.main.transform.forward; // target is into the screen (z-direction), so camera.forward is used.

        // ray position is mouse position in world space.
        ray = new Ray(mouseWorldPosition, target.normalized);

        // cast the ray about as far as the camera can see.
        rayHit = Physics.Raycast(ray, out hitInfo, Camera.main.farClipPlane - Camera.main.nearClipPlane);

        // checks if the ray got a hit. If it did, save the object the mouse is hovering over.
        // also checks if object has been clicked on.
        if (rayHit)
        {
            // saves the hovered over object.
            hoveredObject = hitInfo.collider.gameObject;

            // left mouse button has been clicked, so save to clicked object as well.
            if (Input.GetKeyDown(mouseKey))
            {
                heldObject = hitInfo.collider.gameObject; // save the held obect.
                lastClickedObject = hitInfo.collider.gameObject; // saves the last clicked object.
            }
                
        }    
        else
        {
            // attempts a 2D raycast since it's an orthographic camera.
            RaycastHit2D rayHit2D = Physics2D.Raycast(
                new Vector2(mouseWorldPosition.x, mouseWorldPosition.y),
                new Vector2(target.normalized.x, target.normalized.y),
                Camera.main.farClipPlane - Camera.main.nearClipPlane
                );

            // if there was a collider, then the rayhit was successful.
            if(rayHit2D.collider != null)
            {
                // the ray hit was successful.
                rayHit = true;

                // saves the hovered over object.
                hoveredObject = rayHit2D.collider.gameObject;

                // left mouse button has been clicked, so save to clicked object as well.
                if (Input.GetKeyDown(mouseKey))
                {
                    heldObject = rayHit2D.collider.gameObject; // saves the held obect.
                    lastClickedObject = rayHit2D.collider.gameObject; // saves the last clicked object.
                }
            }
            else
            {
                // stll no object hit.
                hoveredObject = null;
            }
        }

        // left mouse button released, so clear clicked object.
        if (Input.GetKeyUp(mouseKey))
            heldObject = null;

    }
}
