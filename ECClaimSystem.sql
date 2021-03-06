USE [ECClaimSystem]
GO
/****** Object:  StoredProcedure [dbo].[sp_ECClaim_GetAllECClaimsOfFaculty]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Thanh
-- Create date: 6/4/2017
-- Description:	Get All Ec claims of faculty
-- =============================================
CREATE PROCEDURE [dbo].[sp_ECClaim_GetAllECClaimsOfFaculty]
@FacultyId int
AS
BEGIN
	set transaction isolation level read uncommitted;
	select ec.*
	from ECClaim ec (nolock) inner join [User] u (nolock) on u.UserId = ec.UserId
	where u.FacultyId = @FacultyId and u.Active = 1 and ec.Active = 1
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetECClaimOfFaculty]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetECClaimOfFaculty]
@FacultyId int
AS
BEGIN
	set transaction isolation level read uncommitted;
	select ec.*
	from ECClaim ec (nolock) inner join [User] u (nolock) on ec.UserId = u.UserId
	where u.FacultyId = @FacultyId
END

GO
/****** Object:  Table [dbo].[ECClaim]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ECClaim](
	[ClaimId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[Summary] [nvarchar](200) NULL,
	[Situation] [nvarchar](max) NOT NULL,
	[EffectSituation] [nvarchar](max) NOT NULL,
	[CircumstanceStartDate] [datetime] NOT NULL,
	[CircumstanceEndDate] [datetime] NOT NULL,
	[OutCome] [int] NULL,
	[SubmittedDate] [datetime] NULL,
	[FinalClosureDate] [datetime] NULL,
	[ClaimStatus] [int] NULL,
	[DecisionStatus] [int] NULL,
	[DecisionDate] [datetime] NULL,
	[ProcessedUser] [bigint] NULL,
	[HasEvidence] [bit] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK__ECClaim__EF2E139B4E5F0342] PRIMARY KEY CLUSTERED 
(
	[ClaimId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ECEvidence]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ECEvidence](
	[EvidenceId] [int] IDENTITY(1,1) NOT NULL,
	[EvidenceName] [nvarchar](max) NOT NULL,
	[ClaimId] [bigint] NOT NULL,
	[Type] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK__ECEviden__FA39D7ADDF6FB112] PRIMARY KEY CLUSTERED 
(
	[EvidenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Faculty]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faculty](
	[FacultyId] [int] IDENTITY(1,1) NOT NULL,
	[FacultyCode] [nvarchar](100) NOT NULL,
	[FacultyName] [nvarchar](200) NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK__Faculty__306F630EFCBA9117] PRIMARY KEY CLUSTERED 
(
	[FacultyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK__Role__8AFACE1A5936F514] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Setting]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](200) NOT NULL,
	[Value] [nvarchar](200) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK__Setting__3214EC071479F22B] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 4/9/2017 11:10:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountId] [nvarchar](200) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[UserFullName] [nvarchar](200) NOT NULL,
	[BirthDay] [datetime] NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Gender] [int] NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[FacultyId] [int] NULL,
	[RoleId] [int] NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK__User__1788CC4CB2ED46C8] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ECClaim]  WITH CHECK ADD  CONSTRAINT [FK__ECClaim__UserId__239E4DCF] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ECClaim] CHECK CONSTRAINT [FK__ECClaim__UserId__239E4DCF]
GO
ALTER TABLE [dbo].[ECEvidence]  WITH CHECK ADD  CONSTRAINT [FK__ECEvidenc__Claim__267ABA7A] FOREIGN KEY([ClaimId])
REFERENCES [dbo].[ECClaim] ([ClaimId])
GO
ALTER TABLE [dbo].[ECEvidence] CHECK CONSTRAINT [FK__ECEvidenc__Claim__267ABA7A]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK__User__RoleId__1920BF5C] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK__User__RoleId__1920BF5C]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Faculty] FOREIGN KEY([FacultyId])
REFERENCES [dbo].[Faculty] ([FacultyId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Faculty]
GO
