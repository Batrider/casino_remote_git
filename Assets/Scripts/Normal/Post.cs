using UnityEngine;
using System.Collections;
using LitJson;
using System.Text;

public class Post : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoginWithToken_IE());
    }
    IEnumerator LoginWithToken_IE()
    {
        HttpLogin_SEND send = new HttpLogin_SEND();
        send.user_name = "demoagent003";
        send.user_password = "333333";
        send.key = "777_game_malaysia_sd";
        string resultJson = string.Empty;

        for (int i = 0; i < 1; i++)
        {
            string jsonDataPost = JsonMapper.ToJson(send);
            Debug.Log(jsonDataPost);
            WWW www = new WWW("http://777uat.ceo888.club/mobile_login", Encoding.UTF8.GetBytes(jsonDataPost));
            float timeCount = 0;
            while (!www.isDone)
            {
                timeCount += Time.fixedDeltaTime;
                if (timeCount > 5) break;
                yield return null;
            }
            if (www.isDone && www.error == null)
            {
                resultJson = www.text;
                break;
            }else
            {
                Debug.Log(www.error);
            }
        }
        if (resultJson != string.Empty)
        {
            Debug.Log("success-------content----" + resultJson);
            HttpLogin_RECIEVE msgJsonRecieve = JsonMapper.ToObject<HttpLogin_RECIEVE>(resultJson);
            Debug.Log("5001");
        }
    }
}
