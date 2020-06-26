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
    public class OperatorLController : Controller
    {
        ToolEntities tool = new ToolEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Repair(string tool_number)
        {
            ViewBag.tool_number = tool_number;
            return View();
        }
        public ActionResult Information()
        {
            return View();
        }
        //获取个人信息
        [HttpPost]
        public void GetInformation(string user_number)
        {
            var result = from u in tool.tool_User
                         where u.user_number == user_number
                         select u;
            CommonModel msg = new CommonModel();
            foreach (var item in result)
            {
                var data = new
                {
                    user_number = item.user_number,
                    user_name = item.user_name,
                    user_gender = item.user_gender,
                    user_address = item.user_address,
                    user_phone = item.user_phone,
                    user_hiredate = item.user_hiredate
                };
                msg.data = data;
            }
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //修改地址或电话
        [HttpPost]
        public void updataInformation(string user_number, string new_phone, string new_address)
        {
            var result = from u in tool.tool_User
                         where u.user_number == user_number
                         select u;
            tool_User user = result.ToList().FirstOrDefault<tool_User>();
            if (user != null)
            {
                if (new_address == null)
                {
                    user.user_phone = new_phone;
                }
                else
                {
                    user.user_address = new_address;
                }
            }
            tool.SaveChanges();
            CommonModel msg = new CommonModel();
            msg.msg = new_address;
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //获取提交的报修记录
        [HttpPost]
        public void getRepairInformation(string user_number)
        {
            CommonModel msg = new CommonModel();
            var result = from repair in tool.tool_Repair
                         where repair.repair_applicant == user_number && repair.status==0
                         select repair;
            ArrayList data = new ArrayList();
            foreach (tool_Repair repair in result)
            {
                data.Add(repair);
            }
            msg.data = data;
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //提交报修申请
        [HttpPost]
        public void repairReply()
        {
            CommonModel msg = new CommonModel();
            var repair_applicant = Request.Form["repair_applicant"];
            var tool_number = Request.Form["tool_number"];
            var repair_reason = Request.Form["repair_reason"];
            HttpPostedFileBase picture = Request.Files["repair_picture"];

            var repair = new tool_Repair();
            repair.repair_applicant = repair_applicant;
            repair.tool_number = tool_number;
            repair.repair_date = DateTime.Now;
            repair.repair_reason = repair_reason;
            repair.repair_status = 0; //状态设为未处理
            repair.status = 0; //报修记录设置为存在

            string path = "~/images/repair/";
            string uploadPath = Server.MapPath(path);//获取上传目录 转换为物理路径 

            if (!Directory.Exists(uploadPath))//判断目录是否存在
            {
                Directory.CreateDirectory(uploadPath);
            }
            string saveFile = uploadPath + picture.FileName; //保存文件的物理路径
            string newfilename = uploadPath + DateTime.Now.ToString("yyMMddhhss") + ".jpg";
            try//保存图片到服务器
            {
                picture.SaveAs(saveFile);
                msg.msg = "0";  //上传成功 
                repair.repair_picture = "../../images/repair/" + DateTime.Now.ToString("yyMMddhhss") + ".jpg";
            }
            catch (Exception)
            {
                msg.msg = "1";
            }
            FileInfo fi = new FileInfo(saveFile);
            fi.MoveTo(newfilename);
            tool.tool_Repair.Add(repair);
            tool.SaveChanges();
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //撤销还未处理的申请
        public void backout(int repair_id)
        {
            var result = from repair in tool.tool_Repair
                         where repair.repair_id==repair_id
                         select repair;
            tool_Repair rep = result.ToList().FirstOrDefault<tool_Repair>();
            if (rep != null)
                rep.status = 1;
            tool.SaveChanges();
            Response.Write("200");
        }
    }
}