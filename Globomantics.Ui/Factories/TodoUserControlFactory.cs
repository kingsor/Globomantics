// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Controls;
using Globomantics.Ui.UserControls;
using Globomantics.Ui.ViewModels;

namespace Globomantics.Ui.Factories
{
    public class TodoUserControlFactory
    {
        public static UserControl CreateUserControl(ITodoViewModel viewModel)
        {
            UserControl control = viewModel switch
            {
                BugViewModel => new BugControl(viewModel),
                FeatureViewModel => new FeatureControl(viewModel),
                _ => throw new NotImplementedException()
            };

            return control;
        }
    }
}
