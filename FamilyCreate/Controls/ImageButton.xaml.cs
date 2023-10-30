using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FamilyCreate.Controls
{
    public partial class ImageButton : UserControl
    {
        public ImageButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty buttonTextProperty =
DependencyProperty.Register("ButtonText", typeof(string),
typeof(ImageButton), new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty imageSourceProperty =
DependencyProperty.Register("ImageSource", typeof(ImageSource),
typeof(ImageButton), null);

        public static readonly DependencyProperty buttonCommandProperty =
DependencyProperty.Register("ButtonCommand", typeof(ICommand),
typeof(ImageButton), null);

        public static readonly DependencyProperty buttonCommandParamProperty =
DependencyProperty.Register("ButtonCommandParameter", typeof(object),
typeof(ImageButton), null);

        public object ButtonCommandParameter
        {
            get => GetValue(buttonCommandParamProperty);
            set => SetValue(buttonCommandParamProperty, value);
        }

        public string ButtonText
        {
            get => GetValue(buttonTextProperty).ToString()!;
            set => SetValue(buttonTextProperty, value);
        }
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(imageSourceProperty);
            set => SetValue(imageSourceProperty, value);
        }
        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(buttonCommandProperty);
            set => SetValue(buttonCommandProperty, value);
        }
    }
}