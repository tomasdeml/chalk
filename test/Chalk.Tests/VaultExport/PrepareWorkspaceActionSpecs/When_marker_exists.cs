using Chalk.VaultExport;
using NSubstitute;
using NUnit.Specification;

namespace Chalk.Tests.VaultExport.PrepareWorkspaceActionSpecs
{
    [Specification]
    public class When_marker_exists : SpecificationBase
    {
        ILastVersionMarker lastVersionMarkerSubstitute;

        public override void Setup()
        {
            base.Setup();
            lastVersionMarkerSubstitute = Substitute.For<ILastVersionMarker>();
        }

        [Given]
        public void Last_version_marker_already_exists()
        {
            lastVersionMarkerSubstitute.Exists().Returns(true);
        }

        [When]
        public void Executed()
        {
            var sut = new WriteNoVersionMarkerAction(lastVersionMarkerSubstitute);
            sut.Execute();
        }

        [Then]
        public void It_should_not_overwrite_existing_last_version_marker()
        {
            lastVersionMarkerSubstitute.DidNotReceive().MarkNone();
            lastVersionMarkerSubstitute.DidNotReceive().Mark(Arg.Any<int>());
        }
    }
}