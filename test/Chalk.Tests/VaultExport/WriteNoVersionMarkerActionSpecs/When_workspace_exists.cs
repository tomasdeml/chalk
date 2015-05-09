using System.IO.Abstractions;
using Chalk.Actions;
using Chalk.Logging;
using Chalk.VaultExport;
using NSubstitute;
using NUnit.Specification;
using Ploeh.AutoFixture;

namespace Chalk.Tests.VaultExport.WriteNoVersionMarkerActionSpecs
{
    [Specification]
    public class When_workspace_exists : SpecificationBase
    {
        IFixture fixture;
        ActionContext actionContext;
        IFileSystem fileSystemSubstitute;

        public override void Setup()
        {
            base.Setup();
            fileSystemSubstitute = Substitute.For<IFileSystem>();
            fixture = new Fixture();
        }

        [Given]
        public void Action_context_with_path_to_workspace()
        {
            var parameters = fixture.Create<Parameters>();
            actionContext = new ActionContext(parameters, Substitute.For<ILogger>());
        }

        [And]
        public void Already_existing_directory_in_workspace_path()
        {
            fileSystemSubstitute.Directory.Exists(actionContext.Parameters.LocalWorkspacePath).Returns(true);
        }

        [When]
        public void Executed()
        {
            var sut = new PrepareWorkspaceAction(actionContext, fileSystemSubstitute); 
            sut.Execute();
        }

        [Then]
        public void It_should_not_create_workspace_directory_again()
        {
            fileSystemSubstitute.Directory.DidNotReceive().CreateDirectory(Arg.Any<string>());
        }
    }
}