using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Collections.Generic;
using System.IO;


public class ImportIcons : EditorWindow
{
    // The position of the window
    public Rect windowRect = new Rect(100, 100, 100, 100);
    void OnGUI()
    {
        NGUIEditorTools.DrawHeader("Input", true);
        NGUIEditorTools.BeginContents(false);
        GUILayout.BeginHorizontal();
        {
            ComponentSelector.Draw<Object>("Template", NGUISettings.prefabIcon, OnSelectPrefabs, true, GUILayout.MinWidth(100f));
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        bool select = GUILayout.Button("Select Icon's Folder");
        GUILayout.Space(20f);
        GUILayout.EndHorizontal();
        if (select)
        {
            string path = EditorUtility.OpenFolderPanel("Select Icon's Folder", GetString("IconFolder", string.Empty), "");
            if (!string.IsNullOrEmpty(path))
                SetString("IconFolder", path);
        }
        if (!string.IsNullOrEmpty(GetString("IconFolder", string.Empty)))
        {
            GUILayout.Label("Assets" + GetString("IconFolder", string.Empty).Split(new string[]{"Assets"}, 2, System.StringSplitOptions.None) [1]);
            
        }
        GUILayout.BeginHorizontal();
        GUILayout.Space(20f);
        GUILayout.EndHorizontal();
        bool create = GUILayout.Button("Create Icon's Prefabs");
        if (create)
        {
            //target path of icon select           
            string pathToSelect = GetString("IconFolder", string.Empty);
            if (!string.IsNullOrEmpty(pathToSelect))
            {
                string[] iconsFolders =AssetPathReturn(Directory.GetDirectories(pathToSelect));// AssetDatabase.GetSubFolders(AssetPathReturn(pathToSelect));
                //target path of icon create
                string pathToCreate = EditorUtility.OpenFolderPanel("Create Icon's Prefabs",GetString("IconPrefabsFolder","Assets/IconPrefabs"), "");
                if (!string.IsNullOrEmpty(pathToCreate))
                {
                    SetString("IconPrefabsFolder", pathToCreate);
                    for (int i = 0; i<iconsFolders.Length; i++)
                    {
                        string[] iconFilePaths = Directory.GetFiles(pathToSelect+"/"+System.IO.Path.GetFileName(iconsFolders [i]),"*.png");
                        
                        GameObject iconTemplate = Instantiate(NGUISettings.prefabIcon) as GameObject;
                        UI2DSpriteAnimation u2d = iconTemplate.GetComponent<UI2DSpriteAnimation>();
                        iconTemplate.GetComponent<UI2DSprite>().sprite2D = AssetDatabase.LoadAssetAtPath(AssetPathReturn(iconFilePaths [0]),typeof(Sprite)) as Sprite;
                        u2d.frames = new Sprite[iconFilePaths.Length];
                        
                        
                        for(int j=0;j<iconFilePaths.Length;j++)
                        {
                            Sprite iconSprite = AssetDatabase.LoadAssetAtPath(AssetPathReturn(iconFilePaths [j]),typeof(Sprite)) as Sprite;
                            u2d.frames.SetValue(iconSprite,j);
                        }
                        
                        
                        PrefabUtility.CreatePrefab(AssetPathReturn(pathToCreate)+"/"+System.IO.Path.GetFileName(iconsFolders [i]) + ".prefab", iconTemplate);
                        DestroyImmediate(iconTemplate);
                        
                    }
                }
                
            }
        }
        GUILayout.BeginVertical();
        GUILayout.Space(100f);
        GUILayout.EndVertical();
        bool replace = GUILayout.Button("replace the icon's prefabs");
        if (replace)
        {
            string iconPrefabsLocation = EditorUtility.OpenFolderPanel("Select Icon's Prefabs Location",GetString("IconPrefabsFolder","Assets/IconPrefabs"), "");
            if(!string.IsNullOrEmpty(iconPrefabsLocation))
            {
                ReplaceIconMethod(iconPrefabsLocation);
            }
            
        }
        
    }
    //find all the icon prefab and replace
    static void ReplaceIconMethod(string prefabsLocation)
    {
        string[] iconPrefabsInfos = Directory.GetFiles(prefabsLocation,"*.prefab");
        Dictionary<int,Object> iconPrefabs = new Dictionary<int, Object>();
        for(int j=0;j<iconPrefabsInfos.Length;j++)
        {
            Object iconPrefab = AssetDatabase.LoadAssetAtPath(AssetPathReturn(iconPrefabsInfos [j]),typeof(Object));
            iconPrefabs.Add(iconPrefabs.Count,iconPrefab);
            Debug.Log(iconPrefab.ToString());
        }
        //
        InstanPrefabs[] ips = GameObject.FindGameObjectWithTag("ItemsParent").transform.parent.GetComponentsInChildren<InstanPrefabs>();
        Dictionary<int,GameObject> items = new Dictionary<int, GameObject>();
        foreach (InstanPrefabs ip in ips)
        {
            items.Add(items.Count,ip.gameObject);
            Debug.Log(ip.gameObject.name);
        }
        for(int i = 0;i<items.Count;i++)
        {
            Object iconPrefab;
            GameObject oldIcon;
            if(iconPrefabs.TryGetValue(i<iconPrefabs.Count?i:i-iconPrefabs.Count,out iconPrefab)&&items.TryGetValue(i,out oldIcon))
            {
                GameObject icon = Instantiate(iconPrefab) as GameObject;
                icon.name = iconPrefab.name;
                icon.transform.parent = oldIcon.transform.parent;
                icon.transform.localPosition = oldIcon.transform.localPosition;
                icon.transform.localEulerAngles = oldIcon.transform.localEulerAngles;
                icon.transform.localScale = oldIcon.transform.localScale;
                DestroyImmediate(oldIcon);
            }
        }
        items.Clear();
        iconPrefabs.Clear();
        
        
    }
    /* 会将help界面里面的字体也一并改变的 - -
    //find all the font prefab and replace
    static void ReplaceFontMethod(string prefabsLocation)
    {
        string fontPrefabsInfo = Directory.GetFiles(prefabsLocation,"*.prefab")[0];
        GameObject fontPrefab = AssetDatabase.LoadAssetAtPath(AssetPathReturn(prefabsLocation),typeof(GameObject));
        Debug.Log(fontPrefab.name);
        UILabel[] uis = GameObject.FindGameObjectWithTag("GameManager").transform.GetComponentsInChildren<UILabel>();
        UIFont uiFont = fontPrefab.GetComponent<UIFont>();
        for(int i = 0;i<uis.Length;i++)
        {
            uis[i].bitmapFont = uiFont;
        }
        
        
    }
*/
    
    static string AssetPathReturn(string path)
    {
        return ("Assets" + path.Split(new string[]{"Assets"}, 2, System.StringSplitOptions.None) [1]);
    }
    static string[] AssetPathReturn(string[] path)
    {
        for(int i = 0;i<path.Length;i++)
        {
            path[i] = AssetPathReturn(path[i]);
        }
        return path;
    }
    Dictionary<string, int> GetSpriteList(List<UI2DSprite> ui2dSprite)
    {
        Dictionary<string, int> spriteList = new Dictionary<string, int>();
        
        // If we have textures to work with, include them as well
        if (ui2dSprite.Count > 0)
        {
            List<string> texNames = new List<string>();
            foreach (UI2DSprite tex in ui2dSprite)
                texNames.Add(tex.name);
            texNames.Sort();
            foreach (string tex in texNames)
                spriteList.Add(tex, 2);
        }
        
        if (NGUISettings.atlas != null)
        {
            BetterList<string> spriteNames = NGUISettings.atlas.GetListOfSprites();
            foreach (string sp in spriteNames)
            {
                if (spriteList.ContainsKey(sp))
                    spriteList [sp] = 1;
                else
                    spriteList.Add(sp, 0);
            }
        }
        return spriteList;
    }
    void OnSelectPrefabs(Object obj)
    {
        if (NGUISettings.prefabIcon != obj)
        {
            NGUISettings.prefabIcon = obj;
            Repaint();
        }
    }
    // Add menu item to show this demo.
    [MenuItem("Custom Editor/IconMaker")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(ImportIcons));
    }
    //存取路径
    static public string GetString(string name, string defaultValue)
    {
        return EditorPrefs.GetString(name, defaultValue);
    }
    static public void SetString(string name, string val)
    {
        EditorPrefs.SetString(name, val);
    }
    
}
