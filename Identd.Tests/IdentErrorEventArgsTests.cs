using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Identd;

namespace Identd.Tests
{
    [TestClass]
    public class IdentErrorEventArgsTests
    {
        [TestMethod]
        public void IsEventArgs()
        {
            IdentErrorEventArgs e = new IdentErrorEventArgs("Test message.");
            Assert.IsInstanceOfType(e, typeof(EventArgs));
        }

        [TestMethod]
        public void ConstructorShouldAcceptNullMessage()
        {
            string message = null;
            IdentErrorEventArgs e = new IdentErrorEventArgs(message);
        }

        [TestMethod]
        public void ConstructorShouldAcceptNullException()
        {
            Exception ex = null;
            IdentErrorEventArgs e = new IdentErrorEventArgs(ex);
        }

        [TestMethod]
        public void ConstructorShouldAcceptAllNulls()
        {
            IdentErrorEventArgs e = new IdentErrorEventArgs(null, null);
        }

        [TestMethod]
        public void ConstructorShouldAcceptNonNullMessage()
        {
            IdentErrorEventArgs e = new IdentErrorEventArgs("Test message");
        }

        [TestMethod]
        public void ConstructorShouldAcceptNonNullException()
        {
            IdentErrorEventArgs e = new IdentErrorEventArgs(new InvalidOperationException());
        }

        [TestMethod]
        public void ConstructorShouldAcceptNonNullParameters()
        {
            string message = "This is a test message";
            InvalidOperationException ex = new InvalidOperationException("This is a test message.");
            IdentErrorEventArgs e = new IdentErrorEventArgs(message, ex);
        }

        [TestMethod]
        public void MessagePropertyShouldBeMessageParameter()
        {
            string message = "This is a test message";
            IdentErrorEventArgs e = new IdentErrorEventArgs(message);

            Assert.AreEqual(message, e.Message);
        }

        [TestMethod]
        public void MessagePropertyShouldBeExceptionMessage()
        {
            InvalidOperationException ex = new InvalidOperationException("This is a test message.");
            IdentErrorEventArgs e = new IdentErrorEventArgs(ex);

            Assert.AreEqual(ex.Message, e.Message);
        }

        [TestMethod]
        public void ExceptionPropertyShouldBeExceptionParemter()
        {
            InvalidOperationException ex = new InvalidOperationException("This is a test message.");
            IdentErrorEventArgs e = new IdentErrorEventArgs(ex);

            Assert.AreEqual(ex, e.Exception);
        }

        [TestMethod]
        public void MessagePropertyShouldBeMessageParameterWithException()
        {
            string message = "This should be the message property.";
            InvalidOperationException ex = new InvalidOperationException("This is a test message.");
            IdentErrorEventArgs e = new IdentErrorEventArgs(message, ex);

            Assert.AreEqual(message, e.Message);
        }
    }
}
