// Ishan Pranav's REBUS: MainForm.Designer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Windows.Forms;

namespace Rebus.Client.Windows
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.visionPictureBox = new System.Windows.Forms.PictureBox();
            this.visionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.unitCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.renameButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.visionPictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.nameGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // visionPictureBox
            // 
            this.visionPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.visionPictureBox, "visionPictureBox");
            this.visionPictureBox.Name = "visionPictureBox";
            this.visionPictureBox.TabStop = false;
            this.visionPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnVisionPictureBoxMouseClick);
            // 
            // visionToolTip
            // 
            this.visionToolTip.IsBalloon = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.visionPictureBox);
            this.panel1.Name = "panel1";
            // 
            // unitCheckedListBox
            // 
            resources.ApplyResources(this.unitCheckedListBox, "unitCheckedListBox");
            this.unitCheckedListBox.FormattingEnabled = true;
            this.unitCheckedListBox.Name = "unitCheckedListBox";
            this.unitCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnUnitCheckedListBoxItemCheck);
            this.unitCheckedListBox.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.OnUnitCheckedListBoxFormat);
            // 
            // renameButton
            // 
            resources.ApplyResources(this.renameButton, "renameButton");
            this.renameButton.Name = "renameButton";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Click += new System.EventHandler(this.OnRenameButtonClick);
            // 
            // nameTextBox
            // 
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.Name = "nameTextBox";
            // 
            // nameGroupBox
            // 
            resources.ApplyResources(this.nameGroupBox, "nameGroupBox");
            this.nameGroupBox.Controls.Add(this.renameButton);
            this.nameGroupBox.Controls.Add(this.nameTextBox);
            this.nameGroupBox.Name = "nameGroupBox";
            this.nameGroupBox.TabStop = false;
            this.nameGroupBox.EnabledChanged += new System.EventHandler(this.OnNameGroupBoxEnabledChanged);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nameGroupBox);
            this.Controls.Add(this.unitCheckedListBox);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnMainFormClosing);
            this.Load += new System.EventHandler(this.OnMainFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.visionPictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.nameGroupBox.ResumeLayout(false);
            this.nameGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox visionPictureBox;
        private ToolTip visionToolTip;
        private Panel panel1;
        private CheckedListBox unitCheckedListBox;
        private Button renameButton;
        private TextBox nameTextBox;
        private GroupBox nameGroupBox;
    }
}
