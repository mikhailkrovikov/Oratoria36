using System.Windows;
using System.Windows.Media;

namespace Oratoria.UI.Controls.DialogWindows;

public enum MBType
{
    Info,
    Warning,
    Error
}

public enum MBButtons
{
    OK,
    Okcancel
}

public enum MBResult
{
    None,
    Ok,
    Cancel
}

public partial class UserMessageBox : Window
{
    public MBResult Result { get; private set; }

    private UserMessageBox(MBType type, string text, string caption, MBButtons buttons)
    {
        InitializeComponent();
        ConfigButtons(buttons);
        ConfigType(type);
        TextTB.Text = text;
        CaptionTB.Text = caption;
    }

    public static MBResult Show(string text)
        => Show(text, string.Empty, MBType.Info, MBButtons.OK);

    public static MBResult Show(string text, string caption)
        => Show(text, caption, MBType.Info, MBButtons.OK);

    public static MBResult Show(string text, string caption, MBType type)
        => Show(text, caption, type, MBButtons.OK);

    public static MBResult Show(string text, string caption, MBType type, MBButtons buttons)
    {
        var box = new UserMessageBox(type, text, caption, buttons);
        box.ShowDialog();
        return box.Result;
    }

    private void ConfigButtons(MBButtons buttons)
    {
        switch (buttons)
        {
            case MBButtons.OK:
                OKbutton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Hidden;
                break;
            case MBButtons.Okcancel:
                OKbutton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Visible;
                break;
        }
    }

    private void ConfigType(MBType type)
    {
        HeaderBar.Fill = type switch
        {
            MBType.Info => (Brush)FindResource("Brush.Accent"),
            MBType.Warning => (Brush)FindResource("Brush.Warning"),
            MBType.Error => (Brush)FindResource("Brush.Danger"),
            _ => HeaderBar.Fill
        };
        CaptionTB.Foreground = Brushes.White;
    }

    private void OKbutton_Click(object sender, RoutedEventArgs e)
    {
        Result = MBResult.Ok;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Result = MBResult.Cancel;
        Close();
    }
}
