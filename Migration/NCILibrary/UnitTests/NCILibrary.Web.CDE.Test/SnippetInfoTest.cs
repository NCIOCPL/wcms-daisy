using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCI.Web.CDE;
namespace NCI.Web.CDE.Test
{

    [TestClass]
    public class SnippetInfoTest
    {
        [TestMethod]
        public void ReadXml_SingleSnippetInfo_CanDeserialize()
        {
            SnippetInfo snippetInfo = new SnippetInfo();
            string snippetInfoXml = @"
                                        <SnippetInfo>
                                          <SnippetTemplatePath>/path1</SnippetTemplatePath>
                                          <SlotName>Slot1</SlotName>
                                          <Data><![CDATA[Data 1]]></Data>
                                        </SnippetInfo>";
            using (StringReader s = new StringReader(snippetInfoXml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SnippetInfo));
                snippetInfo = (SnippetInfo)serializer.Deserialize(s);
            }

            // Assert
            Assert.AreEqual<string>("/path1", snippetInfo.SnippetTemplatePath);
            Assert.AreEqual<string>("Slot1", snippetInfo.SlotName);
            Assert.AreEqual<string>("Data 1", snippetInfo.Data);
        }

        [TestMethod]
        public void ReadXml_ListOfSnippetInfos_CanDeserialize()
        {
            List<SnippetInfo> snippetInfos = new List<SnippetInfo>();
            string snippetInfoXml = @"
                                      <Snippets>
                                        <SnippetInfo>
                                          <SnippetTemplatePath>/path1</SnippetTemplatePath>
                                          <SlotName>Slot1</SlotName>
                                          <Data><![CDATA[Data 1]]></Data>
                                        </SnippetInfo>
                                        <SnippetInfo>
                                          <SnippetTemplatePath>/path2</SnippetTemplatePath>
                                          <SlotName>Slot2</SlotName>
                                          <Data><![CDATA[Data 2]]></Data>
                                        </SnippetInfo>
                                        <SnippetInfo>
                                          <SnippetTemplatePath>/path3</SnippetTemplatePath>
                                          <SlotName>Slot3</SlotName>
                                          <Data><![CDATA[Data 3]]></Data>
                                        </SnippetInfo>
                                      </Snippets>";
            using (StringReader s = new StringReader(snippetInfoXml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<SnippetInfo>), new XmlRootAttribute("Snippets"));
                snippetInfos = (List<SnippetInfo>)serializer.Deserialize(s);
            }

            // Assert
            Assert.AreEqual<int>(3, snippetInfos.Count);

            Assert.AreEqual<string>("/path1", snippetInfos[0].SnippetTemplatePath);
            Assert.AreEqual<string>("Slot1", snippetInfos[0].SlotName);
            Assert.AreEqual<string>("Data 1", snippetInfos[0].Data);

            Assert.AreEqual<string>("/path2", snippetInfos[1].SnippetTemplatePath);
            Assert.AreEqual<string>("Slot2", snippetInfos[1].SlotName);
            Assert.AreEqual<string>("Data 2", snippetInfos[1].Data);

            Assert.AreEqual<string>("/path3", snippetInfos[2].SnippetTemplatePath);
            Assert.AreEqual<string>("Slot3", snippetInfos[2].SlotName);
            Assert.AreEqual<string>("Data 3", snippetInfos[2].Data);
        }


    }
}
