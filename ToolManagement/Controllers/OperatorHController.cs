using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolManagement.DBModel;

namespace ToolManagement.Controllers
{
    public class OperatorHController : Controller
    {
        ToolEntities tool = new ToolEntities();
        // GET: OperatorH
        public ActionResult Index()
        {
            return View();
        }
        //处理报修申请
        public ActionResult Repair()
        {
            return View();
        }
        public ActionResult Scrap()
        {
            return View();
        }
        //获取所有报修信息
        public void getRepair()
        {
            CommonModel msg = new CommonModel();
            var result = from repair in tool.tool_Repair
                         where repair.status == 0
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
        //处理报修信息
        public void handleRepair(int repair_id, string repair_operator, string refuse_reason, int repair_status)
        {
            var result = from u in tool.tool_Repair
                         where u.repair_id == repair_id
                         select u;
            tool_Repair repair = result.ToList().FirstOrDefault<tool_Repair>();
            if (repair != null)
            {
                repair.repair_operator = repair_operator;
                repair.refuse_reason = refuse_reason;
                repair.repair_status = repair_status;
            }
            tool.SaveChanges();
            CommonModel msg = new CommonModel();
            msg.msg = "操作成功";
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //提交报废申请
        [HttpPost]
        public void scrapReply(string scrap_applicant,string tool_number,string scrap_reason)
        {
            CommonModel msg = new CommonModel();
            var scrap = new tool_Scrap()
            {
                scrap_applicant = scrap_applicant,
                tool_number = tool_number,
                scrap_reason = scrap_reason,
                scrap_date = DateTime.Now,
                scrap_first_trial = 0,
                scrap_second_trial = 0,
                status = 0
            };
            tool.tool_Scrap.Add(scrap);
            try
            {
               tool.SaveChanges();
               msg.msg = "0";  //上传成功 
            }
            catch (Exception)
            {
                throw;
            }
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //获取提交的报废记录
        [HttpPost]
        public void GetScrapInformation(string user_number)
        {
            CommonModel msg = new CommonModel();
            var result = from scrap in tool.tool_Scrap
                         where scrap.scrap_applicant == user_number && scrap.status == 0
                         select scrap;
            ArrayList data = new ArrayList();
            foreach (tool_Scrap scrap in result)
            {
                data.Add(scrap);
            }
            msg.data = data;
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
        //撤销还未处理的申请
        public void Backout(int scrap_id)
        {
            var result = from scrap in tool.tool_Scrap
                         where scrap.scrap_id == scrap_id
                         select scrap;
            tool_Scrap src = result.ToList().FirstOrDefault<tool_Scrap>();
            if (src != null)
                src.status = 1;
            tool.SaveChanges();
            Response.Write("200");
        }
    }
}