

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher
{
    private bool sceneSwitcherCheck;
    
    public void Init()
    {
        SceneSwitcherGUI();
    }

    void SceneSwitcherGUI()
    {
        EditorGUILayout.Space(5f);
        
        sceneSwitcherCheck = EditorGUILayout.Foldout(sceneSwitcherCheck, "Scene Switcher", true);
        EditorGUILayout.BeginVertical("Box");
        if (sceneSwitcherCheck)
        {
            
            for (int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
            {
                EditorGUILayout.BeginHorizontal();
                
                if (EditorSceneManager.GetActiveScene().path == SceneUtility.GetScenePathByBuildIndex(i))
                    GUI.enabled = false;
                
                if (GUILayout.Button(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i))))
                {
                    if (EditorSceneManager.GetActiveScene().isDirty)
                    {
                        if (EditorUtility.DisplayDialog("Changing Unsaved Scene", "Do you want to save your scene? All unsaved data will be lost if you press No",
                                "Yes", "No"))
                        {
                            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                        }
                    }
                    
                    EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(i));
                }

                GUI.enabled = true;

                if (GUILayout.Button("Select"))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(SceneUtility.GetScenePathByBuildIndex(i)));
                }

                EditorGUILayout.EndHorizontal();
                
            }
            
        }
        EditorGUILayout.EndVertical();
    }
}
