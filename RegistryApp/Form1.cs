using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace RegistryApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //GetRegistryTree();
            LoadRegistryKeys(treeView1);
        }

        //эту функцию можешь удалить
        private void GetRegistryTree()
        {
            RegistryKey localMachine = Registry.LocalMachine;
            treeView1.Nodes.Add(localMachine.ToString());
            string[] ValuesNames = localMachine.GetSubKeyNames();

            for (int i = 0; i < localMachine.SubKeyCount; i++)
            {
                treeView1.Nodes[0].Nodes.Add(ValuesNames[i]);
            }


            RegistryKey classesRoot = Registry.ClassesRoot;
            treeView1.Nodes.Add(classesRoot.ToString());
            ValuesNames = classesRoot.GetSubKeyNames();

            for (int i = 0; i < classesRoot.SubKeyCount; i++)
            {
                treeView1.Nodes[1].Nodes.Add(ValuesNames[i]);
            }


            RegistryKey currentUser = Registry.CurrentUser;
            treeView1.Nodes.Add(currentUser.ToString());
            ValuesNames = currentUser.GetSubKeyNames();

            for (int i = 0; i < currentUser.SubKeyCount; i++)
            {
                treeView1.Nodes[2].Nodes.Add(ValuesNames[i]);
            }

            var asd = currentUser.GetValueNames();



            RegistryKey users = Registry.Users;
            treeView1.Nodes.Add(users.ToString());
            ValuesNames = users.GetSubKeyNames();

            for (int i = 0; i < users.SubKeyCount; i++)
            {
                treeView1.Nodes[3].Nodes.Add(ValuesNames[i]);
            }



            RegistryKey currentConfig = Registry.CurrentConfig;
            treeView1.Nodes.Add(currentConfig.ToString());
            ValuesNames = currentConfig.GetSubKeyNames();

            for (int i = 0; i < currentConfig.SubKeyCount; i++)
            {
                treeView1.Nodes[4].Nodes.Add(ValuesNames[i]);
            }
        }

        private void LoadRegistryKeys(TreeView treeView)
        {
            RegistryKey currentUser = Registry.CurrentUser;

            TreeNode currentUserNode = new TreeNode(currentUser.Name);
            treeView.Nodes.Add(currentUserNode);

            LoadSubKeys(currentUser, currentUserNode);



            RegistryKey currentConfig = Registry.CurrentConfig;

            TreeNode currentConfigNode = new TreeNode(currentConfig.Name);
            treeView.Nodes.Add(currentConfigNode);

            LoadSubKeys(currentConfig, currentConfigNode);



            RegistryKey users = Registry.Users;

            TreeNode usersNode = new TreeNode(users.Name);
            treeView.Nodes.Add(usersNode);

            LoadSubKeys(users, usersNode);



            RegistryKey classesRoot = Registry.ClassesRoot;

            TreeNode classesRootNode = new TreeNode(classesRoot.Name);
            treeView.Nodes.Add(classesRootNode);

            LoadSubKeys(classesRoot, classesRootNode);



            RegistryKey localMachine = Registry.LocalMachine;

            TreeNode localMachineNode = new TreeNode(localMachine.Name);
            treeView.Nodes.Add(localMachineNode);

            LoadSubKeys(localMachine, localMachineNode);
        }

        private void LoadSubKeys(RegistryKey parentKey, TreeNode parentNode)
        {
            try
            {
                foreach (string subKeyName in parentKey.GetSubKeyNames())
                {
                    RegistryKey subKey = parentKey.OpenSubKey(subKeyName);

                    TreeNode subNode = new TreeNode(subKeyName);

                    LoadSubKeys(subKey, subNode);

                    parentNode.Nodes.Add(subNode);
                }

                foreach (string valueName in parentKey.GetValueNames())
                {
                    object value = parentKey.GetValue(valueName);

                    TreeNode valueNode = new TreeNode($"{valueName}: {value}");

                    parentNode.Nodes.Add(valueNode);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("access denied");
            }
        }

        public void CreateRegistryValue(TreeNode selectedNode)
        {
            RegistryKey key = GetRegistryKey(selectedNode);

            if (key != null)
            {
                using (RegistryKey subKey = key.CreateSubKey(textBox1.Text))
                {
                    subKey.SetValue(textBox2.Text, textBox3.Text);
                }
            }
        }

        private RegistryKey GetRegistryKey(TreeNode treeNode)
        {
            string path = treeNode.FullPath.Replace("HKEY_CURRENT_USER\\", "");
            string g = treeNode.FirstNode.Name;

            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);
                return key;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                
            }

            CreateRegistryValue(treeView1.SelectedNode);

            
        }
    }
}
