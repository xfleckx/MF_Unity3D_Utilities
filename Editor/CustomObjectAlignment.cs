using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomObjectAlignment : EditorWindow {

    private enum AlignmentTypes { Origin, FirstSelected, LastSelected,
        CenterOfMass
    }
    private enum TargetAxes { x,y,z}
     
    private const string WindowTitle = "Alignment Tools";
    
    [MenuItem("Window/MF_CustomAlignment")]
    // Use this for initialization
    static void Init () {

        var window = EditorWindow.GetWindow<CustomObjectAlignment>();

        window.titleContent = new GUIContent(WindowTitle);

        window.Show();
	}
	 
    void OnGUI()
    {
        hasSelection = CheckEditorSelection();

        EditorGUILayout.BeginVertical();
        
        RenderAlignmentsAtOrigin();

        EditorGUILayout.EndVertical();

    }
    private void RenderAlignmentsAtOrigin()
    {
        EditorGUILayout.LabelField("Align Objects at Origin on:");
        
        targetAlignment = (AlignmentTypes) EditorGUILayout.EnumPopup(targetAlignment);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("X") && hasSelection)
        {
            var transforms = selecteObjects.Select((o) => o.transform);

            foreach (var item in transforms)
            {
                item.position = GetPositionFrom(targetAlignment, TargetAxes.x, item.position, transforms);
            }
        }

        if (GUILayout.Button("Y") && hasSelection)
        {
            var transforms = selecteObjects.Select((o) => o.transform);

            foreach (var item in transforms)
            {
                item.position = GetPositionFrom(targetAlignment, TargetAxes.y, item.position, transforms);
            }
        }

        if (GUILayout.Button("Z") && hasSelection)
        {
            var transforms = selecteObjects.Select((o) => o.transform);

            foreach (var item in transforms)
            {
                item.position = GetPositionFrom(targetAlignment, TargetAxes.z, item.position, transforms);
            }
        }

        EditorGUILayout.EndHorizontal();

    }

    #region Alignment utilities
    private bool hasSelection = false;
    private List<GameObject> selecteObjects;

    private AlignmentTypes targetAlignment = AlignmentTypes.Origin;

    private bool CheckEditorSelection()
    {
        if (Selection.gameObjects.Length > 0)
        {
            selecteObjects = new List<GameObject>(Selection.gameObjects);
            return true;
        }

        return false;
    }

    private Vector3 GetNewPosition(Vector3 oldPosition, TargetAxes axes)
    {
        Vector3 newPosition;

        switch (axes)
        {
            case TargetAxes.x:
                newPosition = new Vector3(0, oldPosition.y, oldPosition.z);
                break;
            case TargetAxes.y:
                newPosition = new Vector3(oldPosition.x, 0, oldPosition.z);
                break;
            case TargetAxes.z:
                newPosition = new Vector3(oldPosition.x, oldPosition.y, 0);
                break;
            default:
                newPosition = oldPosition;
                break;
        }

        return newPosition;
    }

    private Vector3 GetNewPosition(Vector3 refPosition, Vector3 oldPosition, TargetAxes axes)
    {
        Vector3 newPosition;

        switch (axes)
        {
            case TargetAxes.x:
                newPosition = new Vector3(refPosition.x, oldPosition.y, oldPosition.z);
                break;
            case TargetAxes.y:
                newPosition = new Vector3(oldPosition.x, refPosition.y, oldPosition.z);
                break;
            case TargetAxes.z:
                newPosition = new Vector3(oldPosition.x, oldPosition.y, refPosition.z);
                break;
            default:
                newPosition = oldPosition;
                break;
        }

        return newPosition;
    }

    private Vector3 GetPositionFrom(AlignmentTypes type, TargetAxes axes, Vector3 oldPosition, IEnumerable<Transform> possibleReferences)
    { 
        switch (type)
        {
            case AlignmentTypes.Origin:
                return GetNewPosition(oldPosition, axes);

            case AlignmentTypes.FirstSelected:
                var first = possibleReferences.First().position;
                return GetNewPosition(first, oldPosition, axes);

            case AlignmentTypes.LastSelected:
                var last = possibleReferences.Last().position;
                return GetNewPosition(last, oldPosition, axes);

            case AlignmentTypes.CenterOfMass:

                var sum = possibleReferences.Select(t => t.position).Aggregate((t_A, t_B) => t_A + t_B);
                var count = possibleReferences.Count();
                var avg = new Vector3(sum.x / count, sum.y / count, sum.z / count);
                return avg;

            default:
                return oldPosition;
        }
    }
    #endregion
}
