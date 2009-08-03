﻿/*
* Copyright (c) 2007-2009 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SlimTuneUI
{
	public partial class ConnectDialog : Form
	{
		MainWindow m_mainWindow;

		public ConnectDialog(MainWindow mainWindow)
		{
			InitializeComponent();
			m_mainWindow = mainWindow;
		}

		private void m_browseDbButton_Click(object sender, EventArgs e)
		{
			var result = m_saveResultsDialog.ShowDialog(this);
			if(result == DialogResult.OK)
				m_resultsFileTextBox.Text = m_saveResultsDialog.FileName;
		}

		private bool Connect(string host, int port)
		{
			string dbFile = m_resultsFileTextBox.Text;

			//connect to storage before launching the process -- we don't want to launch if this fails
			IStorageEngine storage = null;
			try
			{
				storage = new SqlServerCompactEngine(dbFile, true);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			ConnectProgress progress = new ConnectProgress("localhost", port, storage, 10);
			progress.ShowDialog(this);

			if(progress.Client != null)
			{
				Connection conn = new Connection(storage);
				conn.HostName = host;
				conn.Port = port;
				conn.RunClient(progress.Client);

				SqlVisualizer resultsWindow = new SqlVisualizer();
				resultsWindow.Initialize(m_mainWindow, conn);
				resultsWindow.Show(m_mainWindow.DockPanel);
			}
			else
			{
				storage.Dispose();
				return false;
			}

			return true;
		}

		private void m_connectButton_Click(object sender, EventArgs e)
		{
			bool result = Connect(m_hostNameTextBox.Text, int.Parse(m_portTextBox.Text));
			if(!result)
				return;

			this.Close();
		}
	}
}