using LanCloud.Domain.Application;
using LanCloud.Domain.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanCloud.Forms
{
    public partial class StatusForm : Form
    {
        public StatusForm(LocalApplication application)
        {
            Application = application;

            Application.OnStateChanged += Application_OnStateChanged;
            FormClosing += StatusForm_FormClosing;

            InitializeComponent();
        }

        private void StatusForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            KillSwitch = true;
        }

        private void Application_OnStateChanged(object sender, EventArgs e)
        {
            if (!KillSwitch)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ShowStatus();
                });
            }
        }

        public LocalApplication Application { get; }
        public bool KillSwitch { get; private set; }

        private void StatusForm_Load(object sender, EventArgs e)
        {
            ShowStatus();
        }

        private void ShowStatus()
        {
            treeView1.Nodes.Clear();

            // Hoofdnode
            var applicationNode = new TreeNode($"LocalApplication");
            applicationNode.Nodes.Add(new TreeNode($"{Application.HostName}:{Application.Port}: {Application.Status}"));
            treeView1.Nodes.Add(applicationNode);

            // Eerste laag
            if (Application.LocalShares.Any())
            {
                var localSharesNode = new TreeNode("LocalShares");
                foreach (var localShare in Application.LocalShares)
                {
                    // Tweede laag onder Child 1
                    localSharesNode.Nodes.Add(new TreeNode($"{localShare.HostName}:{localShare.Port}: {localShare.Status}"));
                }
                applicationNode.Nodes.Add(localSharesNode);
            }

            if (Application.RemoteApplications.Any())
            {
                var remoteApplicationsNode = new TreeNode("RemoteApplications");
                foreach (var remoteApplication in Application.RemoteApplications)
                {
                    var remoteApplicationNode = new TreeNode($"{remoteApplication.HostName}:{remoteApplication.Port}: {remoteApplication.Status}");

                    if (remoteApplication.RemoteShares.Any())
                    {
                        var remoteSharesNode = new TreeNode("RemoteShares");
                        foreach (var remoteShare in remoteApplication.RemoteShares)
                        {
                            // Tweede laag onder Child 1
                            remoteSharesNode.Nodes.Add(new TreeNode($"{remoteShare.HostName}:{remoteShare.Port}: {remoteShare.Status}"));
                        }
                        remoteApplicationNode.Nodes.Add(remoteSharesNode);
                    }

                    remoteApplicationsNode.Nodes.Add(remoteApplicationNode);
                }
                treeView1.Nodes.Add(remoteApplicationsNode);
            }

            var virtualFtpServerNode = new TreeNode($"VirtualFtpServer");
            virtualFtpServerNode.Nodes.Add(new TreeNode($"{Application.VirtualFtpServer.HostName}:{Application.VirtualFtpServer.Port}: {Application.VirtualFtpServer.FtpServer.Status}"));
            treeView1.Nodes.Add(virtualFtpServerNode);



            // Open de boomstructuur
            treeView1.ExpandAll();

        }
        public void Kill()
        {
            KillSwitch = true;
            this.Close();
        }
    }
}
