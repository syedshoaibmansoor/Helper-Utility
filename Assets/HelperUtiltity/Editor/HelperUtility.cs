using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Object = System.Object;

public class HelperUtility : EditorWindow
{
    private GUIStyle boxStyle = null;
    
    private bool playerSettingsCheck;
    private bool saveDataCheck;
    private bool canvasUISettingsCheck;

    private bool autoFillBundleId;

    private Vector2 scrollPosition;

    public Object fontObject;
    private bool isTextMeshPro;
    
    // Scene Switcher Class
    private SceneSwitcher sceneSwitcher;
    
    [MenuItem("Utilities/Helper Utility")]
    public static void Init()
    {
        HelperUtility window = new HelperUtility();
        window.Show();
    }

    private void OnEnable()
    {
        sceneSwitcher = new SceneSwitcher();
    }

    private void OnGUI()
    {

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        sceneSwitcher.Init();
        
        SaveDataGUI();
        
        CanvasUISettingsGUI();
        
        PlayerSettingsGUI();
        
        EditorGUILayout.EndScrollView();
    }

    void CanvasUISettingsGUI()
    {
        EditorGUILayout.Space(10f);

        canvasUISettingsCheck = EditorGUILayout.Foldout(canvasUISettingsCheck, "Canvas UI Settings", true);
        
        EditorGUILayout.BeginVertical("Box");

        if (canvasUISettingsCheck)
        {
            isTextMeshPro = EditorGUILayout.Toggle("TextMeshPro: ", isTextMeshPro);
            
            if (isTextMeshPro)
            {
                fontObject = EditorGUILayout.ObjectField((UnityEngine.Object) fontObject, typeof(TMP_FontAsset), true);
                
                if (fontObject != null)
                {
                    if (GUILayout.Button("Change TextMesh Font"))
                    {
                        TextMeshProUGUI[] textFields;
                
                        textFields = FindObjectsOfType<TextMeshProUGUI>();
                    
                        foreach (var textField in textFields)
                        {
                            textField.font = (TMP_FontAsset) fontObject;
                        }

                        fontObject = null;
                    }
                }
            }
            else
            {
                fontObject = EditorGUILayout.ObjectField((UnityEngine.Object) fontObject, typeof(Font), true);

                if (fontObject != null)
                {
                    if (GUILayout.Button("Change Text Font"))
                    {
                        Text[] textFields;

                        textFields = FindObjectsOfType<Text>();

                        foreach (var textField in textFields)
                        {
                            textField.font = (Font) fontObject;
                        }
                        
                        fontObject = null;
                    }
                }
            }
        }
        
        EditorGUILayout.EndVertical();
    }

    void SaveDataGUI()
    {
        EditorGUILayout.Space(10f);
        
        saveDataCheck = EditorGUILayout.Foldout(saveDataCheck, "Save Data Settings", true);

        if (saveDataCheck)
        {
            EditorGUILayout.BeginVertical("Box");
            
            if (GUILayout.Button("Clear Player Prefs"))
            {
                if (EditorUtility.DisplayDialog("Clear Player Preferences",
                        "Are you sure you want to delete all your Player Preferences? All Data will be lost.",
                        "Yes", "No"))
                {
                    PlayerPrefs.DeleteAll();
                }
            }

            if (GUILayout.Button("Open Persistent Data Path"))
            {
                // Opens the folder path
                Application.OpenURL(Application.persistentDataPath);
                
                // Opens Finder in Macs
                EditorUtility.RevealInFinder(Application.persistentDataPath);
            }
            
            EditorGUILayout.EndVertical();
        }
    }

    void PlayerSettingsGUI()
    {
        EditorGUILayout.Space(10f);

        playerSettingsCheck = EditorGUILayout.Foldout(playerSettingsCheck, "Player Settings", true);

        EditorGUILayout.BeginVertical("Box");
        
        if (playerSettingsCheck)
        {
            PlayerSettings.companyName = EditorGUILayout.TextField("Company Name: ", PlayerSettings.companyName);
            PlayerSettings.productName = EditorGUILayout.TextField("Product Name: ", PlayerSettings.productName);

            autoFillBundleId = EditorGUILayout.Toggle("Auto Fill Bundle ID", autoFillBundleId);

            if (autoFillBundleId)
            {
                PlayerSettings.applicationIdentifier = "com." + PlayerSettings.companyName.ToLower() + "." + PlayerSettings.productName.ToLower().Replace(' ', '.');
                GUI.enabled = false;
            }

            PlayerSettings.applicationIdentifier = EditorGUILayout.TextField("Bundle Id: ", PlayerSettings.applicationIdentifier);

            GUI.enabled = true;

            PlayerSettings.bundleVersion = EditorGUILayout.TextField("Bundle Version: ", PlayerSettings.bundleVersion);
            
            #if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode = EditorGUILayout.IntField("Build Version Code: ", PlayerSettings.Android.bundleVersionCode);
            #elif UNITY_IOS
            PlayerSettings.iOS.buildNumber =EditorGUILayout.TextField("Build Version Code: ", PlayerSettings.iOS.buildNumber);
            #endif
            
            
        }
        
        EditorGUILayout.EndVertical();
    }
}
