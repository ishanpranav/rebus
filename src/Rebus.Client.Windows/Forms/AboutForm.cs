﻿// Ishan Pranav's REBUS: AboutForm.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;

namespace Rebus.Client.Windows.Forms
{
    internal sealed partial class AboutForm : FormBase
    {
        public AboutForm()
        {
            InitializeComponent();

            environmentVersionLabel.Text = string.Format(environmentVersionLabel.Text, Environment.Version, Environment.OSVersion);
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnAboutFormLoad(object sender, EventArgs e)
        {
            myPictureBox.Image = Image.FromFile(Path.ChangeExtension(Path.Combine(path1: "images", path2: "Rebus"), extension: "png"));

            myListBox.Items.Add(new Notice(titleLabel.Text, string.Format(Resources.LicenseFormat, titleLabel.Text), await File.ReadAllTextAsync(getPath(key: "LICENSE"))));

            myListBox.SelectedIndex = 0;

            using (TextReader reader = File.OpenText(getPath(key: "THIRD-PARTY-NOTICES")))
            {
                await foreach (Notice notice in Notice.ParseAsync(reader))
                {
                    myListBox.Items.Add(notice);
                }
            }

            static string getPath(string key)
            {
                return Path.ChangeExtension(Path.Combine(path1: "resources", key), extension: "txt");
            }
        }

        private void OnListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            Notice notice = (Notice)myListBox.SelectedItem;

            headerLabel.Text = notice.Title;
            myTextBox.Text = notice.Body;
        }
    }
}
