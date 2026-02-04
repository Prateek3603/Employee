USE [master]
GO
/****** Object:  Database [Task_Mangement_System]    Script Date: 2/4/2026 7:36:23 PM ******/
CREATE DATABASE [Task_Mangement_System]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Task_Mangement_System', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\Task_Mangement_System.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Task_Mangement_System_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\DATA\Task_Mangement_System_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Task_Mangement_System].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Task_Mangement_System] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET ARITHABORT OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Task_Mangement_System] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Task_Mangement_System] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Task_Mangement_System] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Task_Mangement_System] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET RECOVERY FULL 
GO
ALTER DATABASE [Task_Mangement_System] SET  MULTI_USER 
GO
ALTER DATABASE [Task_Mangement_System] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Task_Mangement_System] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Task_Mangement_System] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Task_Mangement_System] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Task_Mangement_System] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Task_Mangement_System] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Task_Mangement_System] SET QUERY_STORE = ON
GO
ALTER DATABASE [Task_Mangement_System] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Task_Mangement_System]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[AttendanceDate] [date] NULL,
	[Status] [nvarchar](10) NULL,
	[Remarks] [nvarchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[AttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[Department] [nvarchar](50) NULL,
	[JoiningDate] [date] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[EmployeeId] [int] NULL,
	[DueDate] [date] NULL,
	[Status] [nvarchar](20) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Tasks] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  StoredProcedure [dbo].[spAttendance_Delete]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAttendance_Delete]
    @AttendanceId INT
AS
BEGIN
    DELETE FROM Attendance
    WHERE AttendanceId=@AttendanceId;
END
GO
/****** Object:  StoredProcedure [dbo].[spAttendance_GetAll]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[spAttendance_GetAll]
AS
BEGIN
    SELECT 
        a.AttendanceId,
        a.EmployeeId,
        e.Name AS EmployeeName,
        a.AttendanceDate,
        a.Status,
        a.Remarks
    FROM Attendance a
    JOIN Employees e ON a.EmployeeId = e.EmployeeId;
END
GO
/****** Object:  StoredProcedure [dbo].[spAttendance_GetByDate]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spAttendance_GetByDate]
    @AttendanceDate DATE
AS
BEGIN
    SELECT 
        a.AttendanceId,
        a.EmployeeId,
        e.Name AS EmployeeName,
        a.AttendanceDate,
        a.Status,
        a.Remarks
    FROM Attendance a
    JOIN Employees e ON a.EmployeeId = e.EmployeeId
    WHERE a.AttendanceDate = @AttendanceDate;
END
GO
/****** Object:  StoredProcedure [dbo].[spAttendance_GetById]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[spAttendance_GetById]
    @AttendanceId INT
AS
BEGIN
    SELECT 
        a.AttendanceId,
        a.EmployeeId,
        e.Name AS EmployeeName,
        a.AttendanceDate,
        a.Status,
        a.Remarks
    FROM Attendance a
    JOIN Employees e ON a.EmployeeId = e.EmployeeId
    WHERE a.AttendanceId = @AttendanceId;
END
GO
/****** Object:  StoredProcedure [dbo].[spAttendance_Insert]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAttendance_Insert]
    @EmployeeId INT,
    @AttendanceDate DATE,
    @Status NVARCHAR(10),
    @Remarks NVARCHAR(250)
AS
BEGIN
    INSERT INTO Attendance
    VALUES (@EmployeeId,@AttendanceDate,@Status,@Remarks);
END
GO
/****** Object:  StoredProcedure [dbo].[spAttendance_Update]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAttendance_Update]
    @AttendanceId INT,
    @Status NVARCHAR(10),
    @Remarks NVARCHAR(250)
AS
BEGIN
    UPDATE Attendance
    SET Status=@Status,
        Remarks=@Remarks
    WHERE AttendanceId=@AttendanceId;
END
GO
/****** Object:  StoredProcedure [dbo].[spAttendance_Upsert]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAttendance_Upsert]
    @EmployeeId INT,
    @AttendanceDate DATE,
    @Status NVARCHAR(10),
    @Remarks NVARCHAR(250)
AS
BEGIN
    -- update if record exists
    UPDATE Attendance
    SET Status = @Status,
        Remarks = @Remarks
    WHERE EmployeeId = @EmployeeId
      AND AttendanceDate = @AttendanceDate;

    -- if no row updated, then insert
    IF @@ROWCOUNT = 0
    BEGIN
        INSERT INTO Attendance (EmployeeId, AttendanceDate, Status, Remarks)
        VALUES (@EmployeeId, @AttendanceDate, @Status, @Remarks);
    END
END
GO
/****** Object:  StoredProcedure [dbo].[spEmployees_GetAll]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spEmployees_GetAll]
AS
BEGIN
    SELECT *
    FROM Employees
    WHERE IsDeleted = 0;
END
GO
/****** Object:  StoredProcedure [dbo].[spEmployees_GetById]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployees_GetById]
    @EmployeeId INT
AS
BEGIN
    SELECT *
    FROM Employees
    WHERE EmployeeId = @EmployeeId AND IsDeleted = 0;
END
GO
/****** Object:  StoredProcedure [dbo].[spEmployees_Insert]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployees_Insert]
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Department NVARCHAR(50),
    @JoiningDate DATE
AS
BEGIN
    INSERT INTO Employees
    VALUES (@Name,@Email,@Phone,@Department,@JoiningDate,0);

    SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spEmployees_SoftDelete]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployees_SoftDelete]
    @EmployeeId INT
AS
BEGIN
    UPDATE Employees
    SET IsDeleted = 1
    WHERE EmployeeId = @EmployeeId;
END
GO
/****** Object:  StoredProcedure [dbo].[spEmployees_Update]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spEmployees_Update]
    @EmployeeId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Department NVARCHAR(50),
    @JoiningDate DATE
AS
BEGIN
    UPDATE Employees
    SET Name=@Name,
        Email=@Email,
        Phone=@Phone,
        Department=@Department,
        JoiningDate=@JoiningDate
    WHERE EmployeeId=@EmployeeId;
END
GO
/****** Object:  StoredProcedure [dbo].[spTasks_GetAll]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[spTasks_GetAll]
AS
BEGIN
    SELECT 
        t.TaskId,
        t.Title,
        t.Description,
        t.EmployeeId,
        e.Name AS EmployeeName,
        t.DueDate,
        t.Status
    FROM Tasks t
    JOIN Employees e ON t.EmployeeId = e.EmployeeId
    WHERE t.IsDeleted = 0;
END
GO
/****** Object:  StoredProcedure [dbo].[spTasks_GetById]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spTasks_GetById]
    @TaskId INT
AS
BEGIN
    SELECT *
    FROM Tasks
    WHERE TaskId=@TaskId AND IsDeleted=0;
END
GO
/****** Object:  StoredProcedure [dbo].[spTasks_Insert]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTasks_Insert]
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @EmployeeId INT,
    @DueDate DATE,
    @Status NVARCHAR(20)
AS
BEGIN
    INSERT INTO Tasks
    VALUES (@Title,@Description,@EmployeeId,@DueDate,@Status,0);

    SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[spTasks_SoftDelete]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spTasks_SoftDelete]
    @TaskId INT
AS
BEGIN
    UPDATE Tasks
    SET IsDeleted=1
    WHERE TaskId=@TaskId;
END
GO
/****** Object:  StoredProcedure [dbo].[spTasks_Update]    Script Date: 2/4/2026 7:36:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spTasks_Update]
    @TaskId INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @EmployeeId INT,
    @DueDate DATE,
    @Status NVARCHAR(20)
AS
BEGIN
    UPDATE Tasks
    SET Title=@Title,
        Description=@Description,
        EmployeeId=@EmployeeId,
        DueDate=@DueDate,
        Status=@Status
    WHERE TaskId=@TaskId;
END
GO
USE [master]
GO
ALTER DATABASE [Task_Mangement_System] SET  READ_WRITE 
GO
