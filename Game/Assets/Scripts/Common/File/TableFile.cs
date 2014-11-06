// tab文件读写，参照c++库
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Common
{
    public class TableFile
    {
        Stream m_tableStream;
        StreamReader m_tableReader;

        Dictionary<string, int> m_attrDict = null;  /*表格属性列 */
        int m_RowCount = 0;
        int m_CursorPos = 0;
        string[] m_CachedColumns; // 当前行缓存(已拆分)


        /** 构造器 */
        private TableFile() { }

        /*  从文件对象中创建 */
        public static TableFile LoadFromFile(string path)
        {
            TableFile tableFile = null;
#if !UNITY_STANDALONE_WIN
            try
            {
                TextAsset text = (TextAsset)Resources.Load(path);
                if (text == null)
                {
                    Common.ExceptionTool.ThrowException("Open file '" + path + "' failed");
                }

                tableFile = LoadFromContent(text.bytes);
            }
            catch (Exception e)
            {
                Common.ExceptionTool.ProcessException(e);
            }
#else // 在DLL, PC情况下用
            try
            {
                FileStream stream = new FileStream(Application.dataPath + "/Resources/" + path + ".bytes", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                byte[] fileBuffer = new byte[stream.Length];
                stream.Read(fileBuffer, 0, Convert.ToInt32(stream.Length));
                tableFile = LoadFromContent(fileBuffer);
            }
            catch (Exception e)
            {
                // Web 环境下没有FileInfo
                Common.ExceptionTool.ProcessException(e);
            }
#endif
            return tableFile;
        }

        /* 从字符串中创建对象 */
        public static TableFile LoadFromContent(byte[] data)
        {
            TableFile tableFile = new TableFile();

            //byte[] tableBytes = Convert.FromBase64String(txt);  // string -> bytes -> stream
            tableFile.m_tableStream = new MemoryStream(data);
            //tableFile.m_tableReader = new StreamReader(tableFile.m_tableStream);

            tableFile.ParseColumnNames(tableFile.m_tableStream);

            tableFile.ParseRowCount(data);

            tableFile.InitStreamReader(); // reset cursor position

            return tableFile;
        }

        /* 初始化实例对象的Reader，回到起点，用于下一次读取 */
        public void InitStreamReader()
        {
            this.m_tableStream.Seek(0, 0);
            m_CursorPos = 0;

            this.m_tableReader = new StreamReader(this.m_tableStream, GameEncoding.Encoding);
        }

        // 开始实例方法

        /// 初始化列名表
        private bool ParseColumnNames(Stream stream)
        {
            var streamReader = new StreamReader(stream, GameEncoding.Encoding);
            m_attrDict = new Dictionary<string, int>();

            string attrLine = streamReader.ReadLine();
            string[] attrArray = attrLine.Split('\t');

            for (int i = 0; i < attrArray.Length; i++)
            {
                /* 列从1开始，  i+1所以 */
                try
                {
                    this.m_attrDict.Add(attrArray[i], i + 1);  /* 放入字典 -> ( ColumnName,  ColumnNum ) (名字，列数）*/
                }
                catch (Exception e)
                {
                    Common.LogWriter.WriteError("添加列名" + attrArray[i] + " 时出错: " + e.Message);
                    continue;
                }
            }
            return true;
        }

        /* 根据列名获取列的数字 */
        private int findColumnByName(string columnName)
        {
            if (m_attrDict.ContainsKey(columnName))
            {
                return this.m_attrDict[columnName];
            }

            Common.LogWriter.WriteError(string.Format("找不到列名[{0}]!在表", columnName));
            return 0;
        }

        /** 获取第一行第column列的名字， 即表头属性名 */
        public string GetColumnName(int column)
        {
            foreach (KeyValuePair<string, int> attr in this.m_attrDict)
            {
                if (attr.Value == column)
                {
                    return attr.Key;
                }
            }

            /* 没找到 */
            return null;
        }


        /** 列数 */
        public int GetColumnsCount()
        {
            return m_attrDict.Count;
        }

        /* 设置值, 返回成功与否， 不抛出异常 */
        public bool GetString(int row, string columnName, string defaultVal, ref string outVal)
        {
            try
            {
                int col = findColumnByName(columnName);
                outVal = GetString(row, col);
            }
            catch (IndexOutOfRangeException)
            {
                outVal = defaultVal;  // 取默认值
            }

            return true;
        }

        /* 获取表格内容字符串 */
        public string GetString(int row, int column)
        {
            if (row < (m_CursorPos - 1))
                this.InitStreamReader(); // 一般不会往前读，避免了seek

            if (row > m_RowCount)
                throw new IndexOutOfRangeException();

            if (row == (m_CursorPos - 1))
            {
                return m_CachedColumns[column - 1].Trim(); // 去空格
            }

            do
            {
                string line = m_tableReader.ReadLine();
                if (line == null)
                    break;
                if (line == string.Empty)
                    continue; // 注意没有递增m_CursorPos

                m_CursorPos++; // 递增行数游标

                if (row == (m_CursorPos - 1))
                {
                    m_CachedColumns = line.Split('\t');

                    return m_CachedColumns[column - 1].Trim();  // 去空格
                }
            } while (true);

            // 仍然没找到？内部状态乱了
            throw new IndexOutOfRangeException();
        }

        /** Float */
        public float GetFloat(int row, int column)
        {
            return Convert.ToSingle(this.GetString(row, column));
        }
        public float GetFloat(int row, string columnName)
        {
            return this.GetFloat(row, this.findColumnByName(columnName));
        }

        public bool GetFloat(int row, string columnName, float defaultVal, ref float outVal)
        {
            try
            {
                outVal = this.GetFloat(row, this.findColumnByName(columnName));
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                outVal = defaultVal;
                return false;
            }
            catch (FormatException)
            {
                // FormatException -> 值为""， 空字符串
                outVal = defaultVal;
                return false;
            }
        }

        /**Integer*/
        public int GetInteger(int row, int column)
        {
            return Convert.ToInt32(this.GetString(row, column));
        }

        public int GetInteger(int row, string columnName)
        {
            return this.GetInteger(row, this.findColumnByName(columnName));
        }

        public bool GetInteger(int row, string columnName, int defaultVal, ref int outVal)
        {
            try
            {
                outVal = this.GetInteger(row, this.findColumnByName(columnName));

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                outVal = defaultVal;
                return false;
            }
            catch (FormatException)
            {
                // FormatException -> 值为""， 空字符串
                outVal = defaultVal;
                return false;
            }
        }

        /* UInt 32 */
        public uint GetUInteger(int row, int column)
        {
            return Convert.ToUInt32(this.GetString(row, column));
        }

        public uint GetUInteger(int row, string columnName)
        {
            return this.GetUInteger(row, this.findColumnByName(columnName));
        }

        public bool GetUInteger(int row, string columnName, uint defaultVal, ref uint outVal)
        {
            try
            {
                outVal = this.GetUInteger(row, this.findColumnByName(columnName));

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                outVal = defaultVal;
                return false;
            }
            catch (FormatException)
            {
                // FormatException -> 值为""， 空字符串
                outVal = defaultVal;
                return false;
            }
        }

        private bool ParseRowCount(byte[] bytes)
        {
            m_RowCount = 0;
            for (uint i = 0; i < bytes.Length; ++i)
            {
                // 匹配\r\n和\n组合
                if (bytes[i] == '\r')
                {
                    if (bytes[i + 1] == '\n')
                    {
                        ++i;
                    }
                    m_RowCount++;
                }
                else if (bytes[i] == '\n')
                {
                    m_RowCount++;
                }
            }

            if (bytes.Length > 0)
            {
                // 末尾没空行的
                if (bytes[bytes.Length - 1] != '\r' && bytes[bytes.Length - 1] != '\n')
                    m_RowCount++;
            }

            if (m_RowCount > 0)
                m_RowCount--; // 减去第一行column header
            return true;
        }

        /// 获取行数（不含第一行的ColumnHeader）
        public int GetRowsCount()
        {
            return m_RowCount;
        }

        public void Close()
        {
            m_tableReader.Close();
            m_tableStream = null;
            m_tableReader = null;
        }
    }
}
