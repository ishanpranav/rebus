// Ishan Pranav's REBUS: ConnectionForm.Designer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Windows.Forms;

namespace Rebus.Client.Windows
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.submitButton = new System.Windows.Forms.Button();
            this.loginEntityGrid = new Rebus.Client.Windows.EntityGrid();
            this.myTabControl = new System.Windows.Forms.TabControl();
            this.loginTabPage = new System.Windows.Forms.TabPage();
            this.registerTabPage = new System.Windows.Forms.TabPage();
            this.registerEntityGrid = new Rebus.Client.Windows.EntityGrid();
            this.myTabControl.SuspendLayout();
            this.loginTabPage.SuspendLayout();
            this.registerTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            resources.ApplyResources(this.submitButton, "submitButton");
            this.submitButton.Name = "submitButton";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.OnSubmitButtonClick);
            // 
            // loginEntityGrid
            // 
            resources.ApplyResources(this.loginEntityGrid, "loginEntityGrid");
            this.loginEntityGrid.Name = "loginEntityGrid";
            this.loginEntityGrid.Saver = null;
            this.loginEntityGrid.SelectedObject = null;
            // 
            // myTabControl
            // 
            resources.ApplyResources(this.myTabControl, "myTabControl");
            this.myTabControl.Controls.Add(this.loginTabPage);
            this.myTabControl.Controls.Add(this.registerTabPage);
            this.myTabControl.Name = "myTabControl";
            this.myTabControl.SelectedIndex = 0;
            // 
            // loginTabPage
            // 
            this.loginTabPage.Controls.Add(this.loginEntityGrid);
            resources.ApplyResources(this.loginTabPage, "loginTabPage");
            this.loginTabPage.Name = "loginTabPage";
            this.loginTabPage.UseVisualStyleBackColor = true;
            // 
            // registerTabPage
            // 
            this.registerTabPage.Controls.Add(this.registerEntityGrid);
            resources.ApplyResources(this.registerTabPage, "registerTabPage");
            this.registerTabPage.Name = "registerTabPage";
            this.registerTabPage.UseVisualStyleBackColor = true;
            // 
            // registerEntityGrid
            // 
            resources.ApplyResources(this.registerEntityGrid, "registerEntityGrid");
            this.registerEntityGrid.Name = "registerEntityGrid";
            this.registerEntityGrid.Saver = null;
            this.registerEntityGrid.SelectedObject = null;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.submitButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.myTabControl);
            this.Controls.Add(this.submitButton);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.myTabControl.ResumeLayout(false);
            this.loginTabPage.ResumeLayout(false);
            this.registerTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button submitButton;
        private EntityGrid loginEntityGrid;
        private TabControl myTabControl;
        private TabPage loginTabPage;
        private TabPage registerTabPage;
        private EntityGrid registerEntityGrid;
    }
}
