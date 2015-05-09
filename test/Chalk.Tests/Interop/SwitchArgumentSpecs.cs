using System.Collections.Generic;
using Chalk.Interop;
using FluentAssertions;
using NUnit.Framework;

namespace Chalk.Tests.Interop
{
    [TestFixture]
    public class SwitchArgumentSpecs
    {
        [Test]
        public void
            When_formatting_command_line_with_Separated_style_Then_it_should_return_Name_string()
        {
            var sut = new SwitchArgument("SwitchName");
            
            IEnumerable<string> commandLine = sut.FormatCommandLine("---", CommandLineArgumentStyle.Separated);

            commandLine.Should().Equal("---SwitchName");
        }

        [Test]
        public void
            When_formatting_command_line_with_Single_style_Then_it_should_return_single_string_Name_string()
        {
            var sut = new SwitchArgument("SwitchName");
            
            IEnumerable<string> commandLine = sut.FormatCommandLine("---", CommandLineArgumentStyle.SingleWithEqualsSignDelimiter);

            commandLine.Should().Equal("---SwitchName");
        }
    }
}