using System;
using System.Windows;
using System.Windows.Input;

namespace CryptoTracker.Commands
{
    public class SelectThameCommand : ICommand
    {
        private ResourceDictionary _currentTheme = new ResourceDictionary() 
        {
            Source = new Uri("Styles/LightTheme.xaml", UriKind.RelativeOrAbsolute)
        };
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (parameter is bool isChecked)
            {
                RemoveTheme(_currentTheme);
                ResourceDictionary newTheme;
                if (isChecked == true)
                {
                    newTheme = new ResourceDictionary
                    {
                        Source = new Uri("Styles/DarkTheme.xaml", UriKind.Relative)
                    };
                }
                else
                {
                    newTheme = new ResourceDictionary
                    {
                        Source = new Uri("Styles/LightTheme.xaml", UriKind.Relative)
                    };
                }
                AddTheme(newTheme);
                _currentTheme = newTheme;
            }
        }
        public void RemoveTheme(ResourceDictionary theme)
        {
            Application.Current.Resources.MergedDictionaries.Remove(theme);
        }
        public void AddTheme(ResourceDictionary theme)
        {
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}
