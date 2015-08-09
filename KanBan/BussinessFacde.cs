using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace KanBan
{
  public  class BussinessFacde
    {
        /// <summary>
        /// 读取最后一条序列号的相关信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetConfigXml(string attribute)
        {
            string result = null;
            XmlDocument xmlDoc = new XmlDocument();
            string addr = "DataConfig.xml";
            xmlDoc.Load(addr);
            XmlNode alarmNode = xmlDoc.SelectSingleNode("config");
            XmlNodeList keys = alarmNode.ChildNodes;
            foreach (XmlNode key in keys)
            {
                if (key.Attributes["name"] != null && key.Attributes["name"].Value.Length > 0 && key.Attributes["name"].Value == attribute)
                {
                    result = key.InnerText;
                }
            }
            return result;
        }
        /// <summary>
        /// 写入每次生成的序列号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static bool SetConfigXml(string name, string value)
        {
            var result = true;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string addr = "DataConfig.xml";
                xmlDoc.Load(addr);
                XmlNode config = xmlDoc.SelectSingleNode("config");
                XmlNodeList keys = config.ChildNodes;
                foreach (XmlNode key in keys)
                {
                    if (key.Attributes["name"] != null && key.Attributes["name"].Value.Length > 0 && key.Attributes["name"].Value == name)
                    {
                        key.InnerText = value;
                    }
                }
                xmlDoc.Save(addr);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 读取opc配置文件 根据工位和order 得到特定变量的值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string GetOpcConfigXml(string station, string order, string type)
        {
            string result = null;
            XmlDocument xmlDoc = new XmlDocument();
            string addr = "opc.xml";
            xmlDoc.Load(addr);
            XmlNode nd;
            nd = xmlDoc.SelectSingleNode("OPC");
            XmlNodeList xnl = nd.ChildNodes;
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("station") == station && xe.GetAttribute("order") == order && xe.GetAttribute("type") == type)
                {
                    result = xe.GetAttribute("client");
                }
            }
            return result;
        }
    }
}
