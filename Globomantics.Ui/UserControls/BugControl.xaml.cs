using Globomantics.Ui.ViewModels;
using System.Windows.Controls;

namespace Globomantics.Ui.UserControls;

/// <summary>
/// Interaction logic for BugControl.xaml
/// </summary>
public partial class BugControl : UserControl
{
    public BugControl(IViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        ParentTodo.SelectedIndex = -1;
    }
}