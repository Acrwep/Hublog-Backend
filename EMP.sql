USE [master]
GO
/****** Object:  Database [EMP]    Script Date: 04-16-2024 9:30:37 AM ******/
CREATE DATABASE [EMP]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EMP', FILENAME = N'D:\LOGNET\EMP.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'EMP_log', FILENAME = N'D:\LOGNET\EMP_log.ldf' , SIZE = 2304KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [EMP] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EMP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EMP] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EMP] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EMP] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EMP] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EMP] SET ARITHABORT OFF 
GO
ALTER DATABASE [EMP] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EMP] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EMP] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EMP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EMP] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EMP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EMP] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EMP] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EMP] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EMP] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EMP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EMP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EMP] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EMP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EMP] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EMP] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EMP] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EMP] SET RECOVERY FULL 
GO
ALTER DATABASE [EMP] SET  MULTI_USER 
GO
ALTER DATABASE [EMP] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EMP] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EMP] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EMP] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [EMP] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EMP] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EMP] SET QUERY_STORE = OFF
GO
USE [EMP]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[UserId] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[AttendanceDate] [datetime] NOT NULL,
	[Start_Time] [datetime] NULL,
	[End_Time] [datetime] NULL,
	[Total_Time] [datetime] NULL,
	[Late_Time] [datetime] NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BreakEntry]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BreakEntry](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[UserId] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[Start_Time] [datetime] NOT NULL,
	[End_Time] [datetime] NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_BreakEntry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BreakMaster]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BreakMaster](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Max_Break_Time] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[OrganizationId] [int] NOT NULL,
 CONSTRAINT [PK_BreakMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Designation]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designation](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Active] [bit] NOT NULL,
	[Description] [varchar](200) NULL,
	[Created_date] [datetime] NOT NULL,
	[OrganizationId] [int] NOT NULL,
 CONSTRAINT [PK_Designation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organization]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Organization_Name] [varchar](100) NOT NULL,
	[Contact_Person] [varchar](100) NOT NULL,
	[Street] [varchar](100) NULL,
	[Country] [varchar](100) NULL,
	[Zip_Code] [varchar](100) NULL,
	[State] [varchar](100) NULL,
	[City] [varchar](100) NULL,
	[Phone_Number] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[AccessLevel] [int] NOT NULL,
	[Description] [varchar](200) NULL,
	[Admin] [bit] NOT NULL,
	[URLS] [bit] NOT NULL,
	[ScreenShot] [bit] NOT NULL,
	[LiveStream] [bit] NOT NULL,
	[OrganizationId] [int] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Active] [bit] NOT NULL,
	[Description] [varchar](200) NULL,
	[OrganizationId] [int] NOT NULL,
	[Parentid] [int] NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[First_Name] [varchar](100) NOT NULL,
	[Last_Name] [varchar](100) NULL,
	[Email] [varchar](100) NOT NULL,
	[DOB] [date] NULL,
	[DOJ] [date] NULL,
	[Phone] [varchar](100) NOT NULL,
	[UsersName] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Gender] [varchar](100) NULL,
	[OrganizationId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[DesignationId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[EmployeeID] [varchar](100) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Designation] ADD  CONSTRAINT [DF_Designation_Created_date]  DEFAULT (getdate()) FOR [Created_date]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_Admin]  DEFAULT ((0)) FOR [Admin]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_URLS]  DEFAULT ((0)) FOR [URLS]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_ScreenShot]  DEFAULT ((0)) FOR [ScreenShot]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_LiveStream]  DEFAULT ((0)) FOR [LiveStream]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Designation] FOREIGN KEY([DesignationId])
REFERENCES [dbo].[Designation] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Designation]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Organization] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Organization]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Role]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_Team] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Team]
GO
/****** Object:  StoredProcedure [dbo].[SP_Attendance]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SP_Attendance]
	@details nvarchar(max)
AS
BEGIN
    declare	@Result int=0;
	declare @msg varchar(100)='';
 BEGIN TRY	
	 BEGIN TRANSACTION	

	   IF OBJECT_ID('tempdb..#temp') IS NOT NULL    drop TABLE tempdb..#temp
		 SELECT * 
         INTO #temp
         FROM 
(SELECT ROW_NUMBER() OVER(ORDER BY Id) AS tempid,Id,UserId,AttendanceDate,OrganizationId,Start_Time,End_Time,Total_Time,Late_Time,Status
        FROM OPENJSON(@details)
        WITH (	
		Id int '$.Id',			
		UserId int '$.UserId',
		OrganizationId int '$.OrganizationId',
		
		AttendanceDate  datetime '$.AttendanceDate',
		Start_Time datetime '$.Start_Time',
		End_Time datetime '$.End_Time',
		Total_Time datetime '$.Total_Time',
		Late_Time datetime '$.Late_Time',
		Status int '$.Status'
        )) AS x

declare	@Id int
declare	@tempid int
declare	@UserId int
declare	@OrganizationId int
declare @count int
declare	@AttendanceDate datetime
declare	@Start_Time datetime
declare	@End_Time datetime
declare	@Total_Time datetime
declare	@Late_Time datetime 
declare	@Status int


		WHILE EXISTS (SELECT * FROM #Temp)
  BEGIN
    SELECT TOP 1 
@Id=Id,
@UserId=UserId,
@OrganizationId=OrganizationId,
@AttendanceDate=AttendanceDate,
@Start_Time=Start_Time,
@End_Time=End_Time,
@Total_Time=Total_Time,
@Late_Time=Late_Time,
@Status=Status,
@tempid=tempid
from #Temp

if(@OrganizationId!='')
BEGIN
set @count=(select count(*) from Organization where Id=@OrganizationId)
if(@count=0)
BEGIN
		  set @msg='Organisation Not Found'+@OrganizationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END
if(@Start_Time='')
BEGIN
set @Start_Time=null;
END
if(@End_Time='')
BEGIN
set @End_Time=null;
END
if(@Total_Time='')
BEGIN
set @Total_Time=null;
END
if(@Late_Time='')
BEGIN
set @Late_Time=null;
END
if(@Id=0)
   BEGIN
     insert into Attendance(UserId,OrganizationId,AttendanceDate,Start_Time,End_Time,Total_Time,Late_Time,Status)
	 values(@UserId,@OrganizationId,@AttendanceDate,@Start_Time,@End_Time,@Total_Time,@Late_Time,@Status)
	 set @Id = (select Scope_identity() AS IdNew)	
   END
ELSE
   BEGIN
    update Attendance set
	UserId= @UserId,
	OrganizationId=@OrganizationId,
	AttendanceDate=@AttendanceDate,
	Start_Time = @Start_Time,
	End_Time=@End_Time,
	Total_Time=@Total_Time,
	Late_Time=@Late_Time,
Status=@Status
	where Id=@Id
   END


   	  delete from #Temp where tempid=@tempid
END
	 COMMIT TRAN -- Transaction Success!
	 SET @Result =1 
	select @Result as Result,@msg as Msg,@Id as Id;
	END TRY
	BEGIN CATCH		
		IF @@TRANCOUNT > 0
					DECLARE @Message varchar(MAX) = ERROR_MESSAGE();
			ROLLBACK TRAN --RollBack in case of Error
		SET @Result =0 
		select @Result as Result,@Message as Msg,@Id as Id;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BreakEntry]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_BreakEntry]
	@details nvarchar(max)
AS
BEGIN
    declare	@Result int=0;
	declare @msg varchar(100)='';
 BEGIN TRY	
	 BEGIN TRANSACTION	

	   IF OBJECT_ID('tempdb..#temp') IS NOT NULL    drop TABLE tempdb..#temp
		 SELECT * 
         INTO #temp
         FROM 
(SELECT ROW_NUMBER() OVER(ORDER BY Id) AS tempid,Id,OrganizationId,UserId,Start_Time,End_Time,Status
        FROM OPENJSON(@details)
        WITH (	
		Id int '$.Id',			
		UserId int '$.UserId',
		OrganizationId int '$.OrganizationId',
		Start_Time datetime '$.Start_Time',
		End_Time datetime '$.End_Time',
		Status int '$.Status'
        )) AS x

declare	@Id int
declare	@tempid int
declare	@UserId int
declare	@OrganizationId int
declare @count int
declare	@Start_Time datetime
declare	@End_Time datetime 
declare	@Status int


		WHILE EXISTS (SELECT * FROM #Temp)
  BEGIN
    SELECT TOP 1 
@Id=Id,
@UserId=UserId,
@OrganizationId=OrganizationId,
@Start_Time=Start_Time,
@End_Time=End_Time,
@Status=Status,
@tempid=tempid
from #Temp

if(@OrganizationId!='')
BEGIN
set @count=(select count(*) from Organization where Id=@OrganizationId)
if(@count=0)
BEGIN
		  set @msg='Organisation Not Found'+@OrganizationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END

if(@End_Time='')
BEGIN
set @End_Time=null;
END
if(@Id=0)
   BEGIN
     insert into BreakEntry(UserId,OrganizationId,Start_Time,End_Time,Status)
	 values(@UserId,@OrganizationId,@Start_Time,@End_Time,@Status)
	 set @Id = (select Scope_identity() AS IdNew)	
   END
ELSE
   BEGIN
    update BreakEntry set
	UserId= @UserId,
	OrganizationId=@OrganizationId,
	Start_Time = @Start_Time,
	End_Time=@End_Time,
Status=@Status
	where Id=@Id
   END


   	  delete from #Temp where tempid=@tempid
END
	 COMMIT TRAN -- Transaction Success!
	 SET @Result =1 
	select @Result as Result,@msg as Msg,@Id as Id;
	END TRY
	BEGIN CATCH		
		IF @@TRANCOUNT > 0
					DECLARE @Message varchar(MAX) = ERROR_MESSAGE();
			ROLLBACK TRAN --RollBack in case of Error
		SET @Result =0 
		select @Result as Result,@Message as Msg,@Id as Id;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertBreakMaster]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_InsertBreakMaster]
	@details nvarchar(max)
AS
BEGIN
    declare	@Result int=0;
	declare @msg varchar(100)='';
 BEGIN TRY	
	 BEGIN TRANSACTION	

	   IF OBJECT_ID('tempdb..#temp') IS NOT NULL    drop TABLE tempdb..#temp
		 SELECT * 
         INTO #temp
         FROM 
(SELECT ROW_NUMBER() OVER(ORDER BY Id) AS tempid,Id,Name,Max_Break_Time,OrganizationId,Active
        FROM OPENJSON(@details)
        WITH (	
		Id int '$.Id',			
		Name varchar(100) '$.Name',
		Max_Break_Time int '$.Max_Break_Time',
		OrganizationId int '$.OrganizationId',
		Active int '$.Active'
        )) AS x

declare	@Id int
declare	@tempid int
declare	@Name varchar(100)
declare	@Max_Break_Time int
declare	@OrganizationId int
declare	@Active int
declare @count int


		WHILE EXISTS (SELECT * FROM #Temp)
  BEGIN
    SELECT TOP 1 
@Id=Id,
@Name=Name,
@Max_Break_Time=Max_Break_Time,
@OrganizationId=OrganizationId,
@Active=Active,
@tempid=tempid
from #Temp


if(@OrganizationId!='')
BEGIN
set @count=(select count(*) from Organization where Id=@OrganizationId)
if(@count=0)
BEGIN
		  set @msg='Organisation Not Found'+@OrganizationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END




if(@Id=0)
   BEGIN
   INSERT INTO [dbo].[BreakMaster]
           ([Name]
           ,[Active]
           ,[Max_Break_Time]
           ,[OrganizationId])
     VALUES
           (@Name
           ,@Active
           ,@Max_Break_Time
           ,@OrganizationId)
	 set @Id = (select Scope_identity() AS IdNew)	
   END
ELSE
   BEGIN
    update BreakMaster set
	[Name]= @Name,
	[Max_Break_Time]= @Max_Break_Time,
	[Active]=@Active
	where Id=@Id and OrganizationId=@OrganizationId
   END


   	  delete from #Temp where tempid=@tempid
END
	 COMMIT TRAN -- Transaction Success!
	 SET @Result =1 
	select @Result as Result,@msg as Msg,@Id as Id;
	END TRY
	BEGIN CATCH		
		IF @@TRANCOUNT > 0
					DECLARE @Message varchar(MAX) = ERROR_MESSAGE();
			ROLLBACK TRAN --RollBack in case of Error
		SET @Result =0 
		select @Result as Result,@Message as Msg,@Id as Id;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertDesignation]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_InsertDesignation]
	@details nvarchar(max)
AS
BEGIN
    declare	@Result int=0;
	declare @msg varchar(100)='';
 BEGIN TRY	
	 BEGIN TRANSACTION	

	   IF OBJECT_ID('tempdb..#temp') IS NOT NULL    drop TABLE tempdb..#temp
		 SELECT * 
         INTO #temp
         FROM 
(SELECT ROW_NUMBER() OVER(ORDER BY Id) AS tempid,Id,Name,Description,OrganizationId,Active
        FROM OPENJSON(@details)
        WITH (	
		Id int '$.Id',			
		Name varchar(100) '$.Name',
		Description varchar(100) '$.Description',
		OrganizationId int '$.OrganizationId',
		Active int '$.Active'
        )) AS x

declare	@Id int
declare	@tempid int
declare	@Name varchar(100)
declare	@Description varchar(100)
declare	@OrganizationId int
declare	@Active int
declare @count int


		WHILE EXISTS (SELECT * FROM #Temp)
  BEGIN
    SELECT TOP 1 
@Id=Id,
@Name=Name,
@Description=Description,
@OrganizationId=OrganizationId,
@Active=Active,
@tempid=tempid
from #Temp


if(@OrganizationId!='')
BEGIN
set @count=(select count(*) from Organization where Id=@OrganizationId)
if(@count=0)
BEGIN
		  set @msg='Organisation Not Found'+@OrganizationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END




if(@Id=0)
   BEGIN
   INSERT INTO [dbo].[Designation]
           ([Name]
           ,[Active]
           ,[Description]
           ,[Created_date]
           ,[OrganizationId])
     VALUES
           (@Name
           ,@Active
           ,@Description
           ,GETDATE()
           ,@OrganizationId)
	 set @Id = (select Scope_identity() AS IdNew)	
   END
ELSE
   BEGIN
    update Designation set
	[Name]= @Name,
	[Description]= @Description,
	[Active]=@Active
	where Id=@Id and OrganizationId=@OrganizationId
   END


   	  delete from #Temp where tempid=@tempid
END
	 COMMIT TRAN -- Transaction Success!
	 SET @Result =1 
	select @Result as Result,@msg as Msg,@Id as Id;
	END TRY
	BEGIN CATCH		
		IF @@TRANCOUNT > 0
					DECLARE @Message varchar(MAX) = ERROR_MESSAGE();
			ROLLBACK TRAN --RollBack in case of Error
		SET @Result =0 
		select @Result as Result,@Message as Msg,@Id as Id;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertRole]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_InsertRole]
	@details nvarchar(max)
AS
BEGIN
    declare	@Result int=0;
	declare @msg varchar(100)='';
 BEGIN TRY	
	 BEGIN TRANSACTION	

	   IF OBJECT_ID('tempdb..#temp') IS NOT NULL    drop TABLE tempdb..#temp
		 SELECT * 
         INTO #temp
         FROM 
(SELECT ROW_NUMBER() OVER(ORDER BY Id) AS tempid,Id,Name,Description,AccessLevel,Admin,URLS,ScreenShot,LiveStream,OrganizationId
        FROM OPENJSON(@details)
        WITH (	
		Id int '$.Id',			
		Name varchar(100) '$.Name',
		Description varchar(200) '$.Description',
		AccessLevel int '$.AccessLevel',
		Admin int '$.Admin',
		URLS int '$.URLS',
		ScreenShot int '$.ScreenShot',
		LiveStream int '$.LiveStream',
		OrganizationId int '$.OrganizationId'
        )) AS x

declare	@Id int
declare	@tempid int
declare	@Name varchar(100)
declare	@Description varchar(200)
declare	@AccessLevel int
declare	@Admin int
declare	@URLS int
declare	@ScreenShot int
declare	@LiveStream int
declare	@OrganizationId int
declare @count int


		WHILE EXISTS (SELECT * FROM #Temp)
  BEGIN
    SELECT TOP 1 
@Id=Id,
@Name=Name,
@Description=Description,
@AccessLevel=AccessLevel,
@Admin=Admin,
@URLS=URLS,
@ScreenShot=ScreenShot,
@LiveStream=LiveStream,
@OrganizationId=OrganizationId,
@tempid=tempid
from #Temp


if(@OrganizationId!='')
BEGIN
set @count=(select count(*) from Organization where Id=@OrganizationId)
if(@count=0)
BEGIN
		  set @msg='Organisation Not Found'+@OrganizationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END




if(@Id=0)
   BEGIN
INSERT INTO [dbo].[Role]
           ([Name]
           ,[AccessLevel]
           ,[Description]
           ,[Admin]
           ,[URLS]
           ,[ScreenShot]
           ,[LiveStream]
           ,[OrganizationId])
     VALUES
           (@Name
           ,@AccessLevel
           ,@Description
           ,@Admin
           ,@URLS
           ,@ScreenShot
           ,@LiveStream
           ,@OrganizationId)
	 set @Id = (select Scope_identity() AS IdNew)	
   END
ELSE
   BEGIN
    update Role set
	[Name]= @Name,
	[AccessLevel]=@AccessLevel,
	[Description]=@Description,
	[Admin]=@Admin,
	[URLS]= @URLS,
	[ScreenShot]=@ScreenShot,
	[LiveStream]= @LiveStream
	where Id=@Id and OrganizationId=@OrganizationId
   END


   	  delete from #Temp where tempid=@tempid
END
	 COMMIT TRAN -- Transaction Success!
	 SET @Result =1 
	select @Result as Result,@msg as Msg,@Id as Id;
	END TRY
	BEGIN CATCH		
		IF @@TRANCOUNT > 0
					DECLARE @Message varchar(MAX) = ERROR_MESSAGE();
			ROLLBACK TRAN --RollBack in case of Error
		SET @Result =0 
		select @Result as Result,@Message as Msg,@Id as Id;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertTeams]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_InsertTeams]
	@details nvarchar(max)
AS
BEGIN
    declare	@Result int=0;
	declare @msg varchar(100)='';
 BEGIN TRY	
	 BEGIN TRANSACTION	

	   IF OBJECT_ID('tempdb..#temp') IS NOT NULL    drop TABLE tempdb..#temp
		 SELECT * 
         INTO #temp
         FROM 
(SELECT ROW_NUMBER() OVER(ORDER BY Id) AS tempid,Id,Name,Description,OrganizationId,Active,Parentid
        FROM OPENJSON(@details)
        WITH (	
		Id int '$.Id',			
		Name varchar(100) '$.Name',
		Description varchar(100) '$.Description',
		OrganizationId int '$.OrganizationId',
		Active int '$.Active',
		Parentid int '$.Parentid'
        )) AS x

declare	@Id int
declare	@tempid int
declare	@Name varchar(100)
declare	@Description varchar(100)
declare	@OrganizationId int
declare	@Active int
declare	@Parentid int

declare @count int


		WHILE EXISTS (SELECT * FROM #Temp)
  BEGIN
    SELECT TOP 1 
@Id=Id,
@Name=Name,
@Description=Description,
@OrganizationId=OrganizationId,
@Active=Active,
@Parentid=Parentid,
@tempid=tempid
from #Temp

if(@Parentid=0)
BEGIN
set @Parentid=null;
END

if(@OrganizationId!='')
BEGIN
set @count=(select count(*) from Organization where Id=@OrganizationId)
if(@count=0)
BEGIN
		  set @msg='Organisation Not Found'+@OrganizationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END




if(@Id=0)
   BEGIN
   INSERT INTO [dbo].[Team]
           ([Name]
           ,[Active]
           ,[Description]
           ,[OrganizationId]
		   ,[Parentid]
		   )
     VALUES
           (@Name
           ,@Active
           ,@Description
           ,@OrganizationId,
		   @Parentid)
	 set @Id = (select Scope_identity() AS IdNew)	
   END
ELSE
   BEGIN
    update Team set
	[Name]= @Name,
	[Description]= @Description,
	[Active]=@Active,
	[Parentid]=@Parentid
	where Id=@Id and OrganizationId=@OrganizationId
   END


   	  delete from #Temp where tempid=@tempid
END
	 COMMIT TRAN -- Transaction Success!
	 SET @Result =1 
	select @Result as Result,@msg as Msg,@Id as Id;
	END TRY
	BEGIN CATCH		
		IF @@TRANCOUNT > 0
					DECLARE @Message varchar(MAX) = ERROR_MESSAGE();
			ROLLBACK TRAN --RollBack in case of Error
		SET @Result =0 
		select @Result as Result,@Message as Msg,@Id as Id;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertUsers]    Script Date: 04-16-2024 9:30:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SP_InsertUsers]
	@details nvarchar(max)
AS
BEGIN
    declare	@Result int=0;
	declare @msg varchar(100)='';
 BEGIN TRY	
	 BEGIN TRANSACTION	

	   IF OBJECT_ID('tempdb..#temp') IS NOT NULL    drop TABLE tempdb..#temp
		 SELECT * 
         INTO #temp
         FROM 
(SELECT ROW_NUMBER() OVER(ORDER BY Id) AS tempid,Id,First_Name,Last_Name,Email,DOB,DOJ,Phone,UsersName,Password,Gender,EmployeeID,OrganizationId,RoleId,DesignationId,TeamId,Active
        FROM OPENJSON(@details)
        WITH (	
		Id int '$.Id',			
		First_Name varchar(100) '$.First_Name',
		Last_Name varchar(100) '$.Last_Name',
		Email varchar(100) '$.Email',
		DOB datetime '$.DOB',
		DOJ datetime '$.DOJ',
		Phone varchar(100) '$.Phone',
		UsersName varchar(100) '$.UsersName',
		Password varchar(100) '$.Password',
		Gender varchar(100) '$.Gender',
		EmployeeID varchar(100) '$.EmployeeID',
		OrganizationId int '$.OrganizationId',
		RoleId int '$.RoleId',
		DesignationId int '$.DesignationId',
		TeamId int '$.TeamId',
		Active int '$.Active'
        )) AS x

declare	@Id int
declare	@tempid int
declare	@First_Name varchar(100)
declare	@Last_Name varchar(100)
declare	@Email varchar(100)
declare	@DOB datetime 
declare	@DOJ datetime 
declare	@Phone varchar(100)
declare	@UsersName varchar(100)
declare	@Password varchar(100)
declare	@Gender varchar(100)
declare	@EmployeeID varchar(100)
declare	@OrganizationId int
declare	@RoleId int
declare	@DesignationId int
declare	@TeamId int
declare	@Active int
declare	@Status int
declare @count int


		WHILE EXISTS (SELECT * FROM #Temp)
  BEGIN
    SELECT TOP 1 
@Id=Id,
@First_Name=First_Name,
@Last_Name=Last_Name,
@Email=Email,
@DOB=DOB,
@DOJ=DOJ,
@Phone=Phone,
@UsersName=UsersName,
@Password=Password,
@Gender=Gender,
@EmployeeID=EmployeeID,
@OrganizationId=OrganizationId,
@RoleId=RoleId,
@DesignationId=DesignationId,
@TeamId=TeamId,
@Active=Active
from #Temp

if @DOB=''
BEGIN
set @DOB=null;
END

if @DOJ=''
BEGIN
set @DOJ=null;
END

if(@Id=0 and @Email!='')
BEGIN
set @count=(select count(*) from users where Email=@Email and Active=1 and OrganizationId=@OrganizationId)
if(@count!=0)
BEGIN
		  set @msg='Email Already Exists'+@Email
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END

if(@OrganizationId!='')
BEGIN
set @count=(select count(*) from Organization where Id=@OrganizationId)
if(@count=0)
BEGIN
		  set @msg='Organisation Not Found'+@OrganizationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END

if(@DesignationId!='')
BEGIN
set @count=(select count(*) from Designation where Id=@DesignationId and Active=1)
if(@count=0)
BEGIN
		  set @msg='Designation Not Found'+@DesignationId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END

if(@RoleId!='')
BEGIN
set @count=(select count(*) from Role where Id=@RoleId)
if(@count=0)
BEGIN
		  set @msg='Role Not Found'+@RoleId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END

if(@TeamId!='')
BEGIN
set @count=(select count(*) from Team where Id=@TeamId and Active=1)
if(@count=0)
BEGIN
		  set @msg='Team Not Found'+@TeamId
		     ROLLBACK TRAN --RollBack in case of Error
		      SET @Result =0 
		      select @Result as Result,@msg as msg,@Id as id;
			  RETURN 
END
END



if(@Id=0)
   BEGIN
   INSERT INTO [dbo].[Users]
           ([First_Name]
           ,[Last_Name]
           ,[Email]
           ,[DOB]
           ,[DOJ]
           ,[Phone]
           ,[UsersName]
           ,[Password]
           ,[Gender]
		   ,[EmployeeID]
           ,[OrganizationId]
           ,[RoleId]
           ,[DesignationId]
           ,[TeamId]
           ,[Active])
     VALUES
           (@First_Name
           ,@Last_Name
           ,@Email
           ,@DOB
           ,@DOJ
           ,@Phone
           ,@UsersName
           ,@Password
           ,@Gender
		   ,@EmployeeID
           ,@OrganizationId
           ,@RoleId
           ,@DesignationId
           ,@TeamId
           ,@Active)
	 set @Id = (select Scope_identity() AS IdNew)	
   END
ELSE
   BEGIN
    update Users set
	[First_Name]= @First_Name,
	[Last_Name]= @Last_Name,
	[DOB]= @DOB,
	[DOJ]= @DOJ,
	[Phone]= @Phone,
	[Gender]= @Gender,
	[EmployeeID]= @EmployeeID,
	[RoleId]= @RoleId,
	[DesignationId]= @DesignationId,
	[TeamId]= @TeamId,
	[Active]=@Active
	where Id=@Id
   END


   	  delete from #Temp where tempid=@tempid
END
	 COMMIT TRAN -- Transaction Success!
	 SET @Result =1 
	select @Result as Result,@msg as Msg,@Id as Id;
	END TRY
	BEGIN CATCH		
		IF @@TRANCOUNT > 0
					DECLARE @Message varchar(MAX) = ERROR_MESSAGE();
			ROLLBACK TRAN --RollBack in case of Error
		SET @Result =0 
		select @Result as Result,@Message as Msg,@Id as Id;
	END CATCH
END
GO
USE [master]
GO
ALTER DATABASE [EMP] SET  READ_WRITE 
GO
