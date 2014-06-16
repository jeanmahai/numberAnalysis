using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Analysis28.DataService.Utility
{
    /// <summary>
    /// 读取配置
    /// </summary>
    public class GetConfig
    {
        /// <summary>
        /// 配置字典
        /// </summary>
        private static Dictionary<string, string> _Configs = null;
        protected static Dictionary<string, string> Configs
        {
            get
            {
                if (_Configs == null)
                    _Configs = new Dictionary<string, string>();
                return _Configs;
            }
        }

        /// <summary>
        /// 从XML文件中读取节点值
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static string GetXMLValue(string nodeName)
        {
            if (Configs.ContainsKey(nodeName))
                return Configs[nodeName];

            string nodeValue = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\SvcConfig.xml";
                xmlDoc.Load(path);
                XmlNode xn = xmlDoc.SelectSingleNode("config");
                nodeValue = xn.SelectNodes(nodeName).Item(0).InnerText;
            }
            catch
            {
                throw;
            }
            Configs[nodeName] = nodeValue;

            return nodeValue;
        }
    }
}
