using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Web;
using NCI.Web.CDE.Configuration;
using System.Web.Caching;
using System.Linq;
using System.Xml.Schema;
using NCI.Logging;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Section Details gets all the snippets from the closest parents which have snippets in any empty slot after the Page Assembly Instructions’ snippets have been loaded,
    /// Section Details will get the snippets associated with a parent from a SectionDetail XML file. 
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("SectionDetail", Namespace = "http://www.example.org/CDESchema", IsNullable = false)] 
    public class SectionDetail
    {

        private SectionDetail _parent;

        /// <summary>
        /// boolean to check if parent is processed
        /// </summary>
        private bool _hasTriedToLoadParent = false;

 
        private string _parentpath;

        /// <summary>
        /// Gets or sets the parent path.
        /// </summary>
        /// <value>The parent path.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ParentPath
        {
            get
            {
                return _parentpath;
            }

            set
            {
                _parentpath = value;
            }
        }

        /// <summary>
        /// Gets the Parent of this SectionDetail
        /// </summary>
        /// <value>The parent.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public SectionDetail Parent
        {
            get
            {
                if (_parent == null && !_hasTriedToLoadParent && !string.IsNullOrEmpty(ParentPath))
                {
                    try
                    {
                        _parent = SectionDetailFactory.GetSectionDetail(_parentpath);
                        //If we cannot load the parent don't keep trying????
                        _hasTriedToLoadParent = true;
                    }
                    catch(Exception ex)
                    {
                        Logger.LogError("CDE:SectionDetail.cs:Parent", "Failed to load the section detail for the parent path: "+ _parentpath, NCIErrorLevel.Error, ex);
                    }
                }
                return _parent;
            }
        }


        private SnippetInfoCollection _snippets = new SnippetInfoCollection();
         
        /// <summary>
        /// Gets and sets the section name
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SectionName { get; set; }



        /// <summary>
        /// Gets the snippet infos.
        /// </summary>
        /// <value>The snippet infos.</value>
        [System.Xml.Serialization.XmlArray(ElementName = "Snippets", Form = XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItem("SnippetInfo", Form = XmlSchemaForm.Unqualified)]
        public SnippetInfoCollection SnippetInfos
        {
            get { return _snippets; }
        }


        /// <summary>
        /// Gets the snippets.
        /// </summary>
        /// <value>The snippets.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public IEnumerable<SnippetInfo> Snippets
        {
            get
            {
                return _snippets;
            }
        }

        /// <summary>
        /// Gets a list of snippets not associated with any of the passed in template slots.
        /// If finds one or more SnippetInfo objects destined for a slot not named in the exclusion list, adds it to 
        /// the list of items to return.  If not, goes to the parent to try to find them.
        /// </summary>
        /// <param name="templateSlotExclusionList">The template slot exclusion list.</param>
        /// <returns></returns>
        public List<SnippetInfo> GetSnippetsNotAssociatedWithSlots(IEnumerable<string> templateSlotExclusionList)
        {
            IEnumerable<SnippetInfo> localSnippets = Snippets;

            List<SnippetInfo> snippetsToAdd = (from snippet in localSnippets
                                               where !(
                                                        from templateName in templateSlotExclusionList
                                                        select templateName
                                                       ).Contains<string>(snippet.SlotName)
                                               select snippet).ToList<SnippetInfo>();

            List<string> templateSlotsFromThisSectionToExclude = (from snippet in snippetsToAdd
                                                                  select snippet.SlotName)
                                                                   .Distinct<string>()
                                                                   .ToList<string>();

            List<string> filledTemplateSlots = new List<string>(templateSlotsFromThisSectionToExclude);
            filledTemplateSlots.AddRange(templateSlotExclusionList);


            if (Parent != null)
            {
                snippetsToAdd.AddRange(Parent.GetSnippetsNotAssociatedWithSlots(filledTemplateSlots));
            }

            return snippetsToAdd;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            SectionDetail target = obj as SectionDetail;

            if (target == null)
                return false;

            if (SectionName != target.SectionName)
                return false;

            if (ParentPath != target.ParentPath)
                return false;

            if (!SnippetInfos.Equals(target.SnippetInfos))
                return false;

            return true;
        }
     }
}
