using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace ButtonPhoneEmulator
{
    public class KeyboardViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<Button, string> _keypad = new()
        {
            {Button.One, ".,?!:"},
            {Button.Two, "abc"},
            {Button.Three, "def"},
            {Button.Four, "ghi"},
            {Button.Five, "jkl"},
            {Button.Six, "mno"},
            {Button.Seven, "pqrs"},
            {Button.Eight, "tuv"},
            {Button.Nine, "wxyz"},
            {Button.Zero, " 0"},
            {Button.Star, "*"},
            {Button.Lattice, "#"},
        };

        private Timer? _timer;
        private int _countOfPress = 0;
        private Button? _lastPressedButton = null;
        private string _title = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public KeyboardButtonCommand KeyboardButtonCommand { get; set; }
        public ReadOnlyDictionary<Button, string> Keypad { get; set; }

        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        public KeyboardViewModel()
        {
            Keypad = new ReadOnlyDictionary<Button, string>(_keypad);
            KeyboardButtonCommand = new KeyboardButtonCommand(Execute, CanExecute);
        }

        private void EraseState()
        {
            _timer?.Dispose();
            _timer = null;
            _countOfPress = 0;
            _lastPressedButton = null;
        }

        private void Callback(object? state)
        {
            EraseState();
        }

        private static bool CanExecute(object? arg)
        {
            //Если нет текста на экране и нажата клавиша стирания текста
            //if (arg is Button btn)
            //    return btn != Button.Star || !string.IsNullOrEmpty(Title);
            return arg is Button;
        }

        private void Execute(object? obj)
        {
            if (obj is not Button btn)
            {
                throw new NotSupportedException("Not supported type of parameter");
            }

            if (btn == Button.Lattice)
            {
                EraseState();
            }
            else
            {
                if (_lastPressedButton != btn)
                {
                    _countOfPress = 0;
                    _lastPressedButton = btn;
                }

                var newString = Title;
                if (_countOfPress != 0)
                {
                    if (!string.IsNullOrEmpty(Title))
                    {
                        newString = newString[..^1];
                    }
                }

                newString += _keypad[btn][_countOfPress++];
                if (_countOfPress == _keypad[btn].Length)
                {
                    _countOfPress = 0;
                }

                Title = newString;
                _timer?.Dispose();
                _timer = new Timer(Callback, null, 1000, 0);
            }

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
