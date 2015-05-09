using System.Collections.Generic;
using Chalk.Interop;
using FluentAssertions;
using NUnit.Framework;

namespace Chalk.Tests.Interop
{
    [TestFixture]
    public class NamedArgumentSpecs
    {
        [Test]
        public void
            When_formatting_command_line_with_Separated_style_Then_it_should_return_Name_and_Value_strings()
        {
            var sut = new NamedArgument("ArgumentName", "ArgumentValue");
            
            IEnumerable<string> commandLine = sut.FormatCommandLine("---", CommandLineArgumentStyle.Separated);

            commandLine.Should().Equal("---ArgumentName", "ArgumentValue");
        }

        [Test]
        public void
            When_formatting_command_line_with_Single_style_Then_it_should_return_single_string_with_Name_and_Value_delimited_by_equals_sign()
        {
            var sut = new NamedArgument("ArgumentName", "ArgumentValue");
            
            IEnumerable<string> commandLine = sut.FormatCommandLine("---", CommandLineArgumentStyle.SingleWithEqualsSignDelimiter);

            commandLine.Should().Equal("---ArgumentName=ArgumentValue");
        }
    }
}
