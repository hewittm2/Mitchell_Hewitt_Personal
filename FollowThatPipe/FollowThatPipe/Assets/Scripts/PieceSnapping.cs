using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSnapping : MonoBehaviour
{
    private GameObject tMin;
    public Camera main;
    public Vector3 screenPoint;
    public Vector3 offset;
    public Vector3 curScreenPoint;
    public Vector3 curPosition;
    public int anchorAmount;
    private GameObject closest;
    private float dist;

    //private float minDist = Mathf.Infinity;

    void Awake()
    {
        closest = null;
        //GameObject[] anchors = new GameObject[GameObject.FindGameObjectsWithTag("Anchor").Length];
    }

    void Update()
    {
        FindClosestTarget(closest);
    }

    GameObject FindClosestTarget(GameObject anchor)
    {
        GameObject[] anchors = GameObject.FindGameObjectsWithTag("Anchor");

        float distance = Mathf.Infinity;
        Vector3 position = this.transform.position;

        foreach (GameObject go in anchors)
        {
            Vector3 diff = go.transform.position - this.transform.position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }


    void OnMouseDown()
    {
        main = FindObjectOfType<Camera>();

        this.screenPoint = main.WorldToScreenPoint(this.gameObject.transform.position);

        this.offset = this.gameObject.transform.position - main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        closest = null;
    }

    private void OnMouseDrag()
    {
        this.curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        this.curPosition = main.ScreenToWorldPoint(curScreenPoint) + this.offset;

        this.transform.position = (this.curPosition);

    }

    public void OnMouseUp()
    {
        this.transform.position = new Vector3(closest.transform.position.x, closest.transform.position.y, closest.transform.position.z - 1);
        this.transform.SetParent(closest.gameObject.transform);
    }
}
