using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomObjectAlignment : EditorWindow {

    [MenuItem("Window/MF_CustomAlignment")]
    // Use this for initialization
    static void Init () {

        var window = EditorWindow.GetWindow<CustomObjectAlignment>();

        window.Show();
	}
	 
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        RenderAlignmentsAtOrigin();

        EditorGUILayout.EndVertical();

    }

    private void RenderAlignmentsAtOrigin()
    {
        EditorGUILayout.LabelField("Align Objects at Origin on:");

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("X"))
        {
            if (Selection.gameObjects.Length > 0)
            {
                var objects = new List<GameObject>(Selection.gameObjects);

                var transforms = objects.Select((o) => o.transform);

                foreach (var item in transforms)
                {
                    item.position = new Vector3(0, item.position.y, item.position.z);
                }
            }
        }

        if (GUILayout.Button("Y"))
        {
            if (Selection.gameObjects.Length > 0)
            {
                var objects = new List<GameObject>(Selection.gameObjects);

                var transforms = objects.Select((o) => o.transform);

                foreach (var item in transforms)
                {
                    item.position = new Vector3(item.position.x, 0, item.position.z);
                }
            }
        }

        if (GUILayout.Button("Z"))
        {
            if (Selection.gameObjects.Length > 0)
            {
                var objects = new List<GameObject>(Selection.gameObjects);

                var transforms = objects.Select((o) => o.transform);

                foreach (var item in transforms)
                {
                    item.position = new Vector3(item.position.x, item.position.y, 0);
                }
            }
        }

        EditorGUILayout.EndHorizontal();

    }
    
}
