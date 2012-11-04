﻿namespace PythonSharp.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PythonSharp.Commands;
    using PythonSharp.Compiler;

    [TestClass]
    public class ExecuteTests
    {
        private Machine machine;

        [TestInitialize]
        public void Setup()
        {
            this.machine = new Machine();
            this.machine.Output = new StringWriter();
        }

        [TestMethod]
        public void ExecuteSimplePrint()
        {
            Assert.AreEqual("1\r\n", this.ExecuteAndPrint("print(1)"));
        }

        [TestMethod]
        public void ExecuteTwoSimplePrints()
        {
            Assert.AreEqual("1\r\n2\r\n", this.ExecuteAndPrint("print(1);print(2)"));
        }

        [TestMethod]
        [DeploymentItem("Examples/printvars.py")]
        public void ExecutePrintVars()
        {
            Assert.AreEqual("1\r\n2\r\n", this.ExecuteFileAndPrint("printvars.py"));
        }

        [TestMethod]
        [DeploymentItem("Examples/return.py")]
        public void ExecuteReturnFile()
        {
            Assert.AreEqual("1\r\n", this.ExecuteFileAndPrint("return.py"));
        }

        [TestMethod]
        [DeploymentItem("Examples/factorial.py")]
        public void ExecuteFactorialFile()
        {
            Assert.AreEqual("1\r\n2\r\n6\r\n24\r\n", this.ExecuteFileAndPrint("factorial.py"));
        }

        [TestMethod]
        [DeploymentItem("Examples/defargs.py")]
        public void ExecuteDefArgsFile()
        {
            Assert.AreEqual("3\r\n4\r\n5\r\n", this.ExecuteFileAndPrint("defargs.py"));
        }

        private string ExecuteAndPrint(string text)
        {
            this.Execute(text);
            return this.machine.Output.ToString();
        }

        private string ExecuteFileAndPrint(string filename)
        {
            this.ExecuteFile(filename);
            return this.machine.Output.ToString();
        }

        private void Execute(string text)
        {
            Parser parser = new Parser(text);
            ICommand command = parser.CompileCommandList();
            command.Execute(this.machine.Environment);
        }

        private void ExecuteFile(string filename)
        {
            Parser parser = new Parser(new StreamReader(filename));
            ICommand command = parser.CompileCommandList();
            command.Execute(this.machine.Environment);
        }
    }
}