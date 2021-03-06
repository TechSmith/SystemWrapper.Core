using System.Diagnostics;
using SystemInterface.Diagnostics;
using SystemWrapper.Diagnostics;
using NUnit.Framework;
using Rhino.Mocks;

namespace SystemWrapper.Tests.Diagnostics
{
    [TestFixture]
    [Author("Brad Irby", "Brad@BradIrby.com")]
    public class ProcessWrapTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor_Sets_ProcessInstance()
        {
            var instance = new ProcessWrap();
            Assert.IsNotNull(instance.ProcessInstance);
        }

        [Test]
        public void Initializer_Sets_ProcessInstance()
        {
            var instance = new ProcessWrap();
            var origInfo = instance.ProcessInstance;
            instance.Initialize();
            Assert.AreNotSame(origInfo, instance.ProcessInstance);
            Assert.IsNotNull(instance.ProcessInstance);
        }

        [Test]
        public void StartInfo_Set_AssignsStartInfoWrap()
        {
            // Arrange
            var mockProcessStartInfoWrap = MockRepository.GenerateMock<IProcessStartInfo>();
            var processStartInfo = new ProcessStartInfo(); // Can't mock, so going to use the actual thing.
            mockProcessStartInfoWrap.Stub(x => x.ProcessStartInfoInstance).Return(processStartInfo);
            var instance = new ProcessWrap();
            instance.Initialize();

            // Act
            instance.StartInfo = mockProcessStartInfoWrap;

            // Assert
            Assert.AreEqual(mockProcessStartInfoWrap, instance.StartInfo);
            Assert.AreEqual(processStartInfo, instance.StartInfo.ProcessStartInfoInstance);
            Assert.AreEqual(processStartInfo, instance.ProcessInstance.StartInfo);
        }
        
        [Test]
        public void Kill_throws_expected_exception_if_process_not_started()
        {
            // Arrange
            var instance = new ProcessWrap();

            // Act
            var ex = Assert.Throws<System.InvalidOperationException>(()=> instance.Kill());
            Assert.That(ex.Message, Is.EqualTo("No process is associated with this object."));
        }
    }
}