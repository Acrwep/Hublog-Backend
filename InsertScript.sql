Declare @oid int;
Declare @did int;
Declare @tid int;
Declare @ridS int;
Declare @ridA int;
Declare @ridE int;
INSERT INTO [dbo].[Organization]
           ([Organization_Name]
           ,[Contact_Person]
           ,[Street]
           ,[Country]
           ,[Zip_Code]
           ,[State]
           ,[City]
           ,[Phone_Number])
     VALUES
           ('Hublog'
           ,'admin'
           ,''
           ,''
           ,''
           ,''
           ,''
           ,'1234567890')
set @oid = (select Scope_identity() AS IdNew)	
INSERT INTO [dbo].[Designation]
           ([Name]
           ,[Active]
           ,[Description]
		   ,[OrganizationId]
           ,[Created_date])
     VALUES
           ('OPERATION'
           ,1
           ,''
		   ,@oid
           ,GETDATE())
set @did = (select Scope_identity() AS IdNew)	
INSERT INTO [dbo].[Team]
           ([Name]
           ,[Active]
		   ,[OrganizationId]
           ,[Description])
     VALUES
           ('OPERATION'
           ,1
		   ,@oid
           ,'')
set @tid = (select Scope_identity() AS IdNew)	
INSERT INTO [dbo].[Role]
           ([Name]
           ,[AccessLevel]
           ,[Description]
		   ,[OrganizationId]
           ,[Admin]
           ,[URLS]
           ,[ScreenShot]
           ,[LiveStream])
     VALUES
           ('SUPER_ADMIN'
           ,0
           ,''
		   ,@oid
           ,1
           ,1
           ,1
           ,1)
set @ridS = (select Scope_identity() AS IdNew)	
INSERT INTO [dbo].[Role]
           ([Name]
           ,[AccessLevel]
           ,[Description]
		    ,[OrganizationId]
           ,[Admin]
           ,[URLS]
           ,[ScreenShot]
           ,[LiveStream])
     VALUES
           ('ADMIN'
           ,1
           ,''
		   ,@oid
           ,1
           ,1
           ,1
           ,1)
set @ridA = (select Scope_identity() AS IdNew)	
INSERT INTO [dbo].[Role]
           ([Name]
           ,[AccessLevel]
           ,[Description]
		    ,[OrganizationId]
           ,[Admin]
           ,[URLS]
           ,[ScreenShot]
           ,[LiveStream])
     VALUES
           ('EMPLOYEE'
           ,2
           ,''
		   ,@oid
           ,0
           ,0
           ,0
           ,0)
set @ridE = (select Scope_identity() AS IdNew)	
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
           ,[OrganizationId]
           ,[RoleId]
           ,[DesignationId]
           ,[TeamId]
           ,[Active]
           ,[EmployeeID])
     VALUES
           ('ADMIN'
           ,''
           ,'admin@Hublog.com'
           ,GETDATE()
           ,GETDATE()
           ,'1234567890'
           ,'admin'
           ,'123456'
           ,'MALE'
           ,@oid
           ,@ridA
           ,@did
           ,@tid
           ,1
           ,'')
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
           ,[OrganizationId]
           ,[RoleId]
           ,[DesignationId]
           ,[TeamId]
           ,[Active]
           ,[EmployeeID])
     VALUES
           ('VETRI'
           ,''
           ,'vetri@Hublog.com'
           ,GETDATE()
           ,GETDATE()
           ,'8667648145'
           ,'empvetri'
           ,'123456'
           ,'MALE'
           ,@oid
           ,@ridE
           ,@did
           ,@tid
           ,1
           ,'')


