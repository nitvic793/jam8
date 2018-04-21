using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    bool isSelecting = false;
    Vector3 mousePositionBegin;

    // Maintain a list of all selected objects
    private List<GameObject> selectedObjects = new List<GameObject>();

    void Update()
    {
        // If we press the left mouse button, begin selection and remember the location of the mouse
        if (Input.GetMouseButtonDown(0))
        {
           
            RaycastHit hit = new RaycastHit();
            isSelecting = true;
            mousePositionBegin = Input.mousePosition;

            foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Soldier"))
            {
				selectableObject.GetComponent<UnitBehavior>().isSelected = false;
				selectableObject.GetComponent<Transform>().Find("SelectionCirclePrefab").gameObject.SetActive(false);
            }

            foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Builder"))
            {
                selectableObject.GetComponent<BuilderBehavior>().isSelected = false;
                selectableObject.GetComponent<Transform>().Find("SelectionCirclePrefab").gameObject.SetActive(false);
            }
            selectedObjects.Clear();
			
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.tag == "Soldier" || hit.transform.tag == "Builder")
                {
                    GameObject selectedSoldier = hit.transform.gameObject;
                    selectedObjects.Add(hit.transform.gameObject);
                    Debug.Log("Test");
                    hit.transform.Find("SelectionCirclePrefab").gameObject.SetActive(true);
                    selectedSoldier.GetComponent<BuilderBehavior>().isSelected = true;
                }
                else if (hit.transform.tag == "Building")
                {
                    //Do action to buuilding here
                }
            }
        }

        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {

            foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Soldier"))
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    selectableObject.GetComponent<UnitBehavior>().isSelected = true;
                    selectedObjects.Add(selectableObject);
                }
            }

            foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Builder"))
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    selectableObject.GetComponent<BuilderBehavior>().isSelected = true;
                    selectedObjects.Add(selectableObject);
                }
            }

            #region Debug
            /*
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Selecting [{0}] Units", selectedObjects.Count));
            foreach (var selectedObject in selectedObjects)
                sb.AppendLine("-> " + selectedObject.gameObject.name);
            Debug.Log(sb.ToString());
            */
            #endregion
            isSelecting = false;
        }

        // Highlight all objects within the selection box
        if (isSelecting)
        {
            foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Soldier"))
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    if (!selectableObject.GetComponent<UnitBehavior>().isSelected)
                    {
                        selectableObject.GetComponent<UnitBehavior>().isSelected = true;
                        selectableObject.GetComponent<Transform>().Find("SelectionCirclePrefab").gameObject.SetActive(true);
                    }
                }
            }

            foreach (var selectableObject in GameObject.FindGameObjectsWithTag("Builder"))
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    if (!selectableObject.GetComponent<BuilderBehavior>().isSelected)
                    {
                        selectableObject.GetComponent<BuilderBehavior>().isSelected = true;
                        selectableObject.GetComponent<Transform>().Find("SelectionCirclePrefab").gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    // Check if a gameobject is within the rectangle bounds
    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds = SelectionRectangle.GetViewportBounds(camera, mousePositionBegin, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    // Display the selection rectangle
    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = SelectionRectangle.GetScreenRectangle(mousePositionBegin, Input.mousePosition);
            SelectionRectangle.DrawScreenRectangle(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            SelectionRectangle.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}
