//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolManagement.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tool_Bank
    {
        public string bank_applicant { get; set; }
        public string type_number { get; set; }
        public string tool_number { get; set; }
        public string tool_location { get; set; }
        public string tool_buyticket { get; set; }
        public System.DateTime tool_buydate { get; set; }
        public string tool_model { get; set; }
        public string tool_picture { get; set; }
        public string tool_lifetime { get; set; }
        public int tool_first_trial { get; set; }
        public string first_operator { get; set; }
        public int tool_second_trial { get; set; }
        public string second_operator { get; set; }
        public int tool_status { get; set; }
    }
}
