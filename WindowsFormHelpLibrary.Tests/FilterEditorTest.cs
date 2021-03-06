// <copyright file="FilterEditorTest.cs">Copyright ©  2019</copyright>
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormHelpLibrary.FilterHelp;
using WindowsFormHelpLibrary.SortableBindingList;

namespace WindowsFormHelpLibrary.Tests
{
    /// <summary>This class contains parameterized unit tests for FilterEditor</summary>
    [PexClass(typeof(FilterEditor))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class FilterEditorTest
    {
        /// <summary>Test stub for .ctor(PropertiesFilter)</summary>
        [STAThread]
        [TestMethod]
        public void ConstructorTest()
        {

        }
    }
}
