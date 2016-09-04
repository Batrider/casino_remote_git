using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LanguageSelect : EditorWindow {

    public Rect WindowRect = new Rect(Screen.width/3,Screen.height/3,Screen.width/3,Screen.height/3);
    void OnGUI()
    {
        NGUIEditorTools.BeginContents(false);
        ComponentSelector.Draw<UIAtlas>(NGUISettings.atlas, OnSelectAtlas, false);
        NGUIEditorTools.EndContents();

        GUILayout.BeginHorizontal();
        bool select = GUILayout.Button("替换图集");
        if (select)
        {
            ReplaceNormalSpriteMethod();
        }
        bool select2 = GUILayout.Button("替换语言包");
        if (select2)
        {
            ReplaceLanguageSpriteMethod();
        }
        GUILayout.EndHorizontal();

        NGUIEditorTools.BeginContents(false);
        ComponentSelector.Draw<UIFont>(NGUISettings.uifont, OnSelectUIFont, false);
        NGUISettings.fontSize = EditorGUILayout.IntField("Size", NGUISettings.fontSize);
        NGUIEditorTools.EndContents();

        GUILayout.BeginVertical();
        bool select3 = GUILayout.Button("替换字体");
        if (select3)
        {
            ReplaceFontMethod();
        }
        GUILayout.EndVertical();
        
    }
    // Add menu item to show this demo.
    [MenuItem("Custom Editor/替换")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(LanguageSelect));
    }
    static void ReplaceFontMethod()
    {
        UILabel[] uis = GameObject.FindGameObjectWithTag("GameManager").transform.GetComponentsInChildren<UILabel>();
        for(int i = 0;i<uis.Length;i++)
        {
            uis[i].bitmapFont = NGUISettings.uifont;
            uis[i].fontSize = NGUISettings.fontSize;
        }
    }
    static void ReplaceNormalSpriteMethod()
    {
        UISprite[] uis = GameObject.FindGameObjectWithTag("GameManager").transform.GetComponentsInChildren<UISprite>();
        List<UISprite> spriteList = new List<UISprite>();
        for(int i = 0;i<uis.Length;i++)
        {
            if(!uis[i].atlas.name.Contains("Lang")&&!uis[i].atlas.name.Contains("Small"))
                spriteList.Add(uis[i]);
        }
        for(int i = 0;i<spriteList.Count;i++)
        {
            spriteList[i].atlas = NGUISettings.atlas;
            spriteList[i].keepAspectRatio = UIWidget.AspectRatioSource.Free;
            spriteList[i].MakePixelPerfect();
            Debug.Log(spriteList[i].atlas.name);
        }
    }
    static void ReplaceLanguageSpriteMethod()
    {
        UISprite[] uis = GameObject.FindGameObjectWithTag("GameManager").transform.GetComponentsInChildren<UISprite>();
        List<UISprite> spriteList = new List<UISprite>();
        for(int i = 0;i<uis.Length;i++)
        {
            if(uis[i].atlas.name.Contains("Lang"))
                spriteList.Add(uis[i]);
        }
        for(int i = 0;i<spriteList.Count;i++)
        {
            spriteList[i].atlas = NGUISettings.atlas;
            spriteList[i].keepAspectRatio = UIWidget.AspectRatioSource.Free;
            spriteList[i].MakePixelPerfect();
            
            Debug.Log(spriteList[i].atlas.name);
        }
    }
    void OnSelectAtlas (Object obj)
    {
        if (NGUISettings.atlas != obj)
        {
            NGUISettings.atlas = obj as UIAtlas;
            Repaint();
        }
    }
    void OnSelectUIFont (Object obj)
    {
        if(obj != NGUISettings.uifont)
        {
            NGUISettings.uifont = obj as UIFont;
            Repaint();
        }
    }


}
