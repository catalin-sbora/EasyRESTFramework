using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyRESTFramework.Client.Internal;
using System.Collections.Generic;

namespace TestInternals
{
    [TestClass]
    public class DependencyGraphTest
    {
        [TestMethod]
        public void TestGraphBuild()
        {
            DependencyGraph graphToTest = new DependencyGraph();
            graphToTest.AddDependency("1", "2");
            graphToTest.AddDependency("1", "3");
            graphToTest.AddDependency("1", "4");
            graphToTest.AddDependency("3", "4");
            graphToTest.AddDependency("2", "5");
            graphToTest.AddDependency("2", "6");
            
            var executionList = graphToTest.GetExecutionList() as ICollection<String>;

            Assert.AreEqual(executionList.Count, 6);
        }
    }
}
