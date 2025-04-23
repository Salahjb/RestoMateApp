using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RestoMate.Data;

namespace RestoMateApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}