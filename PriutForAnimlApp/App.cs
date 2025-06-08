
using PriutForAnimlApp.Views.Windows;
using System.Windows;

namespace PriutForAnimlApp
{
    public class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize main window
            var loginWindow = new LoginWindow();
            var loginVM = new LoginViewModel();
            loginVM.CloseAction = result =>
            {
                if (result == true)
                {
                    if (loginVM.IsAdmin)
                    {
                        var adminWindow = new AdminMainWindow();
                        adminWindow.DataContext = new AdminMainViewModel { CloseAction = () => adminWindow.Close() };
                        adminWindow.Show();
                    }
                    else
                    {
                        var employeeWindow = new EmployeeMainWindow();
                        employeeWindow.DataContext = new EmployeeMainViewModel { CloseAction = () => employeeWindow.Close() };
                        employeeWindow.Show();
                    }
                }
                loginWindow.Close();
            };

            loginWindow.DataContext = loginVM;
            loginWindow.Show();
        }
    }
}


 
        