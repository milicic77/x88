// Unity Lib
using UnityEngine;

namespace Game.Common
{
    public interface ISocketClientProxy
    {
        int Init();
        int Connect(string ip, int port,int nMilliseconds);
        int UnInit();
        int Activate();
        int IsReady();
        //int Send(byte[] data, uint uSize, int nMilliseconds);
    }
    class SocketClientProxy : ISocketClientProxy
    {
        public int Init()
        {
            if (m_bInitFlag)
            {
                Debug.LogError("[SocketClientProxy] SocketClientProxy can't be initialized twice!");
                return 0;
            }

            if (m_bConnected || null != m_SocketStream)
            {
                Debug.LogError("[SocketClientProxy] SocketClientProxy can't be connected when initializing!");
                return 0;
            }

            if (null != m_SocketConnector)
            {
                return 0;
            }

            m_SocketConnector = new AsyncSocketConnector();

            m_bInitFlag = true;
            return 1;
        }

        public int Connect(string ip, int port,int nMilliseconds)
        {
            int nRetCode = 0;

            if (!m_bInitFlag)
            {
                Debug.LogError("[SocketClientProxy] It seems SocketClientProxy hasn't been initialized!");
                return 0;
            }

            if (m_bConnected || null != m_SocketStream)
            {
                Debug.LogError("[SocketClientProxy] It seems KG_SocketClientProxy has connected to server!");
                return 0;
            }

            if (null == m_SocketConnector)
            {
                Debug.LogError("[SocketClientProxy] It seems initializing SocketClientProxy failed!");
                return 0;
            }

            m_SocketStream = m_SocketConnector.Connect(ip, port, nMilliseconds);
            if (null == m_SocketStream)
            {
                Debug.LogErrorFormat("[SocketClientProxy] SocketClientProxy connect to server failed! ip : {0}, port : {1}", ip, port);
                return 0;
            }

            nRetCode = OnClientConnected();
            if (0 == nRetCode)
            { // roll back
                Debug.LogErrorFormat("[SocketClientProxy] OnClientConnected process failed!");
                m_SocketStream.UnInit();
                m_SocketStream = null;
                return 0;
            }

            m_bConnected = true;

            return 1;
        }

        public int UnInit()
        {
            if (!m_bInitFlag)
            {
                Debug.LogError("[SocketClientProxy] SocketClientProxy can't be uninitialized twice!");
                return 0;
            }

            if (null != m_SocketConnector)
            {
            }

            if (null != m_SocketStream)
            {
                m_SocketStream.Close();
            }

            m_bConnected = false;
            m_bInitFlag  = false;

            return 1;
        }

        public int Activate()
        {
            int nRetCode = 0;

            if (!m_bInitFlag)
            {
                Debug.LogError("[SocketClientProxy] It seems SocketClientProxy hasn't been initialized!");
                return 0;
            }

            if (!m_bConnected || null == m_SocketStream)
            {
                Debug.LogError("[SocketClientProxy] It seems SocketClientProxy hasn't been connected to server!");
                return 0;
            }

            if (null == m_SocketConnector)
            {
                Debug.LogError("[SocketClientProxy] It seems initializing SocketClientProxy failed!");
                return 0;
            }

            nRetCode = ProcessNetPackage();
            if (0 == nRetCode)
            { // error occurred
                OnCloseConnection();
                m_SocketStream.Close();
                return 0;
            }

            return 1;
        }

        public int IsReady()
        {
            if (m_bInitFlag && m_bConnected && null != m_SocketConnector && null != m_SocketStream)
            {
                return 1;
            }
            return 0;
        }

        public virtual int ProcessNetPackage()
        {
            int nRetCode       = 0;
            int nPackageSerial = 0;

            while (true)
            {
                byte[] data = null;
                nRetCode = m_SocketStream.Recv(ref data, 0);

                if (-1 == nRetCode)                                     // error(including disconnected)
                    return 0;

                if (0 == nRetCode)
                    break;                                              // timeout : no more data to process

                // recv data successfully
                Debug.LogFormat("[SocketClientProxy] Package Serial = {0}", nPackageSerial);
                nPackageSerial++;

                nRetCode = OnServerDataRecvd(data);
                if (0 == nRetCode)
                    return 0;                                           // error : process failed
            }

            return 1;
        }
        public virtual int OnClientConnected()
        {
            Debug.LogFormat("[SocketClientProxy] Connect to server [ip - {0}, port - {1}]", m_SocketStream.GetEndPointIp(), m_SocketStream.GetEndPointPort());
            return 1;
        }
        public virtual int OnServerDataRecvd(byte[] data)
        {
            int nRetCode = 0;

            Debug.LogFormat("[SocketClientProxy] Receive a package from [ip - {0}, port - {1}]", m_SocketStream.GetEndPointIp(), m_SocketStream.GetEndPointPort());
            Debug.LogFormat("[SocketClientProxy] {0}", System.Text.Encoding.Default.GetString(data));

            nRetCode = m_SocketStream.Send(ref data, 0);
            return nRetCode;
        }
        public virtual int OnCloseConnection()
        {
            Debug.LogFormat("[SocketClientProxy] Disconnect from server [ip - {0}, port - {1}]", m_SocketStream.GetEndPointIp(), m_SocketStream.GetEndPointPort());
            return 1;
        }

        // member variables
        protected bool             m_bInitFlag       = false;           // 初始化标记
        protected bool             m_bConnected      = false;           // 本地客户端是否已经连接服务端
        protected ISocketStream    m_SocketStream    = null;            // 本地客户端套接字数据流
        protected ISocketConnector m_SocketConnector = null;
    }
}
