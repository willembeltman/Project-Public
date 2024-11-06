using LanCloud.Domain.Application;
using System;
using System.Linq;
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

            var applicationNode = new TreeNode($"LocalApplication");

            var name = "";
            if (Application.RemoteApplicationConfig != null)
            {
                name = $"{Application.RemoteApplicationConfig.HostName}:{Application.RemoteApplicationConfig.Port} ";
            }

            var applicationInnerNode = new TreeNode($"{Application.HostName} {name}{Application.Status}");
            applicationNode.Nodes.Add(applicationInnerNode);

            var connections2 = Application.Server?.GetActiveConnections();
            if (connections2?.Any() == true)
            {
                var connectionsNode = new TreeNode("Connections");
                foreach (var connection in connections2)
                {
                    // Tweede laag onder Child 1
                    connectionsNode.Nodes.Add(new TreeNode($"{connection.Name} {connection.Status}"));
                }
                applicationInnerNode.Nodes.Add(connectionsNode);
            }

            treeView1.Nodes.Add(applicationNode);

            if (Application.LocalShareParts?.Any() == true)
            {
                var localSharesNode = new TreeNode("LocalShares");
                foreach (var localShare in Application.LocalShareParts)
                {
                    var name2 = localShare.Server == null ?
                        " " : $" {localShare.HostName}:{localShare.Port} ";
                    var localShareNode = new TreeNode($"{localShare.LocalShare.FileBits.Root.FullName}{name2}{string.Join("^", localShare.Indexes)} {localShare.Status}");

                    var connections = localShare.Server?.GetActiveConnections();
                    if (connections?.Any() == true)
                    {
                        var connectionsNode = new TreeNode("Connections");
                        foreach (var connection in connections)
                        {
                            // Tweede laag onder Child 1
                            connectionsNode.Nodes.Add(new TreeNode($"{connection.Name}: {connection.Status}"));
                        }
                        localShareNode.Nodes.Add(connectionsNode);
                    }

                    localSharesNode.Nodes.Add(localShareNode);
                }
                applicationNode.Nodes.Add(localSharesNode);
            }

            if (Application.RemoteApplications?.Any() == true)
            {
                var remoteApplicationsNode = new TreeNode("RemoteApplications");
                foreach (var remoteApplication in Application.RemoteApplications)
                {
                    var remoteApplicationNode = new TreeNode($"{remoteApplication.HostName}:{remoteApplication.Port} {remoteApplication.Status}");

                    if (remoteApplication.RemoteShares.Any())
                    {
                        var remoteSharesNode = new TreeNode("RemoteShares");
                        foreach (var remoteShare in remoteApplication.RemoteShares)
                        {
                            // Tweede laag onder Child 1
                            remoteSharesNode.Nodes.Add(new TreeNode($"{remoteShare.HostName}:{remoteShare.Port} {remoteShare.Status}"));
                        }
                        remoteApplicationNode.Nodes.Add(remoteSharesNode);
                    }

                    remoteApplicationsNode.Nodes.Add(remoteApplicationNode);
                }
                treeView1.Nodes.Add(remoteApplicationsNode);
            }

            var virtualFtpServerNode = new TreeNode($"VirtualFtpServer");
            virtualFtpServerNode.Nodes.Add(new TreeNode($"{Application.VirtualFtpServer.HostName}:{Application.VirtualFtpServer.Port} {Application.VirtualFtpServer.FtpServer.Status}"));
            treeView1.Nodes.Add(virtualFtpServerNode);

            //var connections3 = Application.VirtualFtpServer.FtpServer.GetActiveConnections();
            //if (connections3.Any())
            //{
            //    var connectionsNode = new TreeNode("Connections");
            //    foreach (var connection in connections3)
            //    {
            //        // Tweede laag onder Child 1
            //        connectionsNode.Nodes.Add(new TreeNode($"{connection.Name}: {connection.Status}"));
            //    }
            //    applicationInnerNode.Nodes.Add(connectionsNode);
            //}


            // Open de boomstructuur
            treeView1.ExpandAll();

        }
        public void Kill()
        {
            KillSwitch = true;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Application.Logger.LogInfo = checkBox1.Checked;
        }
    }
}
