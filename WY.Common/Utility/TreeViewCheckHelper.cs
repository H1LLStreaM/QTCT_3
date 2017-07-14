using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WY.Common.Utility
{
    public class TreeViewCheckHelper
    {
        private bool flag = false;
        private TreeView ctl;

        public TreeViewCheckHelper(TreeView treeview)
        {
            ctl = treeview;
            ctl.AfterCheck += new TreeViewEventHandler(ctl_AfterCheck);

        }

        void ctl_AfterCheck(object sender, TreeViewEventArgs e)
        {
            SelectTreeView(e);
        }

        public void SelectTreeView(TreeViewEventArgs e)
        {
            if (flag)
            {
                return;
            }
            
            if (e.Node.Checked)
            {
                SetChildNodeCheckedState(e.Node, true);
                if (e.Node.Parent != null)
                {
                    flag = true;
                    //e.Node.Parent.Checked = true;
                    SetParentNodeCheckedState(e.Node, true);
                    flag = false;
                }
            }
            else
            {
                SetChildNodeCheckedState(e.Node, false);
                if (e.Node.Parent != null)
                {
                    CheckAllbrother(e.Node);
                }
            }
        }

        private void CheckAllbrother(TreeNode node)
        {

            TreeNodeCollection nodes = node.Parent.Nodes;
            if (nodes.Count > 0)
            {
                flag = true;
                int i = 0;
                foreach (TreeNode tn in nodes)
                {
                    if (tn.Checked)
                    {
                        i = 1;
                        flag = false;
                        break;
                    }
                }
                flag = false;
                if (i == 0)
                {
                    flag = true;
                    node.Parent.Checked = false;
                    flag = false;
                }
                if (node.Parent.Parent != null)
                {
                    CheckAllbrother(node.Parent);
                }
            }

        }

        private void SetChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
                foreach (TreeNode tn in nodes)
                {
                    flag = true;
                    tn.Checked = state;
                    flag = false;

                    SetChildNodeCheckedState(tn, state);
                }
        }

        private void SetParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;

            flag = true;
            parentNode.Checked = state;
            flag = false;
            if (currNode.Parent.Parent != null)
            {
                SetParentNodeCheckedState(currNode.Parent, state);
            }
        }
    }
}
