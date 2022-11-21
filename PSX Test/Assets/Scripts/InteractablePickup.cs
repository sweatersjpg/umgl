using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePickup : MonoBehaviour
{
    public Transform player;
    public GameObject playerItem;

    Camera cam;

    Outline outline;
    float outlineSize;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outlineSize = outline.OutlineWidth;

        cam = player.GetComponentInChildren<Camera>();
        playerItem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool mouseOver = MouseOver();

        if (mouseOver) outline.OutlineWidth = outlineSize * 2;
        else outline.OutlineWidth = outlineSize;

        if (Input.GetMouseButtonDown(0))
        {
            if (mouseOver) DoMouseDown();
        }
    }

    bool MouseOver()
    {
        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        Debug.DrawRay(cam.transform.position, ray.direction);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform == transform)
            {
                // Do something
                return true;
            }
        }

        return false;
    }
    private void DoMouseDown()
    {
        Destroy(gameObject);
        playerItem.SetActive(true);
    }

}
