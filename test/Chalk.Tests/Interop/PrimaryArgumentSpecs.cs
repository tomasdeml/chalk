using System.Collections.Generic;
using Chalk.Interop;
using FluentAssertions;
using NUnit.Framework;

namespace Chalk.Tests.Interop
{
    [TestFixture]
    public class PrimaryArgumentSpecs
    {
        [Test]
        public void
            When_formatting_command_line_with_Separated_style_Then_it_should_return_Value_string_without_prefix()
        {
            var sut = new PositionalArgument("Value");
            
            IEnumerable<string> commandLine = sut.FormatCommandLine("---", CommandLineArgumentStyle.Separated);

            commandLine.Should().Equal("Value");
        }

        [Test]
        public void
            When_formatting_command_line_with_Single_style_Then_it_should_return_Value_string_without_prefix()
        {
            var sut = new PositionalArgument("Value");
            
            IEnumerable<string> commandLine = sut.FormatCommandLine("---", CommandLineArgumentStyle.SingleWithEqualsSignDelimiter);

            commandLine.Should().Equal("Value");
        }
    }
}