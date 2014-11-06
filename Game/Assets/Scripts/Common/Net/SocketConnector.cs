// .Net Lib
using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;

// Unity Lib
using UnityEngine;

namespace Game.Common
{
    /*------------------------------------------------------------------
      Interface   : ISocketConnector
      Description : 套接字连接器接口。
    --------------------------------------------------------------------*/
    public interface ISocketConnector
    {
        ISocketStream Connect(string    ip, int port, int nMilliseconds);
        ISocketStream Connect(IPAddress ip, int port, int nMilliseconds);
    }

    /*------------------------------------------------------------------
      class       : SocketConnector
      Description : 套接字连接器。
    --------------------------------------------------------------------*/
    public class SocketConnector : ISocketConnector
    {
        public SocketConnector()
        {

        }
        public ISocketStream Connect(string ip, int port,int nMilliseconds)
        {
            return Connect(IPAddress.Parse(ip), port, nMilliseconds);
        }
        public ISocketStream Connect(IPAddress ip, int port, int nMilliseconds)
        {
            int        nRetCode   = 0;
            Socket     s          = SocketWrapper.CreateTcpSocket();
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port);

            try
            {
                s.Connect(ipEndPoint);
            }
            catch(SocketException e)
            {
                int    nErrorCode = e.NativeErrorCode;
                string sErrorDesc = SocketWrapper.SocketErrCode2Desc(nErrorCode);
                Debug.LogErrorFormat("[SocketConnector] connect to server failed! ErrorCode : {0}, ErrorDesc : {1}!", nErrorCode, sErrorDesc);
                SocketWrapper.CloseSocket(ref s);
                return null;
            }

            bool bBlockingState = s.Blocking;
            nRetCode = SocketWrapper.CheckCanSend(s, nMilliseconds);
            if (1 != nRetCode)
            {
                SocketWrapper.CloseSocket(ref s);
                return null;
            }
            s.Blocking = bBlockingState;

            if (!s.Connected)
            {
                SocketWrapper.CloseSocket(ref s);
                return null;
            }

            SocektStream ss = new SocektStream();
            nRetCode = ss.Init(s, ipEndPoint);
            if (1 != nRetCode)
            {
                SocketWrapper.CloseSocket(ref s);
                return null;
            }

            return ss;
        }
    }

    /*------------------------------------------------------------------
      class       : AsyncSocketConnector
      Description : 套接字异步连接器。
    --------------------------------------------------------------------*/
    public class AsyncSocketConnector : ISocketConnector
    {
        public AsyncSocketConnector()
        {
        }
        public ISocketStream Connect(string ip, int port,int nMilliseconds)
        {
            return Connect(IPAddress.Parse(ip), port, nMilliseconds);
        }
        public ISocketStream Connect(IPAddress ip, int port, int nMilliseconds)
        {
            int        nRetCode   = 0;
            Socket     s          = SocketWrapper.CreateTcpSocket();
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port);

            try
            {
                m_AllDone.Reset();                                      // 设置无信号
                s.BeginConnect(ipEndPoint, new AsyncCallback(ConnectCallback), s);
                bool bWaitResult = m_AllDone.WaitOne(nMilliseconds);    // 等待信号：回调函数设置有信号，这里认为成功

                if (!bWaitResult || !s.Connected)
                {
                    Debug.LogErrorFormat("[AsyncSocketConnector] connect to server timeout or failed!");
                    SocketWrapper.CloseSocket(ref s);
                    return null;
                }
            }
            catch(SocketException e)
            {
                int    nErrorCode = e.NativeErrorCode;
                string sErrorDesc = SocketWrapper.SocketErrCode2Desc(nErrorCode);
                Debug.LogErrorFormat("[AsyncSocketConnector] connect to server failed! ErrorCode : {0}, ErrorDesc : {1}!", nErrorCode, sErrorDesc);
                SocketWrapper.CloseSocket(ref s);
                return null;
            }

            SocektStream ss = new SocektStream();
            nRetCode = ss.Init(s, ipEndPoint);
            if (1 != nRetCode)
            {
                SocketWrapper.CloseSocket(ref s);
                return null;
            }

            return ss;
        }

        private void ConnectCallback(IAsyncResult callback)
        { // 该函数成功或者失败都会被系统调用
            Socket s = callback.AsyncState as Socket;

            try
            { // 如果建立连接失败将抛出异常
                s.EndConnect(callback);
            }
            catch(SocketException e)
            {
                int    nErrorCode = e.NativeErrorCode;
                string sErrorDesc = SocketWrapper.SocketErrCode2Desc(nErrorCode);
                Debug.LogErrorFormat("[AsyncSocketConnector] connect to server failed! ErrorCode : {0}, ErrorDesc : {1}!", nErrorCode, sErrorDesc);
            }

            // 放行Connect等待事件
            m_AllDone.Set();
        }

        private static ManualResetEvent m_AllDone = new ManualResetEvent(false);
    }
}
