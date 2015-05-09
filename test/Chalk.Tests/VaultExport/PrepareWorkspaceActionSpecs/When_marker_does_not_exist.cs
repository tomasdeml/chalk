using Chalk.VaultExport;
using NSubstitute;
using NUnit.Specification;

namespace Chalk.Tests.VaultExport.PrepareWorkspaceActionSpecs
{
    [Specification]
    public class When_marker_does_not_exist : SpecificationBase
    {
        ILastVersionMarker lastVersionMarkerSubstitute;

        public override void Setup()
        {
            base.Setup();
            lastVersionMarkerSubstitute = Substitute.For<ILastVersionMarker>();
        }

        [Given]
        public void No_last_version_marker_exists()
        {
            lastVersionMarkerSubstitute.Exists().Returns(false);
        }

        [When]
        public void Executed()
        {
            var sut = new WriteNoVersionMarkerAction(lastVersionMarkerSubstitute);
            sut.Execute();
        }

        [Then]
        public void It_should_create_last_version_marker_marking_no_last_version()
        {
           lastVersionMarkerSubstitute.Received().MarkNone(); 
        }
    }
}
