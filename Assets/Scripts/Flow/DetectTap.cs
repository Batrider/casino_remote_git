using TouchScript.Gestures;
using TouchScript.Gestures.Simple;
using UnityEngine;
using System.Collections.Generic;
namespace ca.HenrySoftware.CoverFlow
{
    public class DetectTap : MonoBehaviour
    {
        public float Scale = 1.1f;
        public float Time = 0.333f;
        private Vector3 _origiginalScale;
        private int _tweenUp;
        private int _tweenDown;
        private SceneDataDetect sceneDataDetect;
        private SceneManager sceneManager;
        protected void Awake()
        {
            _origiginalScale = gameObject.transform.localScale;
            sceneDataDetect = GetComponent<SceneDataDetect>();
  //          sceneManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneManager>();
        }
        /// <summary>
        /// 點擊事件
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void HandleTap(object sender, TouchScript.Events.GestureStateChangeEventArgs e)
        {

            if (e.State == Gesture.GestureState.Recognized)
            {
                Debug.Log("Start Down 2");

                //如果资源已存在本地，则进入加载场景，否则下载资源
                if (GetComponent<SceneDataDetect>().dataState == SceneDataDetect.DataState.Downloaded)
                {
                    Debug.Log("Start Down 3");

                    //如果点击的场景在屏幕中央，则直接进入
                    if (transform.localPosition.z == 0)
                        LoadScene();
                    else
                    {
                        FlowView.Instance.Flow(gameObject);
                        Invoke("LoadScene", 0.4f);
                    }
                }
                //判断当前是否有其他场景处于下载状态
                else if (GetComponent<SceneDataDetect>().dataState == SceneDataDetect.DataState.unDownload)
                {
                    Debug.Log("Start Down 4");
                     
                    //下载： 當前如果有場景正在下載，那就不響應
                    foreach (KeyValuePair<int,SceneDataDetect.DataState> kp in SceneManager.sceneDataState)
                    {
                        if (kp.Value == SceneDataDetect.DataState.Downloading)
                        {
                            Debug.Log(kp.Key);
                            return;
                        }
                    }
                    Debug.Log("Start Down 5");
                    //設置狀態條
                    sceneDataDetect.dataState = SceneDataDetect.DataState.Downloading;
                    sceneDataDetect.psc.DownLoadObj(true);
                    sceneDataDetect.psc.UnDownLoadObj(false);
                    //刷新卡牌當前狀態
                    SceneManager.sceneDataState[int.Parse(gameObject.name)] = SceneDataDetect.DataState.Downloading;
//                    sceneManager.DownLoadData(int.Parse(gameObject.name));
                    SceneManager.Instance.DownLoadData(int.Parse(gameObject.name));
                }
            }
        }
        /// <summary>
        /// 加載進場景
        /// </summary>
        public void LoadScene()
        {
            SceneManager.Instance.ChangeTheScene(int.Parse(gameObject.name));
//            sceneManager.ChangeTheScene(int.Parse(gameObject.name));
        }
        #region 卡牌其餘狀態

        private void HandlePress(object sender, TouchScript.Events.GestureStateChangeEventArgs e)
        {
            if (e.State == Gesture.GestureState.Recognized)
            {
                FlowView.Instance.StopInertia();
                PressGesture gesture = sender as PressGesture;
                ScaleUp(gesture.gameObject);
            }
        }
        private void ScaleUp(GameObject o)
        {
            LeanTween.cancel(o, _tweenUp);
            LeanTween.cancel(o, _tweenDown);
            Vector3 to = Vector3.Scale(_origiginalScale, new Vector3(Scale, Scale, 1.0f));
            _tweenUp = LeanTween.scale(o, to, Time).setEase(LeanTweenType.easeSpring).id;
        }
        private void HandleRelease(object sender, TouchScript.Events.GestureStateChangeEventArgs e)
        {
            if (e.State == Gesture.GestureState.Recognized)
            {
                ReleaseGesture gesture = sender as ReleaseGesture;
                ScaleDown(gesture.gameObject);
            }
        }
        private void ScaleDown(GameObject o)
        {
            _tweenDown = LeanTween.scale(o, _origiginalScale, Time).setEase(LeanTweenType.easeSpring).id;
        }
        #endregion
        public void OnEnable()
        {
            GetComponent<TapGesture>().StateChanged += HandleTap;
            GetComponent<PressGesture>().StateChanged += HandlePress;
            GetComponent<ReleaseGesture>().StateChanged += HandleRelease;
        }
        public void OnDisable()
        {
            GetComponent<TapGesture>().StateChanged -= HandleTap;
            GetComponent<PressGesture>().StateChanged -= HandlePress;
            GetComponent<ReleaseGesture>().StateChanged -= HandleRelease;
        }

    }
}
