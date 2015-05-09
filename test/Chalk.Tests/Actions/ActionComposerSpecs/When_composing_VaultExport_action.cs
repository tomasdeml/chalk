using System;
using System.Linq;
using Chalk.Actions;
using Chalk.Logging;
using Chalk.VaultExport;
using FluentAssertions;
using NSubstitute;
using NUnit.Specification;

namespace Chalk.Tests.Actions.ActionComposerSpecs
{
    [Specification]
    public class When_composing_VaultExport_action : SpecificationBase
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
        public void Composing_VaultExport_action()
        {
            var sut = new ActionComposer();
            composedAction = sut.Compose(ActionKind.VaultExport, actionContext);
        }

        [Then]
        public void Composed_action_should_not_be_null()
        {
            composedAction.Should().NotBeNull();
        }

        [Then]
        public void Composed_action_should_contain_two_subactions()
        {
            composedAction.GetInvocationList().Should().HaveCount(2);
        }

        [Then]
        public void Composed_action_should_prepare_workspace_in_first_subaction()
        {
            composedAction.GetInvocationList()
                .First().Target
                .Should().BeOfType<PrepareWorkspaceAction>();
        }

        [Then]
        public void Composed_action_should_export_versions_from_Vault_in_last_subaction()
        {
            composedAction.GetInvocationList()
                .Last().Target
                .Should().BeOfType<VaultExportAction>();
        }
    }
}