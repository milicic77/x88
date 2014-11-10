// .Net Lib
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

// Unity Lib
using UnityEngine;

namespace Game.Common
{
    class SocketWrapper
    {
        public static Socket CreateTcpSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (null == s)
            {
                Debug.LogError("[Socket Error] Create a tcp socket failed!");
            }
            return s;
        }

        public static Socket CreateUdpSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Tcp);
            if (null == s)
            {
                Debug.LogError("[Socket Error] Create a udp socket failed!");
            }
            return s;
        }

        public static int CloseSocket(ref Socket s)
        {
            int nResult = 0;

            if (null == s)
            {
                return 1;
            }

            try
            {
                s.Close();
                s = null;

                nResult = 1;
            }
            catch (SocketException e)
            {
                int    nErrorCode = e.NativeErrorCode;
                string sErrorDesc = SocketWrapper.SocketErrCode2Desc(nErrorCode);
                Debug.LogErrorFormat("[Socket Error] Close socket failed! ErrorCode : {0}, ErrorDesc : {1}!", nErrorCode, sErrorDesc);
            }

            return nResult;
        }

        public static int IsValidSocket(Socket s)
        {
            if (null != s && s.Connected)
            {
                return 1;
            }
            return 0;
        }

        public static int IsSocketInterrupted(SocketException e)
        {
            int nErrorCode = e.NativeErrorCode;
        #if UNITY_ANDROID                                               // android
        #endif

        #if UNITY_IPHONE                                                // iphone
        #endif

        #if UNITY_STANDALONE_WIN                                        // windows
            if (nErrorCode.Equals(10004))
            {
                return 1;
            }
        #endif

        #if UNITY_STANDALONE_OSX                                        // mac os
        #endif

        #if UNITY_STANDALONE_LINUX                                      // linux
            if (nErrorCode.Equals(4))
            {
                return 1;
            }
        #endif

            return 0;
        }

        public static int IsSocketEWouldBlock(SocketException e)
        {
            int nErrorCode = e.NativeErrorCode;
        #if UNITY_ANDROID                                               // android
        #endif

        #if UNITY_IPHONE                                                // iphone
        #endif

        #if UNITY_STANDALONE_WIN                                        // windows
            if (nErrorCode.Equals(10035) || nErrorCode.Equals(997))
            {
                return 1;
            }
        #endif

        #if UNITY_STANDALONE_OSX                                        // mac os
        #endif

        #if UNITY_STANDALONE_LINUX                                      // linux
            if (nErrorCode.Equals(11))
            {
                return 1;
            }
#endif

            return 0;
        }

        public static int CheckCanRecv(Socket s, int nMilliseconds)
        {
            int nRetCode = 0;

            if (IsValidSocket(s) != 1)
            {
                Debug.LogError("[Socket Error] An invalid socket can't be checked!");
                return -1;
            }

            ArrayList checkRead = new ArrayList();
            checkRead.Add(s);

            while (true)
            {
                try
                {
                    Socket.Select(checkRead, null, null, nMilliseconds * 1000);
                    if (checkRead.Count <= 0)
                        return 0;
                }
                catch (SocketException e)
                {
                    nRetCode = IsSocketInterrupted(e);
                    if (1 == nRetCode)
                        continue;

                    nRetCode = IsSocketEWouldBlock(e);
                    if (1 == nRetCode)
                        return 0;

                    Debug.LogError("[Socket Error] An error occurred when CheckCanRecv a socket!");
                    return -1;
                }

                break;
            }

            return 1;
        }

        public static int CheckCanSend(Socket s, int nMilliseconds)
        {
            int nRetCode = 0;

            if (IsValidSocket(s) != 1)
            {
                Debug.LogError("[Socket Error] An invalid socket can't be checked!");
                return -1;
            }

            ArrayList checkWrite = new ArrayList();
            checkWrite.Add(s);

            while (true)
            {
                try
                {
                    Socket.Select(null, checkWrite, null, nMilliseconds * 1000);
                    if (checkWrite.Count <= 0)
                        return 0;
                }
                catch (SocketException e)
                {
                    nRetCode = IsSocketInterrupted(e);
                    if (1 == nRetCode)
                        continue;

                    nRetCode = IsSocketEWouldBlock(e);
                    if (1 == nRetCode)
                        return 0;

                    Debug.LogError("[Socket Error] An error occurred when CheckCanSend a socket!");
                    return -1;
                }

                break;
            }

            return 1;
        }

        /*
         * s             : 套接字
         * data          : 数据缓冲区
         * uSize         : 期望接收到的数据长度
         * uRecvBytes    : 实际接收到的数据长度
         * nMilliseconds : 超时时间
         */
        public static int CheckRecvSocketData(Socket s, byte[] data, uint uSize, ref uint uRecvBytes, int nMilliseconds)
        {
            int  nRetCode         = 0;
            uint uLeftBytesToRecv = uSize;
            uint uBytesHasRecv    = 0;

            if (IsValidSocket(s) != 1)
            {
                Debug.LogError("[Socket Error] An invalid socket can't recv data!");
                return -1;
            }

            if (uSize <= 0 || data.Length <= 0 || data.Length < uSize)
            {
                Debug.LogError("[Socket Error] Parameters of CheckRecvSocketData invalid!");
                return -1;
            }

            uRecvBytes = 0;

            while (uLeftBytesToRecv > 0)
            {
                nRetCode = CheckCanRecv(s, nMilliseconds);
                if (-1 == nRetCode)
                    return -1;

                if (0 == nRetCode)
                {
                    if (uBytesHasRecv > 0)
                        return 1;
                    return 0;
                }

                // recv
                try
                {
                    byte[] buffer = new byte[uLeftBytesToRecv];
                    nRetCode = s.Receive(buffer);
                    if (0 == nRetCode)
                    {
                        Debug.LogError("[Socket Error] Remote socket has been closed!");
                        return -1;                                      // disconnected == error
                    }

                    buffer.CopyTo(data, uSize - uLeftBytesToRecv);

                    uBytesHasRecv    += (uint)nRetCode;
                    uLeftBytesToRecv -= (uint)nRetCode;
                    uRecvBytes = uBytesHasRecv;
                }
                catch (SocketException e)
                {
                    nRetCode = IsSocketInterrupted(e);
                    if (1 == nRetCode)
                        continue;

                    nRetCode = IsSocketEWouldBlock(e);
                    if (1 == nRetCode)
                    {
                        if (uBytesHasRecv > 0)
                            return 1;
                        return 0;
                    }

                    Debug.LogError("[Socket Error] An error occurred when CheckRecvSocketData a socket!");
                    return -1;
                }
            }
            return 1;
        }

        /*
         * s             : 套接字
         * data          : 数据缓冲区
         * uSize         : 期望发送的数据长度
         * nMilliseconds : 超时时间
         */
        public static int CheckSendSocketData(Socket s, byte[] data, uint uSize, int nMilliseconds)
        {
            int  nRetCode         = 0;
            uint uLeftBytesToSend = uSize;

            if (IsValidSocket(s) != 1)
            {
                Debug.LogError("[Socket Error] An invalid socket can't send data!");
                return -1;
            }

            if (uSize <= 0 || data.Length <= 0 || data.Length < uSize)
            {
                Debug.LogError("[Socket Error] Parameters of CheckRecvSocketData invalid!");
                return -1;
            }

            while (uLeftBytesToSend > 0)
            {
                nRetCode = CheckCanSend(s, nMilliseconds);
                if (-1 == nRetCode)
                    return -1;

                if (0 == nRetCode)
                    return 0;

                // send
                try
                {
                    byte[] buffer = new byte[uLeftBytesToSend];
                    System.Array.Copy(data, uSize - uLeftBytesToSend, buffer, 0, uLeftBytesToSend);
                    nRetCode = s.Send(buffer);
                    if (0 == nRetCode)
                    {
                        Debug.LogError("[Socket Error] Remote socket has been closed!");
                        return -1;                                      // disconnected == error
                    }

                    uLeftBytesToSend -= (uint)nRetCode;
                }
                catch (SocketException e)
                {
                    nRetCode = IsSocketInterrupted(e);
                    if (1 == nRetCode)
                        continue;

                    nRetCode = IsSocketEWouldBlock(e);
                    if (1 == nRetCode)
                        return 0;

                    Debug.LogError("[Socket Error] An error occurred when CheckSendSocketData a socket!");
                    return -1;
                }
            }
            return 1;
        }

        public static string SocketErrCode2Desc(int nErrCode)
        {
            if (SocketErrDesc.ContainsKey(nErrCode))
            {
                return SocketErrDesc[nErrCode];
            }
            return "Unknown error";
        }

    #if UNITY_ANDROID                                               // android
        public static Dictionary<int, string> SocketErrDesc = new Dictionary<int, string>();
    #endif

    #if UNITY_IPHONE                                                // iphone
        public static Dictionary<int, string> SocketErrDesc = new Dictionary<int, string>();
    #endif

    #if UNITY_STANDALONE_WIN                                        // windows
        public static Dictionary<int, string> SocketErrDesc = new Dictionary<int, string>()
        {
            {0,     "Success"           },
            {10061, "Connection refused"},
        };
    #endif

    #if UNITY_STANDALONE_OSX                                        // mac os
        public static Dictionary<int, string> SocketErrDesc = new Dictionary<int, string>();
    #endif

#if UNITY_STANDALONE_LINUX                                      // linux

#endif
    }
}

