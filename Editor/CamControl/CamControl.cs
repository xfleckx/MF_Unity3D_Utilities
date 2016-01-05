using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;

public class CamControl : EditorWindow
{
    /*
     * This an extension of Marc Kusters BlenderCameraControls script found here:
     * 
     * http://wiki.unity3d.com/index.php/Blender_Camera_Controls
     *
     * 
     */


    private static bool isEnabled = true;

    private static bool autoRotate = false;

    private static float autoRotateSpeed = 0.1f;

    private static bool showKeyMapping = false;

    [MenuItem("Window/" + "CamControl Window")]
    public static void Init()
    {
        CamControl window = GetWindow<CamControl>();
        var icon = EditorGUIUtility.Load("Assets/MF_Unity3D_Utilities/Editor/CamControl/blender.png") as Texture2D;
         
        window.titleContent = new GUIContent("CamControl",  icon) ;
        window.minSize = new Vector2(10, 10);
    }

    public void OnEnable()
    {
        Debug.Log("Enable Blender Cam Controls");

        SceneView.onSceneGUIDelegate += OnScene;
    }

    public void OnGUI()
    {
        GUILayoutOption[] options = { GUILayout.MinWidth(5) };

        if (!SpecialKeyController.NumLockEnabled())
            EditorGUILayout.HelpBox("Numlock off?", MessageType.Info);
        else
            Repaint();

        //Enable or disable button
        if (!isEnabled)
        {
            if (GUILayout.Button("Enable", options))
                isEnabled = true;
        }
        else
        {
            if (GUILayout.Button("Disable", options))
                isEnabled = false;
        }

        showKeyMapping = EditorGUILayout.Foldout(showKeyMapping, "Show Key Mapping");

        if (showKeyMapping) { 
            GUILayout.TextArea(KEYCODES);
        }

        EditorGUILayout.BeginHorizontal();

        autoRotate = GUILayout.Toggle(autoRotate, new GUIContent("Auto rotate", "Enables the constant rotation of the Scene View"));

        autoRotateSpeed = EditorGUILayout.FloatField("Rotation speed", autoRotateSpeed);

        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Close", options))
            GetWindow<CamControl>().Close();

    }

    private static void OnScene(SceneView sceneview)
    {
        if (!isEnabled) return;

        UnityEditor.SceneView sceneView;
        Vector3 eulerAngles;
        Event current;
        Quaternion rotHelper;

        current = Event.current;

        sceneView = SceneView.lastActiveSceneView;

        eulerAngles = sceneView.camera.transform.rotation.eulerAngles;
        rotHelper = sceneView.camera.transform.rotation;
         
        if (autoRotate)
        {
            sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y + autoRotateSpeed, eulerAngles.z)));
            sceneview.Repaint();
        }

        if (!current.isKey || current.type != EventType.keyDown)
            return;
        

        switch (current.keyCode)
        {
            case KeyCode.Keypad1:
                if (current.control == false)
                    sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(0f, 360f, 0f)));
                else
                    sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(0f, 180f, 0f)));
                break;
            case KeyCode.Keypad2:
                sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, rotHelper * Quaternion.Euler(new Vector3(-15f, 0f, 0f)));
                break;
            case KeyCode.Keypad3:
                if (current.control == false)
                    sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(0f, 270f, 0f)));
                else
                    sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(0f, 90f, 0f)));
                break;
            case KeyCode.Keypad4:
                sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y + 15f, eulerAngles.z)));
                break;
            case KeyCode.Keypad5:
                sceneView.orthographic = !sceneView.orthographic;
                break;
            case KeyCode.Keypad6:
                sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y - 15f, eulerAngles.z)));
                break;
            case KeyCode.Keypad7:
                if (current.control == false)
                    sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
                else
                    sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, Quaternion.Euler(new Vector3(270f, 0f, 0f)));
                break;
            case KeyCode.Keypad8:
                sceneView.LookAtDirect(SceneView.lastActiveSceneView.pivot, rotHelper * Quaternion.Euler(new Vector3(15f, 0f, 0f)));
                break;
            case KeyCode.KeypadPeriod:
                if (Selection.transforms.Length == 1)
                    sceneView.LookAtDirect(Selection.activeTransform.position, sceneView.camera.transform.rotation);
                else if (Selection.transforms.Length > 1)
                {
                    Vector3 tempVec = new Vector3();
                    for (int i = 0; i < Selection.transforms.Length; i++)
                    {
                        tempVec += Selection.transforms[i].position;
                    }
                    sceneView.LookAtDirect((tempVec / Selection.transforms.Length), sceneView.camera.transform.rotation);
                }
                break;
            case KeyCode.KeypadMinus:
                SceneView.RepaintAll();
                sceneView.size *= 1.1f;
                break;
            case KeyCode.KeypadPlus:
                SceneView.RepaintAll();
                sceneView.size /= 1.1f;
                break;
        }
    }

    public void OnDestroy()
    {
        Debug.Log("Disable Blender Cam Controls");

        SceneView.onSceneGUIDelegate -= OnScene;
    }

    private const string KEYCODES = "Numpad1 \t = Front view \n" +
                                    "Control + Numpad1 = Rear view \n" +
                                    "Numpad2 \t = Rotate view down \n" +
                                    "Numpad3 \t = Right view \n" +
                                    "Control + Numpad3 = Left view \n" +
                                    "Numpad4 \t = Rotate view left \n" +
                                    "Numpad5 \t = Switch between orthographic and perspective \n" +
                                    "Numpad6 \t = Rotate view right \n" +
                                    "Numpad6 \t = Rotate view right \n" +
                                    "Control + Numpad7 = Down view \n" +
                                    "Numpad8 \t = Move view up \n" +
                                    "Numpad. \t = Center view on object(s) \n Note: only works on objects that have the CanEditMultipleObjects property \n" +
                                    "Numpad- \t = Zoom camera out \n" +
                                    "Numpad+ \t = Zoom camera in";

}


public static class SpecialKeyController
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern short GetKeyState(int keyCode);


    [DllImport("user32.dll")]
    private static extern int GetKeyboardState(byte[] lpKeyState);


    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);


    private const byte VK_NUMLOCK = 0x90; private const uint KEYEVENTF_EXTENDEDKEY = 1; private const int KEYEVENTF_KEYUP = 0x2; private const int KEYEVENTF_KEYDOWN = 0x0;


    public static bool NumLockEnabled()
    {
        return (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
    }


    public static void SetNumLock(bool bState) { if (NumLockEnabled() != bState) { keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0); keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0); } }
}