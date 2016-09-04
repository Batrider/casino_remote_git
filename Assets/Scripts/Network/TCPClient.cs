using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
public class StateObject
{
    // Client socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 2048;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
    public byte[] recieveDatas = new byte[]{};
}
public delegate void InceptData(byte[] getBytes);

public class TCPClient
{
    //	private byte [] buffer = new byte[1024];
    public string TCPserverName = string.Empty;
    public string TCPserverIP = string.Empty;
    public int TCPserverPort = 0;
    public Socket clientSocket;
    public InceptData recievet;

    public int charSize = 0;

    public TCPClient()
    {
        this.TCPserverIP = VersionController.TCPserverIP;
        this.TCPserverPort = VersionController.TCPserverPort;
        SocketConnect();
    }
    public TCPClient(string Ip, int Port)
    {
        this.TCPserverIP = Ip;
        this.TCPserverPort = Port;
        SocketConnect();
    }
	
    public  void SocketConnect()
    {
        Debug.Log("SocketConnecting");
        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(TCPserverIP), TCPserverPort);  
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            IAsyncResult result = clientSocket.BeginConnect(ipe, new AsyncCallback(connectCallback), clientSocket);
            //IAsyncResult result = clientSocket.BeginConnect (TCPserverName,TCPserverPort,new AsyncCallback (connectCallback),clientSocket);
        } catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    //只有第一次连接的时候会调用
    private void connectCallback(IAsyncResult asyncConnect)
    {
        //与socket建立连接成功，开启线程接受服务端数据。
        receive();
        Debug.Log("recieved");
    }
    public void Send(byte[] sendbytes)
    {
        if (clientSocket == null)
            return;
        if (!clientSocket.Connected)
        {
            clientSocket.Close();
            return;
        }
        try
        {
            IAsyncResult asyncSend = clientSocket.BeginSend(sendbytes, 0, sendbytes.Length, SocketFlags.None, new AsyncCallback(sendCallback), clientSocket);
        } catch (System.Exception e)
        {
            Debug.Log("send message error: " + e);
        }
    }
    private void sendCallback(IAsyncResult asyncSend)
    {
//		Debug.Log("send Message succesfull:");
    }

    public void receive()
    {
        try
        {
//            Debug.Log("PrefixSize:"+prefixSize);
            StateObject so = new StateObject();
            so.workSocket = clientSocket;
            //第一次读取数据的总长度
            clientSocket.BeginReceive(so.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(receivedCallback), so);
        } catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
            clientSocket.Close();
        }
    }
    //字符串长度标志
    public  int prefixSize = 4;
    private bool isPresix = true;
    public MemoryStream receiveData = new MemoryStream();
    public int curPrefix = 0;//需要读取的数据总长度
    public void receivedCallback(IAsyncResult ar)
    {
        try
        {
            StateObject so = (StateObject)ar.AsyncState;
            Socket client = so.workSocket;
            if(!client.Connected)
            {
                return;
            }
            //结束读取、返回已读取的缓冲区里的字节数组长度
            int readSize = client.EndReceive(ar);
            //将每次读取的数据、写入内存流里
            receiveData.Write(so.buffer,0,readSize);
            receiveData.Position = 0;
            this.charSize = readSize;

            if (readSize > 0)
            {
                //--------解析出该消息总字节的长度 curPrefix(这个我们包括前面4个字节）
                if((int)receiveData.Length>=prefixSize&&isPresix)
                {
                    byte[] presixBytes = new byte[prefixSize];
                    receiveData.Read(presixBytes,0,prefixSize);
                    Array.Reverse(presixBytes);
                    curPrefix = BitConverter.ToInt32(presixBytes,0);
//                    NGUIDebug.Log("all msg length:"+curPrefix);
                    isPresix = false;
                }
                if(receiveData.Length< (long)curPrefix)
                {
                    //如果数据没有读取完毕，调整Position到最后，接着读取。
                    receiveData.Position = receiveData.Length;
                }
                else
                {
                    //如果内存流中的实际数字总长度符合要求，则说明数据已经全部读取完毕。
                    //将position位置调整到第4个节点，开始准备读取数据。
                    receiveData.Position = 4;
                    //解析正文
                    byte[] datas = new byte[curPrefix - 4];
                    receiveData.Read(datas, 0, datas.Length);
                    receiveDataLoad(datas);
                    
                }
            }

            //继续接收下次游戏
            client.BeginReceive(so.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(receivedCallback), so);

		
        } catch (System.Exception e)
        {

            Debug.LogError(e.ToString());
            //Closed();
        }
    }
    private void receiveDataLoad(byte[]  bytes)
    {
        byte[] getByte = new byte[bytes.Length];
        System.Array.Copy(bytes, 0, getByte, 0, bytes.Length);
        recievet(getByte);

        receiveData = new MemoryStream();
        isPresix = true;
        curPrefix = 0;//需要读取的数据总长度
    }

    public void Closed()
    {
        try
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            clientSocket = null;
        } catch (System.Exception e)
        {
            Debug.Log(e);
            clientSocket.Close();
            clientSocket = null;
        }
    }
}