using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ToolManagement.DBModel;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;

namespace ToolManagement.Controllers
{
    class CommonModel
    {
        //消息
        private string _msg;
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        //数据
        private object _data;
        public object data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
    public class HomeController : Controller
    {
        ToolEntities tool = new ToolEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Unlogin()
        {
            return View();
        }
        //查员工名字
        [HttpPost]
        public void getPersonName(string user_number)
        {
            var result = from u in tool.tool_User
                         where user_number == u.user_number
                         select new { u.user_name };
            if (result.Count() == 0)
            {
                Response.Write("0");
                Response.End();
            }
            else
            {
                foreach (var item in result)
                {
                    Response.Write(item.user_name);
                    Response.End();
                }
            }

        }
        //查库存信息
        [HttpPost]
        public void getToolInformation(string tool_number)
        {
            var result = from u in tool.tool_Bank
                         where u.tool_number == tool_number
                         select u;
            if (result.Count() == 0)
            {
                 Response.Write("0");
                 Response.End();
            }
            else
            {
                foreach (tool_Bank tool in result)
                {
                    object JSONObj = JsonConvert.SerializeObject(tool);
                    Response.Write(JSONObj);
                    Response.End();
                }
            }
        }
        //登录
        [HttpPost]
        public void login(string user_number, string password)
        {
            var result = from u in tool.tool_User
                         where user_number == u.user_number && u.password == password
                         select u;
            if (result.Count() == 0)
            {
                var jsonData = "{\"msg\":\"密码错误\"}";
                Response.Write(jsonData);
                Response.End();
            }
            else
            {
                CommonModel msg = new CommonModel();
                msg.msg = "密码正确";
                foreach (var item in result)
                {
                    msg.data = item.user_authority;
                }
                object JSONObj = JsonConvert.SerializeObject(msg);
                Response.Write(JSONObj);
                Response.End();
            }
        }
        //获取用户身份,姓名,头像
        public void GetHeadimage(string userid)
        {
            CommonModel msg = new CommonModel();
            var result = from tool in tool.tool_User
                         where tool.user_number == userid
                         select new
                         {
                             tool.user_name,
                             tool.user_authority,
                             tool.user_picture
                         };
            foreach (var item in result)
            {
                var data = new
                {
                    user_name = item.user_name,
                    user_authority = item.user_authority,
                    user_picture = item.user_picture
                };
                msg.data = data;
            }
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //获取库存信息
        public void GetBank()
        {
            CommonModel msg = new CommonModel();
            var result = from Tool in tool.tool_Bank
                         select new
                         {
                             Tool.tool_number,
                             Tool.tool_location,
                             Tool.tool_buydate,
                             Tool.tool_model,
                             Tool.tool_status
                         };
            ArrayList data = new ArrayList();
            foreach (var item in result)
            {
                var items = new
                {
                    tool_number = item.tool_number,
                    tool_location = item.tool_location,
                    tool_buydate = item.tool_buydate,
                    tool_model = item.tool_model,
                    tool_status = item.tool_status
                };
                data.Add(items);
            }
            msg.data = data;
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
    }
}