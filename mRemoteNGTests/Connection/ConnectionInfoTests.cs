﻿using System.Collections.Generic;
using System.Reflection;
using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Connection.Protocol.SSH;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.Connection
{
	public class ConnectionInfoTests
    {
        private ConnectionInfo _connectionInfo;
        private const string TEST_DOMAIN = "somedomain";

        [SetUp]
        public void Setup()
        {
            _connectionInfo = new ConnectionInfo();
        }

        [TearDown]
        public void Teardown()
        {
            _connectionInfo = null;
        }

        [Test]
        public void CopyCreatesMemberwiseCopy()
        {
            _connectionInfo.Domain = TEST_DOMAIN;
            var secondConnection = _connectionInfo.Clone();
            Assert.That(secondConnection.Domain, Is.EqualTo(_connectionInfo.Domain));
        }

        [Test]
        public void CloneDoesNotSetParentOfNewConnectionInfo()
        {
            _connectionInfo.SetParent(new ContainerInfo());
            var clonedConnection = _connectionInfo.Clone();
            Assert.That(clonedConnection.Parent, Is.Null);
        }

        [Test]
        public void CloneAlsoCopiesInheritanceObject()
        {
            var clonedConnection = _connectionInfo.Clone();
            Assert.That(clonedConnection.Inheritance, Is.Not.EqualTo(_connectionInfo.Inheritance));
        }

        [Test]
        public void CloneCorrectlySetsParentOfInheritanceObject()
        {
			var originalConnection = new ConnectionInfo();
            var clonedConnection = originalConnection.Clone();
            Assert.That(clonedConnection.Inheritance.Parent, Is.EqualTo(clonedConnection));
        }

        [Test]
        public void CopyFromCopiesProperties()
        {
            var secondConnection = new ConnectionInfo {Domain = TEST_DOMAIN};
            _connectionInfo.CopyFrom(secondConnection);
            Assert.That(_connectionInfo.Domain, Is.EqualTo(secondConnection.Domain));
        }

        [Test]
        public void CopyingAConnectionInfoAlsoCopiesItsInheritance()
        {
            _connectionInfo.Inheritance.Username = true;
            var secondConnection = new ConnectionInfo {Inheritance = {Username = false}};
            secondConnection.CopyFrom(_connectionInfo);
            Assert.That(secondConnection.Inheritance.Username, Is.True);
        }

        [Test]
        public void PropertyChangedEventRaisedWhenOpenConnectionsChanges()
        {
            var eventWasCalled = false;
            _connectionInfo.PropertyChanged += (sender, args) => eventWasCalled = true;
            _connectionInfo.OpenConnections.Add(new ProtocolSsh2());
            Assert.That(eventWasCalled);
        }

        [Test]
        public void PropertyChangedEventArgsAreCorrectWhenOpenConnectionsChanges()
        {
            var nameOfModifiedProperty = "";
            _connectionInfo.PropertyChanged += (sender, args) => nameOfModifiedProperty = args.PropertyName;
            _connectionInfo.OpenConnections.Add(new ProtocolSsh2());
            Assert.That(nameOfModifiedProperty, Is.EqualTo("OpenConnections"));
        }

	    [TestCaseSource(typeof(InheritancePropertyProvider), nameof(InheritancePropertyProvider.GetProperties))]
	    public void MovingAConnectionFromUnderRootNodeToUnderADifferentNodeEnablesInheritance(PropertyInfo property)
	    {
		    var rootNode = new RootNodeInfo(RootNodeType.Connection);
			var otherContainer = new ContainerInfo();
		    _connectionInfo.Inheritance.EverythingInherited = true;
		    _connectionInfo.SetParent(rootNode);
			_connectionInfo.SetParent(otherContainer);
		    var propertyValue = property.GetValue(_connectionInfo.Inheritance);
		    Assert.That(propertyValue, Is.True);
	    }

		[TestCase(ProtocolType.Http, ExpectedResult = 80)]
        [TestCase(ProtocolType.Https, ExpectedResult = 443)]
        [TestCase(ProtocolType.IntApp, ExpectedResult = 0)]
        [TestCase(ProtocolType.Raw, ExpectedResult = 23)]
        [TestCase(ProtocolType.Rdp, ExpectedResult = 3389)]
        [TestCase(ProtocolType.Rlogin, ExpectedResult = 513)]
        [TestCase(ProtocolType.Ssh1, ExpectedResult = 22)]
        [TestCase(ProtocolType.Ssh2, ExpectedResult = 22)]
        [TestCase(ProtocolType.Telnet, ExpectedResult = 23)]
        [TestCase(ProtocolType.Vnc, ExpectedResult = 5900)]
        public int GetDefaultPortReturnsCorrectPortForProtocol(ProtocolType protocolType)
        {
            _connectionInfo.Protocol = protocolType;
            return _connectionInfo.GetDefaultPort();
        }

	    private class InheritancePropertyProvider
	    {
		    public static IEnumerable<PropertyInfo> GetProperties()
		    {
			    return new ConnectionInfoInheritance(new ConnectionInfo()).GetProperties();
		    }
	    }
    }
}