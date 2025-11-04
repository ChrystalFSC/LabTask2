namespace LabTask2;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();          // ✅ keep this
    }

    // ❌ Do NOT override CreateWindow here
    // protected override Window CreateWindow(IActivationState activationState) { ... }
}
