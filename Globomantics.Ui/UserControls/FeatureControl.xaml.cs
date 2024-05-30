using Globomantics.Ui.ViewModels;
using System.Windows.Controls;

namespace Globomantics.Ui.UserControls;

/// <summary>
/// Interaction logic for FeatureControl.xaml
/// </summary>
public partial class FeatureControl : UserControl
{
    public FeatureControl(IViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        ParentTodo.SelectedIndex = -1;
    }
}
