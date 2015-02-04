using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Uber.Base;

namespace Uber.Core
{
    public class Trip
    {
        private List<TreeNode> treeNodeList = new List<TreeNode>();

        private List<List<Location>> locationList = new List<List<Location>>();

        public decimal Fare
        {
            get;
            set;
        }

        public void AddToTreeNodeList(TreeNode node)
        {
            if (!this.treeNodeList.Contains(node))
            {
                this.treeNodeList.Add(node);
            }
        }

        public void AddToLocationList(Location loc)
        {
            
        }
    }
}