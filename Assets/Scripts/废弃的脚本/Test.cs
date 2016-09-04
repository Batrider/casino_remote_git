using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
    public int maxValue;
	// Use this for initialization
	void Start () {
        PrimeFilter(maxValue);
	/*
       List<int> primes = PrimeNumber(maxValue);
        Debug.Log(primes.Count);
        /*
        for(int i = 0;i<primes.Count;i++)
        {
            Debug.Log("prime:"+primes[i]);
        }
        */
	}
	
	public List<int> PrimeNumber(int maxValue)
    {
        float startTime = Time.realtimeSinceStartup;
        List<int> primes = new List<int>();
        primes.Add(2);
        bool isPrime = false;
        for(int i = 3;i<maxValue+1;i+=2)
        {
            for(int j = 0;j<primes.Count;j++)
            {
                if(i%primes[j] == 0)
                {
                    //不是质数
                    isPrime = false;
                    break;
                }
                isPrime = true;                
            }
            if(isPrime)
                primes.Add(i);
        }
        float endTime = Time.realtimeSinceStartup;
        Debug.Log("Duration:"+(endTime-startTime));
        
        return primes;
    }

    public void PrimeFilter(int maxValue)
    {
        float startTime = Time.realtimeSinceStartup;
        
        bool[] primelist = new bool[maxValue+1];
        for(int i = 0;i<primelist.Length;i++)
        {
            primelist[i] = true;
        }
        primelist[0] = false;
        primelist[1] = false;

        for(int i = 0;i<primelist.Length;i++)
        {
            if(primelist[i] == true)
            {
                for(int j = 2*i;j<primelist.Length;j+=i)
                {
                    primelist[j] = false;
                }
            }
        }
        //统计
        int countPrime = 0;
        for(int i = 0;i<primelist.Length;i++)
        {
            if(primelist[i] == true)
            {
//                Debug.Log("质数："+i);
                countPrime++;
            }
        }
        float endTime = Time.realtimeSinceStartup;
        Debug.Log("Duration:"+(endTime-startTime));


       
    }




























}
