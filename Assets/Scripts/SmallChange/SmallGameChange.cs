//小游戏的进入特效脚本
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SmallGameChange : MonoBehaviour {
    
    public Object smallGameObj;
	public GameObject[] AlphaOBJ1;
	public GameObject[] AlphaOBJ2;
    private List<GameObject> sGs = new List<GameObject>();
	public virtual void ChangeToSg()
	{
	}
    public virtual void ReturnToGame()
	{
	}
    protected IEnumerator InstaniateSmallGame()
    {
        yield return new WaitForSeconds(4.5f);
        GameObject smallGame = Instantiate(smallGameObj) as GameObject;
        smallGame.transform.SetParent(transform);
        smallGame.transform.localEulerAngles = Vector3.zero;
        smallGame.transform.localPosition = Vector3.zero;
        smallGame.transform.localScale = Vector3.one;
        sGs.Add(smallGame);
    }
    protected void RemoveTheSmallGame()
    {
        foreach(GameObject go in sGs)
        {
            go.SetActive(false);
            Destroy(go);
        }
        sGs.Clear();
    }
    
    public IEnumerator YieldDelay()
    {
        manager.smallgame = false;
        yield return new WaitForSeconds(4f);
        RemoveTheSmallGame();
    }
}
