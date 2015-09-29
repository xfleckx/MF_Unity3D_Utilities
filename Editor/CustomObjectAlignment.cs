using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomObjectAlignment : EditorWindow {

    private enum AlignmentTypes { Origin, FirstSelected, LastSelected }

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

    private bool hasSelection = false;
    private List<GameObject> selecteObjects;

    private bool CheckEditorSelection()
    {
        if (Selection.gameObjects.Length > 0)
        {
            selecteObjects = new List<GameObject>(Selection.gameObjects);
            return true;
        }

        return false;
    }
      
    private AlignmentTypes targetAlignment = AlignmentTypes.Origin;

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
                switch (targetAlignment)
                {
                    case AlignmentTypes.Origin:
                        item.position = new Vector3(0, item.position.y, item.position.z);
                        break;
                    case AlignmentTypes.FirstSelected:
                        var first = transforms.First().position;
                        item.position = new Vector3(first.x, item.position.y, item.position.z);
                        break;
                    case AlignmentTypes.LastSelected:
                        var last = transforms.Last().position;
                        item.position = new Vector3(last.x, item.position.y, item.position.z);
                        break;
                    default:
                        break;
                }
            }
        }

        if (GUILayout.Button("Y") && hasSelection)
        {
                var transforms = selecteObjects.Select((o) => o.transform);

                foreach (var item in transforms)
                {
                    switch (targetAlignment)
                    {
                        case AlignmentTypes.Origin:
                            item.position = new Vector3(item.position.x, 0, item.position.z);
                            break;
                        case AlignmentTypes.FirstSelected:
                            var first = transforms.First().position;
                            item.position = new Vector3(item.position.y, first.y, item.position.z);
                            break;
                        case AlignmentTypes.LastSelected:
                            var last = transforms.Last().position;
                            item.position = new Vector3(item.position.x, last.y, item.position.z);
                            break;
                        default:
                            break;
                    } 
                }
        }

        if (GUILayout.Button("Z") && hasSelection)
        { 
            var transforms = selecteObjects.Select((o) => o.transform);

            foreach (var item in transforms)
            {
                switch (targetAlignment)
                {
                    case AlignmentTypes.Origin:
                        item.position = new Vector3(item.position.x, item.position.z, 0);
                        break;
                    case AlignmentTypes.FirstSelected:
                        var first = transforms.First().position;
                        item.position = new Vector3(item.position.y, item.position.y, first.z);
                        break;
                    case AlignmentTypes.LastSelected:
                        var last = transforms.Last().position;
                        item.position = new Vector3(item.position.x, item.position.y, last.z);
                        break;
                    default:
                        break;
                } 
            } 
        }

        EditorGUILayout.EndHorizontal();

    }
     
}
