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

        #region DEPENDENCY PROPERTY

        public static DependencyProperty CommandDependencyProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButton), new PropertyMetadata(null));
        public static DependencyProperty ImageDependencyProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));
        public static DependencyProperty TextDependencyProperty = DependencyProperty.Register("Text", typeof(String), typeof(ImageButton), new UIPropertyMetadata(""));

        #endregion

        #region PROPERTY

        /// <summary>
        /// Lấy/gán command cho button
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandDependencyProperty); }
            set { base.SetValue(CommandDependencyProperty, value); }
        }

        /// <summary>
        /// Lấy/gán ảnh cho button
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageDependencyProperty); }
            set { SetValue(ImageDependencyProperty, value); }
        }

        /// <summary>
        /// Lấy/gán chữ cho button
        /// </summary>
        public String Text
        {
            get { return (String)GetValue(TextDependencyProperty); }
            set { SetValue(TextDependencyProperty, value); }
        }

        #endregion

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
