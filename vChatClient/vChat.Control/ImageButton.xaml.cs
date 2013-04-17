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

namespace vChat.Control
{
    /// <summary>
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    public partial class ImageButton : UserControl
    {
        public event RoutedEventHandler Click;
        public static DependencyProperty CommandDependencyProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButton), new PropertyMetadata(null));
        //public static DependencyProperty ImageDependencyProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));
        //public static DependencyProperty TextDependencyProperty = DependencyProperty.Register("Text", typeof(String), typeof(ImageButton), new UIPropertyMetadata(""));

        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandDependencyProperty); }
            set { base.SetValue(CommandDependencyProperty, value); }
        }

        public ImageSource Image
        {
            //get { return (ImageSource)GetValue(ImageDependencyProperty); }
            //set { SetValue(ImageDependencyProperty, value); }
            get { return img.Source; }
            set { img.Source = value; }
        }

        public String Text
        {
            //get { return (String)GetValue(TextDependencyProperty); }
            //set { SetValue(TextDependencyProperty, value); }
            get { return tbl.Text; }
            set { tbl.Text = value; }
        }        
        
        public ImageButton()
        {            
            InitializeComponent();            
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }
    }
}
