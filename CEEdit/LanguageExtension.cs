using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace CEEdit
{
    [MarkupExtensionReturnType(typeof(BindingExpression))]
    public class LangExtension : MarkupExtension
    {
        public string Key { get; set; }
        public string? DefaultValue { get; set; }

        public LangExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
            {
                Source = new LanguageBinding(Key, DefaultValue)
            };
            
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target)
            {
                return binding.ProvideValue(serviceProvider);
            }
            
            return binding;
        }
    }

    public class LanguageBinding : INotifyPropertyChanged
    {
        private readonly string _key;
        private readonly string? _defaultValue;

        public LanguageBinding(string key, string? defaultValue = null)
        {
            _key = key;
            _defaultValue = defaultValue;
            LanguageManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        public string Value => LanguageManager.Instance.GetString(_key, _defaultValue ?? string.Empty);

        private void OnLanguageChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}