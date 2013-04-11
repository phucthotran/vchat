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
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:vChat.Templates"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:vChat.Templates;assembly=vChat.Templates"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:BaseUserControl/>
    ///
    /// </summary>
    public class vChatController : UserControl
    {
        static vChatController()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(vChatController), new FrameworkPropertyMetadata(typeof(vChatController)));
        }

        private string _Prefix = "Controller";
        private string _ControllerPath = "vChat.Controllers";
        private dynamic _Controller = null;

        public dynamic Controller
        {
            get { return _Controller; }
        }

        public vChatController()
        {
            Type filename = Type.GetType(_ControllerPath + "." + this.GetType().Name + _Prefix);
            if (filename != null)
            _Controller = Activator.CreateInstance(filename);
        }
    }
}
