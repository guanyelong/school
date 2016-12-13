using SP.Business.HIS;
using SP.Models;
using SP.Models.HIS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SP.HIS.Models
{
    public class JTreeCommon
    {
        private MenuItemBLL menuBll = new MenuItemBLL();

        /// <summary>
        ///  根据权限获取MenuTree
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Hashtable> GetUserTreeList(string id)
        {
            //var menus = xDoc.GetTreeData();
            UserRoleBLL userRoleBLL = new UserRoleBLL();
            //登录失效
            if (AdminSystemInfo.CurrentUser == null)
            {
                return new List<Hashtable>();
            }

            id = string.IsNullOrEmpty(id) ? "01" : id;

            int userId = AdminSystemInfo.CurrentUser.ID;
            //根据权限获取xml所有数据
            var menus = userRoleBLL.GetAppUserMenuList(userId);
            //获取顶级菜单
            var parentMenus = menus.Where(o => o.ParentID == Convert.ToInt32(id)).OrderBy(o => o.ORDERINDEX);

            List<Hashtable> treeList = new List<Hashtable>();
            foreach (SYS_ITEMMENU item in parentMenus)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", item.ID);
                ht.Add("text", item.TEXT);
                ht.Add("checked", item.CHECKED);

                Dictionary<string, string> attributes = new Dictionary<string, string>();
                attributes.Add("url", item.URL);
                ht.Add("attributes", attributes);
                ht.Add("state", item.STATE);
                InitMenuTreeChildren(menus, ht, item.ID);
                treeList.Add(ht);
            }
            return treeList;
        }
        //加载子级节点var
        private void InitMenuTreeChildren(List<SYS_ITEMMENU> menuList, Hashtable ht, int parentId)
        {
            var childrenList = menuList.Where(o => o.ParentID == parentId).OrderBy(o => o.ORDERINDEX);
            int childrenCount = childrenList.Count();
            if (childrenCount > 0)
            {
                List<Hashtable> cList = new List<Hashtable>();
                foreach (var item in childrenList)
                {
                    Hashtable cht = new Hashtable();
                    cht.Add("id", item.ID);
                    cht.Add("text", item.TEXT);
                    cht.Add("checked", item.CHECKED);
                    Dictionary<string, string> attributes = new Dictionary<string, string>();
                    attributes.Add("url", item.URL);
                    cht.Add("attributes", attributes);
                    cht.Add("state", item.STATE);
                    InitMenuTreeChildren(menuList, ht, item.ID);
                    cList.Add(cht);
                }
                ht.Add("children", cList);
            }
        }
    }
}