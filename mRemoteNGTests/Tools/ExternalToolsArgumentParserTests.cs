using System;
using System.Collections;
using mRemoteNG.Connection;
using mRemoteNG.Tools;
using NUnit.Framework;


namespace mRemoteNGTests.Tools
{
    public class ExternalToolsArgumentParserTests
    {
        private ExternalToolArgumentParser _argumentParser;
        private const string TEST_STRING = @"()%!^abc123*<>&|""'\";
        private const string STRING_AFTER_METACHARACTER_ESCAPING = @"^(^)^%^!^^abc123*^<^>^&^|^""'\";
        private const string STRING_AFTER_ALL_ESCAPING = @"^(^)^%^!^^abc123*^<^>^&^|\^""'\";
        private const string STRING_AFTER_NO_ESCAPING = TEST_STRING;
        private const int PORT = 9933;
        private const string PORT_AS_STRING = "9933";
        private const string SAMPLE_COMMAND_STRING = @"/k echo ()%!^abc123*<>&|""'\";


        [OneTimeSetUp]
        public void Setup()
        {
            var connectionInfo = new ConnectionInfo
            {
                Name = TEST_STRING,
                Hostname = TEST_STRING,
                Port = PORT,
                Username = TEST_STRING,
                Password = TEST_STRING,
                Domain = TEST_STRING,
                Description = TEST_STRING,
                MacAddress = TEST_STRING,
                UserField = TEST_STRING
            };
            _argumentParser = new ExternalToolArgumentParser(connectionInfo);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _argumentParser = null;
        }

        [TestCaseSource(typeof(ParserTestsDataSource), nameof(ParserTestsDataSource.TestCases))]
        public string ParserTests(string argumentString)
        {
            return _argumentParser.ParseArguments(argumentString);
        }

        [Test]
        public void NullConnectionInfoResultsInEmptyVariables()
        {
            var parser = new ExternalToolArgumentParser(null);
            var parsedText = parser.ParseArguments("test %USERNAME% test");
            Assert.That(parsedText, Is.EqualTo("test  test"));
        }



        private class ParserTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData("%NAME%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-NAME%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!NAME%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%HOSTNAME%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-HOSTNAME%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!HOSTNAME%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%PORT%").Returns(PORT_AS_STRING);
                    yield return new TestCaseData("%-PORT%").Returns(PORT_AS_STRING);
                    yield return new TestCaseData("%!PORT%").Returns(PORT_AS_STRING);
                    yield return new TestCaseData("%USERNAME%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-USERNAME%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!USERNAME%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%PASSWORD%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-PASSWORD%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!PASSWORD%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%DOMAIN%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-DOMAIN%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!DOMAIN%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%DESCRIPTION%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-DESCRIPTION%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!DESCRIPTION%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%MACADDRESS%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-MACADDRESS%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!MACADDRESS%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%USERFIELD%").Returns(STRING_AFTER_ALL_ESCAPING);
                    yield return new TestCaseData("%-USERFIELD%").Returns(STRING_AFTER_METACHARACTER_ESCAPING);
                    yield return new TestCaseData("%!USERFIELD%").Returns(STRING_AFTER_NO_ESCAPING);
                    yield return new TestCaseData("%%") {TestName = "EmptyVariableTagsNotParsed" }.Returns("%%");
                    yield return new TestCaseData("/k echo %!USERNAME%") { TestName = "ParsingWorksWhenVariableIsNotInFirstPosition" }.Returns(SAMPLE_COMMAND_STRING);
                    yield return new TestCaseData("%COMSPEC%") { TestName = "EnvironmentVariablesParsed" }.Returns(Environment.GetEnvironmentVariable("comspec"));
                    yield return new TestCaseData("%UNSUPPORTEDPARAMETER%") { TestName = "UnsupportedParametersNotParsed" }.Returns("%UNSUPPORTEDPARAMETER%");
                    yield return new TestCaseData(@"\%COMSPEC\%") { TestName = "BackslashEscapedEnvironmentVariablesParsed" }.Returns(Environment.GetEnvironmentVariable("comspec"));
                    yield return new TestCaseData(@"^%COMSPEC^%") { TestName = "ChevronEscapedEnvironmentVariablesNotParsed" }.Returns("%COMSPEC%");
                }
            }
    }
    }
}