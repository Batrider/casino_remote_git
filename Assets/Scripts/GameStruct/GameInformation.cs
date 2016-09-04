using UnityEngine;
using System.Collections;

public class GameInformation{
    private int id;
    private int lines;
    private string name;
    private string prefabsPath;
    private string goldPreabsPath;
    public GameInformation(int id,int line,string name)
    {
        this.id = id;
        this.name = name;
        this.lines = line;
//        this.prefabsPath = prefabsPath;
//       this.goldPreabsPath = goldPreabsPath;
    }
    public int ID
    {
        get{
            return id;
        }
        set{
            this.id = value;
        }
    }
    public int LINES
    {
        get{
            return lines;
        }
        set{
            this.lines = value;
        }
    }
    public string NAME
    {
        get{
            return name;
        }
        set{
            this.name = value;
        }
    }
    public string PrefabsPath
    {
        get{
            return prefabsPath;
        }
        set{
            this.prefabsPath = value;
        }
    }
    public string GoldPreabsPath
    {
        get{
            return goldPreabsPath;
        }
        set{
            this.goldPreabsPath = value;
        }
    }
}
