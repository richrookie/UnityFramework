// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(JoystickController))]
// public class JoystickEditor : Editor
// {
//     JoystickController _joystickController = null;

//     private void OnEnable()
//     {
//         _joystickController = target as JoystickController;
//     }

//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();

//     }


//     [MenuItem("Create/Joystick")]
//     public static void Create()
//     {
//         GameObject go = GameObject.Instantiate(Resources.Load("JoystickCanvas"));
//         go.name = "JoystickCanvas";
//         SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManagement.EditorSceneManager.GetActiveScene());
//     }
// }
