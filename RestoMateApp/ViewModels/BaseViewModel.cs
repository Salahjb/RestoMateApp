using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RestoMateApp.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
        
    private string _title;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
        
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
                
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
        
    public event PropertyChangedEventHandler PropertyChanged;
        
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}