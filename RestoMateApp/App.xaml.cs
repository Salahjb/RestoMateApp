using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RestoMateApp.Data;

namespace RestoMateApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}