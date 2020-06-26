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
using System.IO;

namespace ToolManagement.Controllers
{
    public class AdminController : Controller
    {
        ToolEntities tool = new ToolEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewUser()
        {
            return View();
        }
        //获取用户信息
        public void GetUser()
        {
            CommonModel msg = new CommonModel();
            var result = from user in tool.tool_User
                         where user.status==0
                         select user;
            ArrayList data = new ArrayList();
            foreach (tool_User user in result)
            {
                data.Add(user);
            }
            msg.data = data;
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //添加用户
        [HttpPost]
        public void personReply()
        {
            CommonModel msg = new CommonModel();
            var user_number = Request.Form["user_number"];
            var user_name = Request.Form["user_name"];
            var user_gender = Request.Form["user_gender"];
            var user_age = DateTime.Parse(Request.Form["user_age"]);
            var user_authority = int.Parse(Request.Form["user_authority"]);
            var user_phone = Request.Form["user_phone"];
            var user_address = Request.Form["user_address"];
            
            HttpPostedFileBase picture = Request.Files["user_picture"];
            var user = new tool_User();
            user.user_number = user_number;
            user.user_name = user_name;
            user.user_gender = user_gender;
            user.user_age = user_age;
            user.user_hiredate = DateTime.Now;
            user.user_authority = user_authority;
            user.user_phone = user_phone;
            user.user_address = user_address;
            user.password = "123456";
            user.status = 0;
            string path = "~/images/userImage/";
            string uploadPath = Server.MapPath(path);//获取上传目录 转换为物理路径   
            if (!Directory.Exists(uploadPath))//判断目录是否存在
            {
                Directory.CreateDirectory(uploadPath);
            }
            string saveFile = uploadPath + picture.FileName; //保存文件的物理路径
            string newfilename = uploadPath + user.user_number + ".jpg";
            try//保存图片到服务器
            {
                picture.SaveAs(saveFile);
                msg.msg = "0";  //上传成功 
                user.user_picture = "../../images/userImage/" + user.user_number + ".jpg";
            }
            catch (Exception)
            {
                msg.msg = "1";
            }
            FileInfo fi = new FileInfo(saveFile); 
            fi.MoveTo(newfilename); 
            tool.tool_User.Add(user);
            tool.SaveChanges();
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //注销用户
        public void deleteUser(string user_number)
        {
            var result = from user in tool.tool_User
                         where user.user_number == user_number
                         select user;
            tool_User u = result.ToList().FirstOrDefault<tool_User>();
            if (u != null)
                u.status = 1;
            tool.SaveChanges();
            Response.Write("200");
        }
        //修改用户权限
        [HttpPost]
        public void edit(string user_number,int user_authority)
        {
            var result = from u in tool.tool_User
                         where u.user_number == user_number
                         select u;
            tool_User user = result.ToList().FirstOrDefault<tool_User>();
            if (user != null)
                user.user_authority = user_authority;
            tool.SaveChanges();
            Response.Write("200");
        }
    }
}