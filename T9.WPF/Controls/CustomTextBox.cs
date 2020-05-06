using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace T9.WPF.Controls
{
    class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            SelectionChanged += HandleSelectionChanged;
        }

        public int BindableSelectionStart
        {
            get
            {
                return (int)this.GetValue(BindableSelectionStartProperty);
            }
            set
            {
                this.SetValue(BindableSelectionStartProperty, value);
            }
        }
        public string BindableSelectedText
        {
            get { return (string)GetValue(BindableSelectedTextProperty); }
            set { SetValue(BindableSelectedTextProperty, value); }
        }

        public static readonly DependencyProperty BindableSelectedTextProperty;
        public static readonly DependencyProperty BindableSelectionStartProperty;
        static CustomTextBox()
        {
            BindableSelectionStartProperty =
                DependencyProperty.Register("BindableSelectionStart", typeof(int), typeof(CustomTextBox), new PropertyMetadata(OnBindableSelectionStartChanged));

            BindableSelectedTextProperty =
                DependencyProperty.Register("BindableSelectedText", typeof(string), typeof(CustomTextBox), new PropertyMetadata(OnBindableSelectedTextChanged));
        }

        static void OnBindableSelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs a)
        {
            var textBox = (CustomTextBox)d;

            int newValue = (int)a.NewValue;
            textBox.SelectionStart = newValue;
        }
        static void OnBindableSelectedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs a)
        {
            var textBox = (CustomTextBox)d;

            string newValue = (string)a.NewValue;
            textBox.SelectedText = newValue;
        }

        void HandleSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (BindableSelectionStart != SelectionStart)
            {
                BindableSelectionStart = SelectionStart;
            }

            if (BindableSelectedText != SelectedText)
            {
                BindableSelectedText = SelectedText;
            }
        }
    }
}
