using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMP.Models.Master
{
    public class LoginModels
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
    public class GetModels
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public string CDate { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }

    }
    public class Users
    {
        public int Id { get; set; } //(int, not null)
        public string First_Name { get; set; } //(varchar(100), not null)
        public string Last_Name { get; set; } //(varchar(100), null)
        public string Email { get; set; } //(varchar(100), not null)
        public Nullable<DateTime> DOB { get; set; } //(date, null)
        public Nullable<DateTime> DOJ { get; set; } //(date, null)
        public string Phone { get; set; } //(varchar(100), not null)
        public string UsersName { get; set; } //(varchar(100), not null)
        public string Password { get; set; } //(varchar(100), not null)
        public string Gender { get; set; } //(varchar(100), null)
        public int OrganizationId { get; set; } //(int, not null)
        public int RoleId { get; set; } //(int, not null)
        public int DesignationId { get; set; } //(int, not null)
        public int TeamId { get; set; } //(int, not null)
        public bool Active { get; set; } //(bit, not null)
        public string RoleName { get; set; } //(varchar(100), not null)
        public string AccessLevel { get; set; } //(varchar(100), not null)
        public string DesignationName { get; set; } //(varchar(100), not null)
        public string TeamName { get; set; } //(varchar(100), not null)
        public string EmployeeID { get; set; } //(varchar(100), null)

    }
    public class Designation
    {
        public int Id { get; set; } //(int, not null)
        public string Name { get; set; } //(varchar(100), not null)
        public bool Active { get; set; } //(bit, not null)
        public string Description { get; set; } //(varchar(200), null)
        public DateTime Created_date { get; set; } //(datetime, not null)
        public int OrganizationId { get; set; } //(int, not null)
    }
    public class BreakMaster
    {
        public int Id { get; set; } //(int, not null)
        public string Name { get; set; } //(varchar(100), not null)
        public int Max_Break_Time { get; set; } //(int, not null)
        public bool Active { get; set; } //(bit, not null)
        public int OrganizationId { get; set; } //(int, not null)
    }
    public class Team
    {
        public int Id { get; set; } //(int, not null)
        public string Name { get; set; } //(varchar(100), not null)
        public bool Active { get; set; } //(bit, not null)
        public string Description { get; set; } //(varchar(200), null)
        public int OrganizationId { get; set; } //(int, not null)
        public Nullable<int> Parentid { get; set; } //(int, null)
    }
    public class Role
    {
        public int Id { get; set; } //(int, not null)
        public string Name { get; set; } //(varchar(50), not null)
        public int AccessLevel { get; set; } //(int, not null)
        public string Description { get; set; } //(varchar(200), null)
        public bool Admin { get; set; } //(bit, not null)
        public bool URLS { get; set; } //(bit, not null)
        public bool ScreenShot { get; set; } //(bit, not null)
        public bool LiveStream { get; set; } //(bit, not null)
        public int OrganizationId { get; set; } //(int, not null)
    }
    public class RoleModel
    {
        public string Name { get; set; }
    }
    public class ResultModel
    {
        public string AccountName { get; set; }
        public int Result { get; set; }
        public string Msg { get; set; }
        public string BillNo { get; set; }
        public Int32 StockNo { get; set; }
        public DateTime SaleTime { get; set; }

        public Int32 Id { get; set; }

    }
    public class UserAttendanceModel
    {
        public int OrganizationId { get; set; } //(int, not null)
        public int Id { get; set; } //(int, not null)
        public int UserId { get; set; } //(int, not null)
        public DateTime AttendanceDate { get; set; } //(datetime, not null)

        public Nullable<DateTime> Start_Time { get; set; } //(datetime,  null)
        public Nullable<DateTime> End_Time { get; set; } //(datetime,  null)
        public Nullable<DateTime> Total_Time { get; set; } //(datetime,  null)
        public Nullable<DateTime> Late_Time { get; set; } //(datetime,  null)
        public int Status { get; set; } //(int, not null)
    }
    public class UserBreakModel
    {
        public int OrganizationId { get; set; } //(int, not null)
        public int BreakEntryId { get; set; } //(int, not null)

        public int Id { get; set; } //(int, not null)
        public int UserId { get; set; } //(int, not null)

        public DateTime BreakDate { get; set; } //(datetime, not null)

        public DateTime Start_Time { get; set; } //(datetime, not null)
        public Nullable<DateTime> End_Time { get; set; } //(datetime, not null)
        public int Status { get; set; } //(int, not null)
    }
    public class UserScreenShotModel
    {
        public int OrganizationId { get; set; } //(int, not null)
        public int Id { get; set; } //(int, not null)
        public int UserId { get; set; } //(int, not null)
        public string ScreenShotDate { get; set; } //(datetime, not null)

        public DateTime ServerDate { get; set; } //(datetime, not null)
        public DateTime Created_date { get; set; } //(datetime, not null)


        public string FileName { get; set; } //(datetime, not null)
        public string FilePath { get; set; } //(datetime, not null)
    }
    public class GetAttendanceModel
    {
        public int OrganizationId { get; set; } //(int, not null)
        public int UserId { get; set; } //(int, not null)
        public int TeamId { get; set; } //(int, not null)
        public DateTime CurrentDate { get; set; } //(datetime, not null)
        public DateTime FromDate { get; set; } //(datetime, not null)
        public DateTime ToDate { get; set; } //(datetime, not null)
        public int Type { get; set; } //(int, not null)
    }
    public class ViewAttendanceModel
    {
        public decimal AttendancePer { get; set; } //(int, not null)
        public string LAttendancePer { get; set; } //(int, not null)

        public decimal LatePer { get; set; } //(int, not null)
        public string LLatePer { get; set; } //(int, not null)

        public string BreakPer { get; set; } //(int, not null)
        public string LBreakPer { get; set; } //(int, not null)

        public string Working { get; set; } //(int, not null)
        public string LWorking { get; set; } //(int, not null)

        public Todaychatmodel Todaychat { get; set; } //(int, not null)

        public List<Attendancechatmodel> Attendancechat { get; set; } //(int, not null)
        public List<Latechatmodel> Latechat { get; set; } //(int, not null)
        public List<Breakchatmodel> Breakchat { get; set; } //(int, not null)
    }
    public class Attendancechatmodel
    {
        public DateTime Date { get; set; } //(datetime, not null)
        public int Present { get; set; } //(int, not null)
        public int Absent { get; set; } //(int, not null)
    }
    public class Todaychatmodel
    {
        public int Present { get; set; } //(int, not null)
        public int Absent { get; set; } //(int, not null)
        public int Total { get; set; } //(int, not null)
        public string PresentPer { get; set; } //(int, not null)
        public string AbsentPer { get; set; } //(int, not null)
    }
    public class Latechatmodel
    {
        public DateTime Date { get; set; } //(datetime, not null)
        public int Present { get; set; } //(int, not null)
        public int Absent { get; set; } //(int, not null)
    }
    public class Breakchatmodel
    {
        public DateTime Date { get; set; } //(datetime, not null)
        public int Present { get; set; } //(int, not null)
        public int Absent { get; set; } //(int, not null)
    }
    public class ResponseViewModel
    {
        public bool status { get; set; }
        public int status_code { get; set; }
        public int error_code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public int optional { get; set; }
    }
    public class MontinoutModel
    {
        public string date { get; set; }
        public string punch_in { get; set; }
        public string punch_out { get; set; }
        public string punch_duration { get; set; }
        public string workplace { get; set; }
        public string att_status { get; set; }
    }
    public class UserAttendanceDetailModel
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public bool Active { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public DateTime Total_Time { get; set; }
        public DateTime Late_Time { get; set; }
        public int Status { get; set; }
    }
    public class AttendanceRecord
    {
        public string First_Name { get; set; }
        public string Email { get; set; }
        public string EmployeeID { get; set; }
        public bool Active { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public DateTime Total_Time { get; set; }
        public DateTime Late_Time { get; set; }
        public string Status { get; set; }
    }
    public class  UserBreakRecordModel
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string BreakType { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public DateTime BreakDate { get; set; }    
    }

    public class UserScreenShotModels
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime ScreenShotDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] ImageData { get; set; }
    }


}