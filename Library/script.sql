USE [master]
GO
/****** Object:  Database [library]    Script Date: 7/6/2023 1:51:17 AM ******/
CREATE DATABASE [library]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'library_db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\library_db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'library_db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\library_db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [library] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [library].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [library] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [library] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [library] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [library] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [library] SET ARITHABORT OFF 
GO
ALTER DATABASE [library] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [library] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [library] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [library] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [library] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [library] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [library] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [library] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [library] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [library] SET  DISABLE_BROKER 
GO
ALTER DATABASE [library] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [library] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [library] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [library] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [library] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [library] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [library] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [library] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [library] SET  MULTI_USER 
GO
ALTER DATABASE [library] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [library] SET DB_CHAINING OFF 
GO
ALTER DATABASE [library] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [library] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [library] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [library] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [library] SET QUERY_STORE = OFF
GO
USE [library]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 7/6/2023 1:51:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](500) NULL,
	[AuthorName] [nvarchar](500) NULL,
	[GenereId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BooksLoans]    Script Date: 7/6/2023 1:51:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BooksLoans](
	[Id] [uniqueidentifier] NOT NULL,
	[BookId] [uniqueidentifier] NOT NULL,
	[SubscriberId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BooksLoans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Geners]    Script Date: 7/6/2023 1:51:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Geners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[Description] [nvarchar](1500) NULL,
 CONSTRAINT [PK_Geners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscribers]    Script Date: 7/6/2023 1:51:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscribers](
	[Id] [uniqueidentifier] NOT NULL,
	[Firstname] [nvarchar](400) NULL,
	[Lastname] [nvarchar](400) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK_Subscribers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'21b5b8a2-926d-4714-9672-11ba0ac45f5c', N'Nineteen Eighty-Four', N'Nineteen ', 1006, CAST(N'2023-07-01T16:30:21.987' AS DateTime), CAST(N'2023-07-01T16:30:21.987' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'ff1ca72f-b1b6-4551-8d42-246e1e0e9527', N'The Adventures of Huckleberry Finn', N'Huckleberry', 6, CAST(N'2023-07-01T16:28:54.573' AS DateTime), CAST(N'2023-07-01T16:28:54.573' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'7b55b885-6596-4320-bb87-337c7eeea845', N'Nineteen Eighty-Four Sesson 2', N'Eighty', 7, CAST(N'2023-07-01T16:31:07.757' AS DateTime), CAST(N'2023-07-01T16:31:07.757' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'6f0e196a-38e5-4d3f-b792-34ffc7671d44', N'To Kill a Mockingbird', N'Indra', 1006, CAST(N'2023-07-01T16:21:00.143' AS DateTime), CAST(N'2023-07-01T16:30:01.050' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'f5610d8f-d05f-472a-b110-4dcb48502a3b', N'Wuthering Heights', N'Wuthering', 1007, CAST(N'2023-07-01T16:30:35.747' AS DateTime), CAST(N'2023-07-01T16:30:35.747' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'4fb4e8e0-56cf-4f9e-86b6-5cb510712f15', N'Don Quixote', N'Quixote', 7, CAST(N'2023-07-01T16:30:53.523' AS DateTime), CAST(N'2023-07-01T16:30:53.523' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'68350735-bac2-4c6e-9fb7-8ddc26dc5aa7', N'The Real Star 2', N'Jhone', 7, CAST(N'2023-07-01T16:19:37.397' AS DateTime), CAST(N'2023-07-01T16:19:48.220' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'9f51d450-21c1-4ff2-840f-9cde7a2f2ae9', N'Anna Karenina', N'Karenina', 1007, CAST(N'2023-07-01T16:20:36.300' AS DateTime), CAST(N'2023-07-01T16:29:53.180' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'de1d65c3-24d7-46f1-ad64-b1d3431ba68d', N'Jane Eyre', N'Eyre', 7, CAST(N'2023-07-01T16:20:50.557' AS DateTime), CAST(N'2023-07-01T16:29:57.657' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'd4a030fe-70b0-488a-a300-b29f4591f122', N'Nineteen Eighty-Four Sesson', N'Sesson ', 7, CAST(N'2023-07-01T16:34:41.183' AS DateTime), CAST(N'2023-07-01T16:35:11.463' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'c4178a54-097d-406e-9685-da4724344b28', N'Real adventure', N'Mortin', 1006, CAST(N'2023-07-01T16:20:21.423' AS DateTime), CAST(N'2023-07-01T16:30:04.637' AS DateTime))
INSERT [dbo].[Books] ([Id], [Title], [AuthorName], [GenereId], [CreatedAt], [UpdatedAt]) VALUES (N'196a60ac-6fbb-4818-bdae-eb497fa27f2f', N'Pride and Prejudice', N'Pride ', 7, CAST(N'2023-07-01T16:20:44.353' AS DateTime), CAST(N'2023-07-01T16:28:12.543' AS DateTime))
GO
INSERT [dbo].[BooksLoans] ([Id], [BookId], [SubscriberId]) VALUES (N'dc392662-f30c-4a4e-9fe7-1f06edc1ed67', N'7b55b885-6596-4320-bb87-337c7eeea845', N'75a4a008-123f-458d-9ce7-fc1e0503d710')
INSERT [dbo].[BooksLoans] ([Id], [BookId], [SubscriberId]) VALUES (N'291ab804-fb4e-4d5e-9dab-40583d93714a', N'ff1ca72f-b1b6-4551-8d42-246e1e0e9527', N'1ab2f85d-9e9d-482b-935c-2a06e00a7067')
INSERT [dbo].[BooksLoans] ([Id], [BookId], [SubscriberId]) VALUES (N'b152e1a0-e833-4a05-b533-65acd89c1c3d', N'21b5b8a2-926d-4714-9672-11ba0ac45f5c', N'75a4a008-123f-458d-9ce7-fc1e0503d710')
INSERT [dbo].[BooksLoans] ([Id], [BookId], [SubscriberId]) VALUES (N'b7b31765-17b4-49d4-91d5-8277c52a5d27', N'21b5b8a2-926d-4714-9672-11ba0ac45f5c', N'1d36076c-4e54-4c23-911d-a3f998cfbdc8')
INSERT [dbo].[BooksLoans] ([Id], [BookId], [SubscriberId]) VALUES (N'aa01e4b0-72c9-4e24-bb1c-a0c47ed9b73d', N'21b5b8a2-926d-4714-9672-11ba0ac45f5c', N'1ab2f85d-9e9d-482b-935c-2a06e00a7067')
INSERT [dbo].[BooksLoans] ([Id], [BookId], [SubscriberId]) VALUES (N'7737bfe4-511f-43d6-ae7c-ba25818ad58f', N'7b55b885-6596-4320-bb87-337c7eeea845', N'1d36076c-4e54-4c23-911d-a3f998cfbdc8')
GO
SET IDENTITY_INSERT [dbo].[Geners] ON 

INSERT [dbo].[Geners] ([Id], [Name], [Description]) VALUES (6, N'Adventure', N'This is a adventure genere')
INSERT [dbo].[Geners] ([Id], [Name], [Description]) VALUES (7, N'Story', N'This is story genere')
INSERT [dbo].[Geners] ([Id], [Name], [Description]) VALUES (1006, N'Fiction', N'This is Fiction genere')
INSERT [dbo].[Geners] ([Id], [Name], [Description]) VALUES (1007, N'Novel', N'This is Novel genere')
INSERT [dbo].[Geners] ([Id], [Name], [Description]) VALUES (1008, N'Historical', N'This is a historical genere')
SET IDENTITY_INSERT [dbo].[Geners] OFF
GO
INSERT [dbo].[Subscribers] ([Id], [Firstname], [Lastname], [CreatedAt], [UpdatedAt]) VALUES (N'1ab2f85d-9e9d-482b-935c-2a06e00a7067', N'Subs', N'rider', CAST(N'2023-07-02T09:10:16.973' AS DateTime), CAST(N'2023-07-02T14:18:27.203' AS DateTime))
INSERT [dbo].[Subscribers] ([Id], [Firstname], [Lastname], [CreatedAt], [UpdatedAt]) VALUES (N'1d36076c-4e54-4c23-911d-a3f998cfbdc8', N'Jone', N'Smith', CAST(N'2023-07-03T18:08:49.670' AS DateTime), CAST(N'2023-07-03T18:08:59.040' AS DateTime))
INSERT [dbo].[Subscribers] ([Id], [Firstname], [Lastname], [CreatedAt], [UpdatedAt]) VALUES (N'75a4a008-123f-458d-9ce7-fc1e0503d710', N'df', N'sfd', CAST(N'2023-07-03T18:00:40.233' AS DateTime), CAST(N'2023-07-03T18:00:44.253' AS DateTime))
GO
ALTER TABLE [dbo].[Books] ADD  CONSTRAINT [DF_Books_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[BooksLoans] ADD  CONSTRAINT [DF_BooksLoans_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Subscribers] ADD  CONSTRAINT [DF_Subscribers_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_Books] FOREIGN KEY([GenereId])
REFERENCES [dbo].[Geners] ([Id])
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_Books]
GO
ALTER TABLE [dbo].[BooksLoans]  WITH CHECK ADD  CONSTRAINT [FK_BooksSubscribers_Books] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
GO
ALTER TABLE [dbo].[BooksLoans] CHECK CONSTRAINT [FK_BooksSubscribers_Books]
GO
ALTER TABLE [dbo].[BooksLoans]  WITH CHECK ADD  CONSTRAINT [FK_BooksSubscribers_Subscribers] FOREIGN KEY([SubscriberId])
REFERENCES [dbo].[Subscribers] ([Id])
GO
ALTER TABLE [dbo].[BooksLoans] CHECK CONSTRAINT [FK_BooksSubscribers_Subscribers]
GO
USE [master]
GO
ALTER DATABASE [library] SET  READ_WRITE 
GO
