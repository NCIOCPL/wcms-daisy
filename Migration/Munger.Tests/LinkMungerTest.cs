using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Xunit;

using Munger;
using NCI.CMS.Percussion.Manager.CMS;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using Munger.Configuration;

namespace Munger.Tests.LinkMungerTest
{
    /// <summary>
    /// Tests for LinkMunger.RewriteLinkReferences()
    /// </summary>
    public class RewriteLinkReferencesTest
    {
        Mock<ICMSController> mockCmsController = new Mock<ICMSController>();
        Mock<ILogger> mockLogger = new Mock<ILogger>();

        /// <summary>
        /// Verify that empty documents don't get mangled.
        /// </summary>
        [Fact]
        public void EmptyDocument()
        {
            Mock<IMungerConfiguration> mockConfiguration = new Mock<IMungerConfiguration>();
            mockConfiguration
                .Setup(x => x.HostList).Returns(() =>
                {
                    HostListElement list = new HostListElement();
                    list.Add(new HostElement { Name = "www.cancer.gov", IsCanonical = true });
                    return list;
                });
            mockConfiguration
                .Setup(x => x.SubstitutionList).Returns(new RewritingListElement());


            HtmlParser parser = new HtmlParser();

            IDocument document = parser.Parse("<!DOCTYPE html><html><body></body></html>");

            ILinkMunger munger = MungerFactory.CreateLinkMunger(mockCmsController.Object, mockLogger.Object, mockConfiguration.Object);

            // TODO:  Test running an empty document through!
        }
    }
}
