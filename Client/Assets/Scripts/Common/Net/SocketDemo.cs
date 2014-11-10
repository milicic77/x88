using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Game.Common
{
    /*------------------------------------------------------------------
      class       : MyClientProxy
      Description : 一个客户端代理使用例子。
    --------------------------------------------------------------------*/
    class MyClientProxy : SocketClientProxy
    {
        public override int OnClientConnected()
        {
            Debug.LogFormat("[MyClientProxy] Connect to server [ip - {0}, port - {1}]", m_SocketStream.GetEndPointIp(), m_SocketStream.GetEndPointPort());
            return 1;
        }
        public override int OnServerDataRecvd(byte[] data)
        {
            int nRetCode = 0;

            Debug.LogFormat("[MyClientProxy] Receive a package from [ip - {0}, port - {1}]", m_SocketStream.GetEndPointIp(), m_SocketStream.GetEndPointPort());
            Debug.LogFormat("[MyClientProxy] {0}", System.Text.Encoding.Default.GetString(data));

            nRetCode = Send(data, 0);
            return nRetCode;
        }
        public override int OnCloseConnection()
        {
            Debug.LogFormat("[MyClientProxy] Disconnect from server [ip - {0}, port - {1}]", m_SocketStream.GetEndPointIp(), m_SocketStream.GetEndPointPort());
            return 1;
        }
    }
    public class SocketDemo : MonoBehaviour
    {
        private ISocketClientProxy m_proxy = null;

        void Start()
        {
            int nRetCode = 0;
            m_proxy = new MyClientProxy();

            nRetCode = m_proxy.Init();
            if (0 == nRetCode)
            {
                return;
            }

            nRetCode = m_proxy.Connect("127.0.0.1", 7463, 5000);
            if (0 == nRetCode)
            {
                m_proxy.UnInit();
                return;
            }
        }

        void Update()
        {
            int nRetCode = 0;

            nRetCode = m_proxy.IsReady();
            if (0 == nRetCode)
            {
                return;
            }

            nRetCode = m_proxy.Activate();
            if (0 == nRetCode)
            {
                m_proxy.UnInit();
            }
        }
    }
}
