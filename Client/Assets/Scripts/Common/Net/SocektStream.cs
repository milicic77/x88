// .Net Lib
using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;

// Unity Lib
using UnityEngine;

namespace Game.Common
{
    /*------------------------------------------------------------------
      Interface   : ISocketStream
      Description : 套接字流接口。
    --------------------------------------------------------------------*/
    public interface ISocketStream
    {
        int UnInit();
        int Close();
        int CheckCanSend(int nMilliseconds);
        int CheckCanRecv(int nMilliseconds);
        int Send(ref byte[] data, int nMilliseconds);
        int Recv(ref byte[] data, int nMilliseconds);
        string GetEndPointIp();
        int    GetEndPointPort();
    }

    /*------------------------------------------------------------------
      class       : ISocketStream
      Description : 无缓冲套接字流。
    --------------------------------------------------------------------*/
    public class SocektStream : ISocketStream
    {
        public int Init(Socket s, IPEndPoint ipEndPoint)
        {
            if (m_bInitFlag)
            {
                Debug.LogError("[Socket Error] SocektStream can't be initialized twice!");
                return 0;
            }

            if (null == s)
            {
                Debug.LogError("[Socket Error] SocektStream can't take over a null socket!");
                return 0;
            }

            m_hRemoteSocket = s;
            m_ipEndPoint    = ipEndPoint;
            m_bInitFlag     = true;

            return 1;
        }

        public int UnInit()
        {
            int nRetCode = 0;

            if (!m_bInitFlag)
            {
                Debug.LogError("[Socket Error] SocektStream can't be uninitialized twice!");
                return 0;
            }

            nRetCode = SocketWrapper.CloseSocket(ref m_hRemoteSocket);
            if (1 != nRetCode)
            {
                Debug.LogError("[Socket Error] SocektStream close socket failed!");
                return 0;
            }

            m_bInitFlag     = false;
            m_hRemoteSocket = null;

            return 1;
        }

        public int Close()
        {
            int nRetCode = SocketWrapper.CloseSocket(ref m_hRemoteSocket);
            m_hRemoteSocket = null;
            return nRetCode;
        }

        public int CheckCanSend(int nMilliseconds)
        {
            return SocketWrapper.CheckCanSend(m_hRemoteSocket, nMilliseconds);
        }
        public int CheckCanRecv(int nMilliseconds)
        {
            return SocketWrapper.CheckCanRecv(m_hRemoteSocket, nMilliseconds);
        }

        public int Send(ref byte[] data, int nMilliseconds)
        {
            short  nDataBytes = (short)(data.Length + sizeof(short));
            byte[] header     = BitConverter.GetBytes(nDataBytes);
            byte[] package    = header.Concat(data).ToArray();

            return SocketWrapper.CheckSendSocketData(m_hRemoteSocket, package, (uint)package.Length, nMilliseconds);
        }
        public int Recv(ref byte[] data, int nMilliseconds)
        {
            int   nRetCode   = 0;
            uint  uRecvBytes = 0;
            short uDataBytes = 0;

            // 收取包头
            byte[] header = new byte[2];
            nRetCode = SocketWrapper.CheckRecvSocketData(m_hRemoteSocket, header, 2, ref uRecvBytes, nMilliseconds);
            if (-1 == nRetCode)
                return -1;

            if (0 == nRetCode)
                return 0;

            if (uRecvBytes < 2)
                return -1;

            uDataBytes = BitConverter.ToInt16(header, 0);
            uDataBytes -= 2;

            if (uDataBytes < 0)
                return -1;

            // 收取包体
            data = new byte[uDataBytes];
            nRetCode = SocketWrapper.CheckRecvSocketData(m_hRemoteSocket, data, (uint)uDataBytes, ref uRecvBytes, nMilliseconds);
            if (nRetCode <= 0)                                          // error - 无缓冲接收必须成功
                return -1;

            if (uRecvBytes < uDataBytes)                                // error - 无缓冲接收必须成功
                return -1;

            return 1;
        }
        public string GetEndPointIp()
        {
            return m_ipEndPoint.Address.ToString();
        }
        public int GetEndPointPort()
        {
            return m_ipEndPoint.Port;
        }

        // member variables
        protected bool       m_bInitFlag     = false;                   // 初始化标记
        protected Socket     m_hRemoteSocket = null;                    // 远端套接字
        protected IPEndPoint m_ipEndPoint    = null;                    // 套接字地址
    }

    /*------------------------------------------------------------------
      class       : ISocketStream
      Description : 带冲套接字流。
    --------------------------------------------------------------------*/
    //public class BufferedSocketStream : ISocketStream
    //{
    //    public BufferedSocketStream()
    //    {
    //    }
    //}
}