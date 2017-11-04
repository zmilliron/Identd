using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identd.Tests
{
    /// <summary>
    /// Summary description for IdentServerTests
    /// </summary>
    [TestClass]
    public class IdentServerTests
    {
        private const string _userName = "testname";

        [TestMethod]
        public void IsIDisposable()
        {
            IdentServer server = new IdentServer(_userName);
            Assert.IsInstanceOfType(server, typeof(IDisposable));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldNotAcceptNullName()
        {
            IdentServer server = new IdentServer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorShouldNowAcceptEmptyName()
        {
            IdentServer server = new IdentServer(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorShouldNotAcceptWhitespaceName()
        {
            IdentServer server = new IdentServer("           ");
        }

        [TestMethod]
        public void TimeoutShouldInitializeAsDefault()
        {
            IdentServer server = new IdentServer(_userName);

            Assert.AreEqual(IdentServer.DEFAULTTIMEOUT, server.Timeout);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TimeoutShouldNotAcceptNegativeNumber()
        {
            IdentServer server = new IdentServer(_userName);
            server.Timeout = -500;
        }

        [TestMethod]
        public void DisposeShouldAllowMultipleCalls()
        {
            IdentServer server = new IdentServer(_userName);
            server.Dispose();
            server.Dispose();
            server.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void StartShouldThrowIfDisposed()
        {
            IdentServer server = new IdentServer(_userName);
            server.Dispose();
            server.Start();
        }

        [TestMethod]
        public void IsRunningShouldInitializeFalse()
        {
            IdentServer server = new IdentServer(_userName);

            Assert.AreEqual(false, server.IsRunning);
        }
    }
}
