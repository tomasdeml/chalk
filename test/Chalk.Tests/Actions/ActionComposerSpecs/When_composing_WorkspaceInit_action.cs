using System;
using System.Linq;
using Chalk.Actions;
using Chalk.GitImport;
using Chalk.Logging;
using Chalk.VaultExport;
using FluentAssertions;
using NSubstitute;
using NUnit.Specification;

namespace Chalk.Tests.Actions.ActionComposerSpecs
{
    [Specification]
    public class When_composing_WorkspaceInit_action : SpecificationBase
    {
        ActionContext actionContext;
        Action composedAction; 

        [Given]
        public void Action_context_with_parameters()
        {
            actionContext = new ActionContext(new Parameters
            {
                LocalWorkspacePath = @"C:\Workspace" 
            }, Substitute.For<ILogger>());
        }

        [When]
        public void Composing_WorkspaceInit_action()
        {
            var sut = new ActionComposer();
            composedAction = sut.Compose(ActionKind.WorkspaceInit, actionContext);
        }

        [Then]
        public void Composed_action_should_not_be_null()
        {
            composedAction.Should().NotBeNull();
        }

        [Then]
        public void Composed_action_should_contain_three_subactions()
        {
            composedAction.GetInvocationList().Should().HaveCount(3);
        }

        [Then]
        public void Composed_action_should_prepare_workspace_in_first_subaction()
        {
            composedAction.GetInvocationList()
                .First().Target
                .Should().BeOfType<PrepareWorkspaceAction>();
        }

        [Then]
        public void Composed_action_should_initialize_Git_repository_in_second_subaction()
        {
            composedAction.GetInvocationList()
                .ElementAt(1).Target
                .Should().BeOfType<GitInitializeLocalRepositoryAction>();
        }

        [Then]
        public void Composed_action_should_create_empty_version_marker_in_last_subaction()
        {
            composedAction.GetInvocationList()
                .Last().Target
                .Should().BeOfType<WriteNoVersionMarkerAction>();
        }
    }
}
