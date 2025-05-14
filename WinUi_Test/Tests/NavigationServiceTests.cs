using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.UI.Xaml.Controls;
using WinUI.Service;
using WinUI.View;

/*Eroarea System.Runtime.InteropServices.COMException: 
 * Class not registered (0x80040154 REGDB_E_CLASSNOTREG) 
 * din testele tale de navigare apare din cauza faptului că
 * Microsoft.UI.Xaml.Controls.Frame este o clasă dependentă de 
 * framework-ul WinUI 3, care nu poate fi instanțiat în timpul testelor 
 * unitare deoarece nu există un context vizual/XAML disponibil în runtime.

✅ Soluția corectă este:
1. Extrage o interfață pentru INavigationFrame,
2. Injectează această interfață în NavigationService,
3. Creează un FakeNavigationFrame în test pentru a verifica ce pagină a fost navigată.



public interface INavigationFrame
{
    bool Navigate(Type sourcePageType, object parameter = null);
}




using Microsoft.UI.Xaml.Controls;

public class RealNavigationFrame : INavigationFrame
{
    private readonly Frame _frame;

    public RealNavigationFrame(Frame frame)
    {
        _frame = frame;
    }

    public bool Navigate(Type sourcePageType, object parameter = null)
    {
        return _frame.Navigate(sourcePageType, parameter);
    }
}


*/

namespace WinUi_Test.Tests
{
    [TestClass]
    public class NavigationServiceTests
    {
        private class FakeFrame : Frame
        {
            public Type LastNavigatedPageType { get; private set; }
            public object LastParameter { get; private set; }

            public new bool Navigate(Type sourcePageType, object parameter)
            {
                LastNavigatedPageType = sourcePageType;
                LastParameter = parameter;
                return true;
            }
        }

        [TestMethod]
        public void navigateToLogin_navigatesToLogInView()
        {
            // Arrange
            var fakeFrame = new FakeFrame();
            NavigationService.sMainFrame = fakeFrame;

            // Act
            NavigationService.navigateToLogin();

            // Assert
            Assert.AreEqual(typeof(LogInView), fakeFrame.LastNavigatedPageType);
            Assert.IsNull(fakeFrame.LastParameter);
        }

        [TestMethod]
        public void navigate_withParameter_navigatesToExpectedPageWithParameter()
        {
            // Arrange
            var fakeFrame = new FakeFrame();
            var testParam = "TestParam";
            NavigationService.sMainFrame = fakeFrame;

            // Act
            NavigationService.navigate(typeof(LogInView), testParam);

            // Assert
            Assert.AreEqual(typeof(LogInView), fakeFrame.LastNavigatedPageType);
            Assert.AreEqual(testParam, fakeFrame.LastParameter);
        }
    }
}
