// Ishan Pranav's REBUS: LoginForm.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Client.Windows.Properties;

namespace Rebus.Client.Windows
{
    internal partial class LoginForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        private Credentials _credentials;

        [MemberNotNull(nameof(_credentials))]
        public void SetCredentials(Credentials value)
        {
            _credentials = value;
            loginEntityGrid.SelectedObject = value;
            registerEntityGrid.SelectedObject = value;
        }

        public LoginForm(Credentials credentials, IServiceProvider serviceProvider, ObjectSaver saver)
        {
            InitializeComponent();
            SetCredentials(credentials);

            _serviceProvider = serviceProvider;
            loginEntityGrid.Saver = saver;
            registerEntityGrid.Saver = saver;
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnSubmitButtonClick(object sender, EventArgs e)
        {
            if (loginEntityGrid.ValidateObject())
            {
                _credentials.ApplyCulture();

                try
                {
                    await using (RpcClient<ILoginService> client = new RpcClient<ILoginService>())
                    {
                        await client.ConnectAsync(_credentials.IPAddress, _credentials.Port);

                        bool isLogin = myTabControl.SelectedTab == loginTabPage;

                        if (isLogin)
                        {
                            _credentials.PlayerId = await client.Service.LoginAsync(_credentials.Username, _credentials.Password);
                        }
                        else
                        {
                            _credentials.PlayerId = await client.Service.RegisterAsync(_credentials.Username, _credentials.Password);
                        }

                        if (_credentials.PlayerId != default)
                        {
                            Hide();

                            _serviceProvider
                                .GetRequiredService<MainForm>()
                                .Show();
                        }
                        else
                        {
                            string errorMessage;

                            if (isLogin)
                            {
                                errorMessage = Resources.LoginErrorMessage;
                            }
                            else
                            {
                                errorMessage = Resources.RegisterErrorMessage;
                            }

                            MessageBox.Show(errorMessage, Resources.WarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Resources.ErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
