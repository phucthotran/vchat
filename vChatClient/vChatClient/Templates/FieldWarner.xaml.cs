using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vChat.Templates
{
    /// <summary>
    /// Interaction logic for FieldWarner.xaml
    /// </summary>
    public partial class FieldWarner : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FieldWarner), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                WarningText.Text = value;
                if (String.IsNullOrWhiteSpace(value))
                    Warning = false;
                else
                    Warning = true;
            }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(FieldWarner), new PropertyMetadata(Brushes.DarkRed));
        public SolidColorBrush Color
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set
            {
                SetValue(TextProperty, value);
                WarningText.Foreground = value;
            }
        }

        public static readonly DependencyProperty WarningProperty = DependencyProperty.Register("Warning", typeof(bool), typeof(FieldWarner), new PropertyMetadata(true));
        public bool Warning
        {
            get { return (bool)GetValue(WarningProperty); }
            set
            {
                SetValue(WarningProperty, value);
                Busy = false;
            }
        }

        public static readonly DependencyProperty BusyProperty = DependencyProperty.Register("Busy", typeof(bool), typeof(FieldWarner), new PropertyMetadata(true));
        public bool Busy
        {
            get { return (bool)GetValue(BusyProperty); }
            set
            {
                SetValue(BusyProperty, value);
                if (value)
                {
                    ProgressBar.Visibility = System.Windows.Visibility.Visible;
                    ProgressBar.State = Elysium.Controls.ProgressState.Busy;
                    WarningText.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    ProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                    ProgressBar.State = Elysium.Controls.ProgressState.Normal;
                    if (Warning)
                    {
                        WarningText.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        WarningText.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
            }
        }

        public static readonly DependencyProperty MarginBottomProperty = 
            DependencyProperty.Register("MarginBottom", typeof(double), typeof(FieldWarner), new PropertyMetadata((double)0, delegate (DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
                FieldWarner thiz = sender as FieldWarner;
                thiz.ProgressBar.Margin = new Thickness(thiz.ProgressBar.Margin.Left, thiz.ProgressBar.Margin.Top, thiz.ProgressBar.Margin.Right, (double)e.NewValue);
                thiz.WarningText.Margin = new Thickness(thiz.WarningText.Margin.Left, thiz.WarningText.Margin.Top, thiz.WarningText.Margin.Right, (double)e.NewValue);
            }));
        public double MarginBottom
        {
            get { return (double)GetValue(MarginBottomProperty); }
            set
            {
                SetValue(MarginBottomProperty, value);
            }
        }

        public FieldWarner()
        {
            InitializeComponent();
            ProgressBar.Margin = new Thickness(ProgressBar.Margin.Left, ProgressBar.Margin.Top, ProgressBar.Margin.Right, MarginBottom);
            WarningText.Margin = new Thickness(WarningText.Margin.Left, WarningText.Margin.Top, WarningText.Margin.Right, MarginBottom);
        }
    }
}
