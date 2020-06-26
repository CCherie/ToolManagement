using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToolManagement.DBModel;

namespace ToolManagement.Controllers
{
    public class ManagerController : Controller
    {
        ToolEntities tool = new ToolEntities();
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Scrap()
        {
            return View();
        }
        //获取所有报废信息
        public void getScrap()
        {
            CommonModel msg = new CommonModel();
            var result = from scrap in tool.tool_Scrap
                         where scrap.status == 0 && scrap.scrap_first_trial != 0
                         select scrap;
            ArrayList data = new ArrayList();
            foreach (tool_Scrap scrap in result)
            {
                data.Add(scrap);
            }
            msg.data = data;
            object JSONobj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONobj);
            Response.End();
        }
        //二审报废信息
        public void ScrapFirstTrial(int scrap_id, string second_operator, int second_first_trial)
        {
            var result = from u in tool.tool_Scrap
                         where u.scrap_id == scrap_id
                         select u;
            tool_Scrap scrap = result.ToList().FirstOrDefault<tool_Scrap>();
            if (scrap != null)
            {
                scrap.scrap_second_trial = scrap.scrap_second_trial;
                scrap.second_operator = scrap.second_operator;
            }
            tool.SaveChanges();
            CommonModel msg = new CommonModel();
            msg.msg = "操作成功";
            object JSONObj = JsonConvert.SerializeObject(msg);
            Response.Write(JSONObj);
            Response.End();
        }
    }
}