using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.UI.Xaml.Controls;
using System;
using WinUI.Service;
using WinUI.View;

namespace WinUi_Test.Tests
{
    public class FakeFrame : Frame
    {
        public Type NavigatedToType { get; private set; }
        public object PassedParameter { get; private set; }

        public new bool Navigate(Type sourcePageType, object parameter)
        {
            NavigatedToType = sourcePageType;
            PassedParameter = parameter;
            return true;
        }
    }

    [TestClass]
    public class NavigationServiceTests
    {
        [TestMethod]
        public void navigateToLogin_shouldNavigateToLogInView()
        {
            // Arrange
            var fakeFrame = new FakeFrame();
            NavigationService.sMainFrame = fakeFrame;

            // Act
            NavigationService.navigateToLogin();

            // Assert
            Assert.AreEqual(typeof(LogInView), fakeFrame.NavigatedToType);
            Assert.IsNull(fakeFrame.PassedParameter);
        }
    }
}
