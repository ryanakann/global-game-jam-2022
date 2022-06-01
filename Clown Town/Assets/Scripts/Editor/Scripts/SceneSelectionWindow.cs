using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

public class SceneSelectionWindow : EditorWindow
{
    public static SceneSelectionWindow window;

    public static SceneSelectionData data;

    [MenuItem("Tools/Scene Selection %T")]
    static void Init()
    {
        InitWindow();
        InitData();
    }

    private static void InitWindow()
    {
        window = (SceneSelectionWindow)GetWindow(typeof(SceneSelectionWindow));
        window.minSize = new Vector2(300f, 450f);
        window.titleContent = new GUIContent("Scene Selector", "Lists out all scenes in your project hierarchy.");
        window.Show();
    }

    private static void InitData()
    {
        data.hierarchyRoot = new TreeNode("Assets", "");

        var sceneGUIDs = AssetDatabase.FindAssets(string.Format("t:scene"));
        var scenePaths = new string[sceneGUIDs.Length];
        for (int i = 0; i < sceneGUIDs.Length; i++)
        {
            scenePaths[i] = AssetDatabase.GUIDToAssetPath(sceneGUIDs[i]);
            var pathStructure = scenePaths[i].Split('/');

            AppendToTree(pathStructure, scenePaths[i], ref data.hierarchyRoot);
        }
    }

    private static void AppendToTree(string[] pathStructure, string fullPath, ref TreeNode root)
    {
        var pathLength = pathStructure.Length;
        if (pathLength == 0) return;
        if (pathLength == 1)
        {
            root.name = root.name.Replace(".unity", "");
            root.path = fullPath;
            return;
        }

        var nextDirName = pathStructure[1];
        var pathSubstructure = new string[pathLength - 1];
        Array.Copy(pathStructure, 1, pathSubstructure, 0, pathLength - 1);

        if (root.children == null)
        {
            root.children = new List<TreeNode>();
        }

        for (int i = 0; i < root.children.Count; i++)
        {
            var existingChild = root.children[i];
            if (existingChild.name == nextDirName)
            {
                AppendToTree(pathSubstructure, fullPath, ref existingChild);
                root.children[i] = existingChild;
                return;
            }
        }

        root.children.Add(new TreeNode(nextDirName, ""));
        var newChild = root.children[root.children.Count - 1];
        AppendToTree(pathSubstructure, fullPath, ref newChild);
        root.children[root.children.Count - 1] = newChild;
    }

    void OnGUI()
    {
        float minHeight = EditorGUIUtility.singleLineHeight;

        GUILayout.BeginArea(new Rect(0f, 0f, window.position.width, minHeight), EditorStyles.toolbar);
        data.searchText = EditorGUILayout.TextField("Scenes in Project: ", data.searchText, EditorStyles.toolbarSearchField);
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(0f, minHeight, window.position.width, window.position.height - minHeight * 2.5f));
        data.scrollPosition = EditorGUILayout.BeginScrollView(data.scrollPosition);
        DisplayHierarchy(ref data.hierarchyRoot);
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(0f, window.position.height - minHeight * 1.5f, window.position.width, minHeight * 1.5f), EditorStyles.toolbar);
        EditorGUILayout.BeginHorizontal();
        data.loadAdditively = EditorGUILayout.ToggleLeft("Load additively", data.loadAdditively);
        GUILayout.Button("Create");
        GUILayout.Button("Cancel");
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void DisplayHierarchy(ref TreeNode node, int indentLevel = 0)
    {
        EditorGUI.indentLevel = indentLevel;

        if (node.children.Count == 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15f * indentLevel);
            node.expanded = GUILayout.Button(node.name);
            GUILayout.EndHorizontal();
            if (node.expanded)
            {
                //if (EditorSceneManager.dir)
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(node.path, data.loadAdditively ? OpenSceneMode.Additive : OpenSceneMode.Single);
            }
            return;
        } else
        {
            node.expanded = EditorGUILayout.Foldout(node.expanded, node.name);
            if (!node.expanded) return;
        }


        for (int i = 0; i < node.children.Count; i++)
        {
            var child = node.children[i];
            DisplayHierarchy(ref child, indentLevel + 1);
            node.children[i] = child;
        }
        EditorGUI.indentLevel = indentLevel;
    }
}

public struct SceneSelectionData
{
    public string searchText;
    public int selectedSceneIndex;
    public Vector2 scrollPosition;
    public bool loadAdditively;

    public TreeNode hierarchyRoot;
}

public class TreeNode
{
    public string name;
    public string path;
    public bool expanded;
    public List<TreeNode> children;

    public TreeNode(string name, string path)
    {
        this.name = name;
        this.path = path;
        expanded = true;
        children = new List<TreeNode>();
    }
}