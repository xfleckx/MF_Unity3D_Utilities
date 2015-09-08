using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

public class WindowGrabber : EditorWindow {

    private const string WindowTitle = "ScreenGrab";

    private Rect lastPosition;

    private const float X_POSITION_NEW_SCENE_VIEW = 10;
    private const float Y_POSITION_NEW_SCENE_VIEW = 10;
    private const float WIDTH_NEW_SCENE_VIEW = 600;
    private const float HEIGHT_NEW_SCENE_VIEW = 400;
    private enum OutputFormat { jpg, png }

    private bool selectJPG = false;
    private bool selectPNG = true; 

    private OutputFormat selectedOutputFormat = OutputFormat.png;

    private const string StandardImageFolderName = "ScreenGrabberOutput";
    private const string StandardPatternImageName = "sceneView";

    private string customImageName = StandardPatternImageName;
    private string customFolderNamer = StandardImageFolderName;

    private Texture2D tmpTexture;


    [MenuItem("Window/MF_ScreenGrabber")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        WindowGrabber window = EditorWindow.GetWindow<WindowGrabber>();

        if (!Directory.Exists(StandardImageFolderName))
        {
            Directory.CreateDirectory(StandardImageFolderName);
        }

        window.titleContent = new GUIContent( WindowTitle );
        
        window.Show();
        
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Image Format");

        selectJPG = GUILayout.Toggle(!selectPNG, Enum.GetName(typeof(OutputFormat), OutputFormat.jpg));

        selectPNG = GUILayout.Toggle(!selectJPG, Enum.GetName(typeof(OutputFormat), OutputFormat.png));

        if (selectJPG)
            selectedOutputFormat = OutputFormat.jpg;

        if (selectPNG)
            selectedOutputFormat = OutputFormat.png;

        string extension = Enum.GetName(typeof(OutputFormat), selectedOutputFormat);

        EditorGUILayout.EndHorizontal();

        customImageName = GUILayout.TextField(customImageName);
        customFolderNamer = GUILayout.TextField(customFolderNamer);

        if(GUILayout.Button("Grab last active scene view"))
        {
            var sceneView = SceneView.lastActiveSceneView;

            if (sceneView == null)
                sceneView = EditorWindow.GetWindow<SceneView>();

            if (sceneView == null) { 
               var createOne = EditorUtility.DisplayDialog("No Scene View available!", "There is no Scene View - please create one through \"Window -> Scene\" or click \"Open Scene Window\"!", "Open Scene Window", "Cancel");

                if (!createOne)
                    return;

                sceneView = EditorWindow.CreateInstance<SceneView>();

                sceneView.position = new Rect(
                    X_POSITION_NEW_SCENE_VIEW, 
                    Y_POSITION_NEW_SCENE_VIEW, WIDTH_NEW_SCENE_VIEW, 
                    HEIGHT_NEW_SCENE_VIEW);

                sceneView.Show();

                return;
            }

            var resultFilePath = string.Format("{1}{0}{2}.{3}", Path.DirectorySeparatorChar, StandardImageFolderName, StandardPatternImageName, extension);

            GrabSingleView(sceneView, new FileInfo(resultFilePath), selectedOutputFormat);
        }

        var availableViews = SceneView.sceneViews;

        if (availableViews.Count > 1 && GUILayout.Button("Grab all active scene views"))
        {
            var listOfViews = availableViews.Cast<SceneView>();

            string multiImageNameFormat = StandardPatternImageName + "_{0}";
            int idx = 0;
            foreach (var view in listOfViews)
            {
                var targetImageFilePath = Path.Combine(StandardImageFolderName, string.Format(multiImageNameFormat, idx));

                GrabSingleView(view, new FileInfo(targetImageFilePath), selectedOutputFormat);
                idx++;
            }
        }

        if(GUILayout.Button("Show Screenshot Folder"))
        {
            Process.Start("explorer.exe", StandardImageFolderName);
            
        }

        EditorGUILayout.EndVertical();
    }

    private void GrabSingleView(EditorWindow view, FileInfo targetFile, OutputFormat format)
    {
        var width = Mathf.FloorToInt(view.position.width);
        var height = Mathf.FloorToInt(view.position.height);
        
        Texture2D screenShot = new Texture2D(width, height);

        this.HideOnGrabbing();

        var colorArray = InternalEditorUtility.ReadScreenPixel(view.position.position, width, height);

        screenShot.SetPixels(colorArray);

        byte[] encodedBytes = null; 
        if (format == OutputFormat.jpg)
        {
            encodedBytes = screenShot.EncodeToJPG(); 
        }
        else
        {
            encodedBytes = screenShot.EncodeToPNG(); 
        }

        File.WriteAllBytes(targetFile.FullName, encodedBytes);

        this.ShowAfterHiding();
    }

    private void HideOnGrabbing()
    {
        lastPosition = this.position;

        this.position = new Rect(0, 0, 1, 1);
    }

    private void ShowAfterHiding()
    {
        this.position = lastPosition;
    }

}
