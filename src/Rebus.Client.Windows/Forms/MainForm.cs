// Ishan Pranav's REBUS: MainForm.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;

namespace Rebus.Client.Windows
{
    internal partial class MainForm : Form
    {
        private const int XOffset = 512;
        private const int YOffset = 512;

        private readonly Credentials _credentials;
        private readonly RpcClient<IGameService> _client;
        private readonly Layout _layout;
        private readonly GraphicsEngine _graphicsEngine;
        private readonly Dictionary<HexPoint, ZoneResult> _zones = new Dictionary<HexPoint, ZoneResult>();

        //private readonly HexPoint _origin;

        private HexPoint _location;

        public MainForm(Credentials credentials, RpcClient<IGameService> gameService)
        {
            InitializeComponent();

            _credentials = credentials;
            _client = gameService;
            _layout = new Layout(XOffset, YOffset);
            _graphicsEngine = new GraphicsEngine(XOffset * 2, YOffset * 2);
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMainFormLoad(object sender, EventArgs e)
        {
            await _client.ConnectAsync(_credentials.IPAddress, _credentials.Port);

            int radius = await _client.Service.GetRadiusAsync();
            float scaleFactor = 2f / ((radius * 4) + 1);

            _layout.HexagonWidth = XOffset * scaleFactor;
            _layout.HexagonHeight = YOffset * scaleFactor;

            await RequestUnitsAsync();

            DrawZones();
        }

        private void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void OnVisionPictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            DrawZones();
            DrawUnits();

            _location = _layout.GetHexPoint(e.Location.X, e.Location.Y);

            if (_zones.TryGetValue(_location, out ZoneResult? zone))
            {
                visionToolTip.ToolTipTitle = zone.Name;

                visionToolTip.Show(_location.ToString(), visionPictureBox, e.Location);
            }
            else
            {
                visionToolTip.Hide(visionPictureBox);
            }
        }

        private void DrawUnits()
        {
            unitCheckedListBox.Items.Clear();
            nameGroupBox.Enabled = false;

            if (_zones.TryGetValue(_location, out ZoneResult? zone))
            {
                foreach (Unit unit in zone.Value.Units)
                {
                    unitCheckedListBox.Items.Add(unit);
                }
            }
        }

        private async Task RequestUnitsAsync()
        {
            _zones.Clear();

            await foreach (ZoneResult zone in _client.Service.GetZonesAsync(_credentials.PlayerId))
            {
                _zones.Add(zone.Value.Location, zone);
            }
        }

        private void DrawZones()
        {
            visionPictureBox.Image?.Dispose();

            using (MemoryStream memoryStream = new MemoryStream())
            using (SKManagedWStream output = new SKManagedWStream(memoryStream))
            {
                _graphicsEngine.Draw(output, SKEncodedImageFormat.Png, new ZoneVisualizer(_zones.Values, _credentials.PlayerId, _layout));

                memoryStream.Position = 0;

                visionPictureBox.Image = Image.FromStream(memoryStream);
            }

            visionPictureBox.Refresh();
        }

        private void OnUnitCheckedListBoxItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool isSingle = unitCheckedListBox.CheckedIndices.Count == 0;

            nameGroupBox.Enabled = isSingle;

            if (isSingle)
            {
                if (nameTextBox.TextLength == 0)
                {
                    nameTextBox.Text = ((Unit)unitCheckedListBox.Items[e.Index]).Name;
                }
            }
            else
            {
                nameTextBox.Clear();
            }
        }

        private void OnUnitCheckedListBoxFormat(object sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is Unit unit)
            {
                e.Value = $"{unit.Name} ()";
            }
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnRenameButtonClick(object sender, EventArgs e)
        {
            string previous = nameTextBox.Text;

            nameTextBox.Text = await _client.Service.RenameAsync(((Unit)unitCheckedListBox.CheckedItems[0]).Id, nameTextBox.Text);

            if (nameTextBox.Text == previous)
            {
                await RequestUnitsAsync();

                DrawUnits();
            }
        }

        private void OnNameGroupBoxEnabledChanged(object sender, EventArgs e)
        {
            nameTextBox.ReadOnly = !nameGroupBox.Enabled;

            if (nameTextBox.ReadOnly)
            {
                nameTextBox.Clear();
            }

            renameButton.Enabled = nameGroupBox.Enabled;
        }
    }
}
