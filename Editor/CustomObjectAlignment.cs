using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomObjectAlignment : EditorWindow {

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

    private void RenderAlignmentsAtOrigin()
    {
        EditorGUILayout.LabelField("Align Objects at Origin on:");

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("X") && hasSelection)
        {
                var transforms = selecteObjects.Select((o) => o.transform);

                foreach (var item in transforms)
                {
                    item.position = new Vector3(0, item.position.y, item.position.z);
                }
        }

        if (GUILayout.Button("Y") && hasSelection)
        {
                var transforms = selecteObjects.Select((o) => o.transform);

                foreach (var item in transforms)
                {
                    item.position = new Vector3(item.position.x, 0, item.position.z);
                }
        }

        if (GUILayout.Button("Z") && hasSelection)
        { 
            var transforms = selecteObjects.Select((o) => o.transform);

            foreach (var item in transforms)
            {
                item.position = new Vector3(item.position.x, item.position.y, 0);
            } 
        }

        EditorGUILayout.EndHorizontal();

    }
    
}
