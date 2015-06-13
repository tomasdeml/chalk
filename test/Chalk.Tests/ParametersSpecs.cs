using System;
using Chalk.Actions;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Chalk.Tests
{
    [TestFixture]
    public class ParametersSpecs
    {
        readonly IFixture fixture = new Fixture();

        [Test]
        public void Given_parameters_for_WorkspaceInit_action_without_LocalWorkspacePath_set_Then_it_should_raise_error()
        {
            var sut = CreateValidParametersForWorkspaceInitAction();
            sut.LocalWorkspacePath = null;

            Action validationAction = () => sut.Validate();
            validationAction.ShouldThrow<ValidationException>()
                .Which.Errors.Should()
                .Contain(e => e.PropertyName == "LocalWorkspacePath"); 
        }

        [Test]
        public void Given_parameters_for_WorkspaceInit_action_without_CommitAuthorEmailDomain_set_Then_it_should_raise_error()
        {
            var sut = CreateValidParametersForWorkspaceInitAction();
            sut.CommitAuthorEmailDomain = null;

            Action validationAction = () => sut.Validate();
            validationAction.ShouldThrow<ValidationException>()
                .Which.Errors.Should()
                .Contain(e => e.PropertyName == "CommitAuthorEmailDomain"); 
        }

        Parameters CreateValidParametersForWorkspaceInitAction()
        {
            return new Parameters
            {
                Action = ActionKind.WorkspaceInit,
                LocalWorkspacePath = fixture.Create("Path"),
                CommitAuthorEmailDomain = fixture.Create("Domain")
            };
        }
    }
}
