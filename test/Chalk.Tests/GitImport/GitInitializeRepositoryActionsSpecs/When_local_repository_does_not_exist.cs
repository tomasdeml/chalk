using Chalk.Actions;
using Chalk.GitImport;
using Chalk.GitImport.Interop;
using Chalk.Logging;
using NSubstitute;
using NUnit.Specification;
using Ploeh.AutoFixture;

namespace Chalk.Tests.GitImport.GitInitializeRepositoryActionsSpecs
{
    [Specification]
    public class When_local_repository_does_not_exist : SpecificationBase
    {
        ActionContext actionContext;
        IFixture fixture;
        IGitFacade gitFacadeSubstitute;
        IRepository repositorySubstitute;

        public override void Setup()
        {
            base.Setup();
            fixture = new Fixture();
            repositorySubstitute = Substitute.For<IRepository>();
            gitFacadeSubstitute = Substitute.For<IGitFacade>();
            gitFacadeSubstitute.NewRepository(Arg.Any<string>()).Returns(repositorySubstitute);
        }

        [Given]
        public void Action_context_with_path_to_workspace()
        {
            actionContext = new ActionContext(fixture.Create<Parameters>(), Substitute.For<ILogger>());
        }

        [And]
        public void No_existing_git_repository_directory_in_workspace_path()
        {
            repositorySubstitute.Exists().Returns(false);
        }

        [When]
        public void Executed()
        {
            var sut = new GitInitializeLocalRepositoryAction(actionContext, gitFacadeSubstitute);
            sut.Execute();
        }

        [Then]
        public void It_should_initialize_local_repository_in_workspace_path()
        {
            repositorySubstitute.Received().Initialize();
        }
    }
}