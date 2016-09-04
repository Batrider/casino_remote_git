using UnityEngine;
using System.Collections;

public class SimulateData : MonoBehaviour {
    //游戏数据模拟
	/*
    public GameObject inputObj;
    private UILabel[] nums = new UILabel[17];
    string[] datas = {"03","05","16","11","10","10","09","16","16","11","12","08","12","10","12","06","08"};
    void Start()
    {
        nums[0] = inputObj.transform.FindChild("Label").GetComponent<UILabel>();
        nums[0].text = datas[0];
        for (int i = 1; i<17; i++)
        {
            GameObject inputIns = (GameObject)Instantiate(inputObj); 
            inputIns.name = (i+1).ToString();
            inputIns.transform.parent = this.gameObject.transform;
            inputIns.transform.localPosition = new Vector3(35f*(i+1),18,0);
            inputIns.transform.localEulerAngles = Vector3.zero;
            inputIns.transform.localScale = Vector3.one;

            nums[i] = inputIns.transform.FindChild("Label").GetComponent<UILabel>();
            nums[i].text = datas[i];
        }
    }
   */
    public UIInput uiInput;
    string[] datas = {"03","05","16","11","10","10","09","16","16","11","12","08","12","10","12","06","08"};
	void Start()
	{
        uiInput.value = datas[0];
		for (int i = 1; i<datas.Length; i++)
		{
            uiInput.value +=" " + datas[i];
		}
	}
    public void simulateData()
    {
        string data = uiInput.value;
        Debug.Log(data);
        GameObject.Find("/UI Root").GetComponent<manager>().StartCoroutine("Simulate");
        GameObject.Find("/GameManager").GetComponent<NetworkConnect>().Simulate(data);
    }
}
