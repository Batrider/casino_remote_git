using UnityEngine;
using System.Collections.Generic;
namespace ca.HenrySoftware.CoverFlow
{
    public class FlowView : Singleton<FlowView>
    {
        public AudioClip cardClip;

        public float Time = 0.333f;
        public float Offset = 1;
        public bool Clamp = true;
        public GameObject[] Views;
        private float _clamp;
        public int _current;
        private int _tweenInertia;
        protected void Start()
        {
            _clamp = Views.Length * Offset + 1;
            _current = 13;
        }
        public int GetClosestIndex()
        {
            int closestIndex = -1;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < Views.Length; i++)
            {
                float distance = (Vector3.zero - Views [i].transform.localPosition).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }
            return closestIndex;
        }
        public void Flow()
        {
            Flow(GetClosestIndex());
            GetComponent<AudioSource>().PlayOneShot(cardClip);
        }
        private int GetIndex(GameObject view)
        {
            int found = -1;
            for (int i = 0; i < Views.Length; i++)
            {
                if (view == Views [i])
                {
                    found = i;
                }
            }
            return found;
        }
        public void Flow(GameObject target)
        {
            foreach(GameObject card in Views)
            {
                card.GetComponent<BoxCollider>().enabled = false;
            }
            int found = GetIndex(target);
            Debug.Log("found:" + found);
            /*
            if (found != -1)
            {
                Flow(found);
            }
            */
            if (found - _current > 0)
            {
                for (int i = 0; i<found-_current; i++)
                {
                    ChangeViewsLocation(1);
                    Flow(_current + 1);
                }
            } else if(_current - found>0)
            {
                for (int i = 0; i<_current-found; i++)
                {
                    ChangeViewsLocation(-1);
                    Flow(_current - 1);
                }
            }

        }
        public void Flow(int target)
        {
            target = (int)Views.Length / 2;
            for (int i = 0; i < Views.Length; i++)
            {
                int delta = (target - i) * -1;
                Vector3 to = new Vector3(delta * Offset, 0.0f, Mathf.Abs(delta) * Offset * 0.5f);
                LeanTween.moveLocal(Views [i], to, Time).setEase(LeanTweenType.easeSpring);
            }
            _current = target;

        }
        public void Flow(float offset)
        {

            
            for (int i = 0; i < Views.Length; i++)
            {
                Vector3 p = Views [i].transform.localPosition;
                float newX = p.x + offset;
                bool negative = newX < 0;
                Vector3 newP;
                if (Clamp)
                {
                    float clampX = Mathf.Clamp(newX, ClampXMin(i, negative), ClampXMax(i, negative));
                    float clampZ = Mathf.Clamp(Mathf.Abs(newX), 0.0f, ClampXMax(i, negative));
                    newP = new Vector3(clampX, p.y, clampZ * 0.5f);
                } else
                {
                    newP = new Vector3(newX, p.y, Mathf.Abs(newX) * 0.5f);
                }
                Views [i].transform.localPosition = newP;

            }
            //循环卡片设计
            ChangeViewsLocation(GetClosestIndex() - _current);
            _current = (int)Views.Length / 2;

            
        }
        private float ClampXMin(int index, bool negative)
        {
            float newIndex = negative ? index : (Views.Length - index - 1);
            return -(_clamp - (Offset * newIndex));
        }
        private float ClampXMax(int index, bool negative)
        {
            float newIndex = negative ? index : (Views.Length - index - 1);
            return _clamp - (Offset * newIndex);
        }
        public void Inertia(float velocity)
        {
            _tweenInertia = LeanTween.value(gameObject, Flow, velocity, 0, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(Flow).id;
        }
        public void StopInertia()
        {
            LeanTween.cancel(gameObject, _tweenInertia);
        }      
        public void LeftClick()
        {
            if (_current > 0)
            {
                ChangeViewsLocation(-1);
                Flow(_current - 1);
                GetComponent<AudioSource>().PlayOneShot(cardClip);
                
            }
        }
        public void RightClick()
        {
            if (_current < Views.Length - 1)
            {
                ChangeViewsLocation(1);
                Flow(_current + 1);
                GetComponent<AudioSource>().PlayOneShot(cardClip);
                
            }
        }
        public void ChangeViewsLocation(int indexNext)
        {
            GameObject tempObj = indexNext > 0 ? Views [0] : Views [Views.Length - 1];

            if (indexNext > 0)
            {
                for (int i = 0; i<Views.Length-1; i++)
                    Views [i] = Views [i + 1];
                Views [Views.Length - 1] = tempObj;
                Views [Views.Length - 1].transform.localPosition = new Vector3(Views [Views.Length - 2].transform.localPosition.x + 1, 0, Views [Views.Length - 2].transform.localPosition.z + 0.5f);
                
            }
            if (indexNext < 0)
            {
                for (int i = Views.Length-1; i>0; i--)
                    Views [i] = Views [i - 1];
                Views [0] = tempObj;
                Views [0].transform.localPosition = new Vector3(Views [1].transform.localPosition.x - 1, 0, Views [1].transform.localPosition.z - 0.5f);
            }
        }
        void OnDisable()
        {
            SceneManager.cardPos.Clear();
            for (int i = 0; i<Views.Length; i++)
            {
                SceneManager.cardPos.Add(Views [i].name, Views [i].transform.localPosition);
                
            }
        }
        void OnEnable()
        {
            Debug.Log(SceneManager.preSceneStr);
            if (SceneManager.preSceneStr == "Lobby")
            {
                for (int i = 0; i < Views.Length; i++)
                {
                    int delta = (5 - i) * -1;
                    Vector3 to = new Vector3(delta * Offset, 0.0f, Mathf.Abs(delta) * Offset * 0.5f);
                    LeanTween.moveLocal(Views[i], to, Time).setEase(LeanTweenType.easeSpring);
                }//Flow((int)(Views.Length / 2));
            }else
            {
                int i = 0;
                foreach (KeyValuePair<string, Vector3> kp in SceneManager.cardPos)
                {
                    Views[i] = GameObject.Find("FlowRoot/Views/" + kp.Key);
                    Views[i].transform.localPosition = kp.Value;
                    i++;
                }
                SceneManager.cardPos.Clear();
            }
        }
    }

}



