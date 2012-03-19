using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.Interpreting.Nodes;
using NUnit.Framework;

namespace FlowSystemTests.FlowSystemService
{
    [TestFixture]
    class SweepStepNodeTest
    {
        [Test]
        public void DecartTest()
        {
            Trace.WriteLine("null");
            Assert.Catch(typeof (ArgumentException), () => SweepStepNode.DecartIt<int>(null));

            Trace.WriteLine("empty");
            try { SweepStepNode.DecartIt<int>(new List<IEnumerable<int>>()); }
            catch (ArgumentException expected) { }

            Trace.WriteLine("empty sub");
            try { SweepStepNode.DecartIt<int>(new List<IEnumerable<int>> { new int[] { 0, 1, 2 }, new int[] { } }); }
            catch (ArgumentException expected) { }

            IList<IEnumerable<int>> inp = null;

            Trace.WriteLine("[1, 0] -> [1],[0]");
            inp = new List<IEnumerable<int>> { new int[] { 1, 0 } };
            CollectionAssert.AreEqual(SweepStepNode.DecartIt<int>(inp), new List<List<int>> { new List<int>() { 1 },new List<int>(){ 0 } });

            
            Trace.WriteLine("[1] [0] -> [1,0]");
            inp = new List<IEnumerable<int>> { new int[] { 1 }, new int[] { 0 } };
            CollectionAssert.AreEqual(SweepStepNode.DecartIt<int>(inp), new List<List<int>> { new List<int>() { 1, 0 } });

            Trace.WriteLine("[1, 2] [3, 4] -> [1,3], [1,4], [2,3], [2,4]");
            inp = new List<IEnumerable<int>> { new int[] { 1, 2 }, new int[] { 3, 4 } };
            CollectionAssert.AreEquivalent(SweepStepNode.DecartIt<int>(inp), new List<List<int>> { new List<int>() { 1, 3 }, new List<int>() { 1, 4 }, new List<int>() { 2, 3 }, new List<int>() { 2, 4 } });
            
        }
        [Test]
        public void ZipTest()
        {
            Trace.WriteLine("null");
            Assert.Catch(typeof(ArgumentException), () => SweepStepNode.ZipIt<int>(null));

            Trace.WriteLine("empty");
            try { SweepStepNode.ZipIt<int>(new List<IEnumerable<int>>()); }
            catch (ArgumentException expected) { }

            Trace.WriteLine("empty sub");
            try { SweepStepNode.ZipIt<int>(new List<IEnumerable<int>> { new int[] { 0, 1, 2 }, new int[] { } }); }
            catch (ArgumentException expected) { }

            IList<IEnumerable<int>> inp = null;

            Trace.WriteLine("[1, 0] -> [1],[0]");
            inp = new List<IEnumerable<int>> { new int[] { 1, 0 } };
            CollectionAssert.AreEqual(SweepStepNode.ZipIt<int>(inp), new List<List<int>> { new List<int>() { 1 }, new List<int>() { 0 } });


            Trace.WriteLine("[1] [0] -> [1,0]");
            inp = new List<IEnumerable<int>> { new int[] { 1 }, new int[] { 0 } };
            CollectionAssert.AreEqual(SweepStepNode.ZipIt<int>(inp), new List<List<int>> { new List<int>() { 1, 0 } });

            Trace.WriteLine("[1, 2] [3, 4] -> [1,3], [2,4]");
            inp = new List<IEnumerable<int>> { new int[] { 1, 2 }, new int[] { 3, 4 } };
            CollectionAssert.AreEquivalent(SweepStepNode.ZipIt<int>(inp), new List<List<int>> { new List<int>() { 1, 3 }, new List<int>() { 2, 4 } });

        }
    }
}
