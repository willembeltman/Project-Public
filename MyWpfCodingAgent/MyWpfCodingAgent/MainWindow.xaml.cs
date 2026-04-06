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

namespace MyWpfCodingAgent;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void SendPromptButton_Click(object sender, RoutedEventArgs e)
    {
        var prompt = PromptTextBox.Text;
        // TODO: send to agent
    }

    private void BrowseDirectory_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            CheckFileExists = false,
            CheckPathExists = true,
            FileName = "Select folder"
        };

        if (dialog.ShowDialog() == true)
        {
            var path = System.IO.Path.GetDirectoryName(dialog.FileName);
            DirectoryTextBox.Text = path;
        }
    }

    private void LlmEventsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = LlmEventsList.SelectedItem;
        if (selected != null)
        {
            EventDetailsTextBox.Text = selected.ToString();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}