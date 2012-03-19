using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Easis.Wfs.FlowSystemService;
using Easis.Wfs.Interpreting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlowSystemTests.FlowSystemService
{
    using NUnit.Framework;

    [TestFixture]
    public class DataContextExtractorTest
    {
        [Test]
        public void CreateDataContextTest()
        {
            DataContextExtractor target = new DataContextExtractor(); // TODO: Initialize to an appropriate value
            string scriptDataContext = "file1 = FSID#0000000001-00012930\nasd =4\ndsa=0";
            FlowDataContext expected = new FlowDataContext();
            expected.InputFiles.Add("file1","0000000001-00012930");
            expected.InputParameters.Add("asd","4");
            expected.InputParameters.Add("dsa","0");

            FlowDataContext actual;
            actual = target.CreateDataContext(scriptDataContext);

            bool works = true;

            foreach (var iff in expected.InputFiles)
            {
                if (!actual.InputFiles.ContainsKey(iff.Key))
                {
                    works = false;
                    break;
                }
                else
                {
                    if (actual.InputFiles[iff.Key] != iff.Value)
                    {
                        works = false;
                        break;
                    }
                }
            }

            foreach (var iff in expected.InputParameters)
            {
                if (!actual.InputParameters.ContainsKey(iff.Key))
                {
                    works = false;
                    break;
                }
                else
                {
                    if (actual.InputParameters[iff.Key] != iff.Value)
                    {
                        works = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(works);
        }
    }
}
