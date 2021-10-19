USE [ZmgTestDb]
GO
SET IDENTITY_INSERT [dbo].[roles] ON 
GO
INSERT [dbo].[roles] ([role_id], [role_name]) VALUES (CAST(1 AS Numeric(18, 0)), N'Editor')
GO
INSERT [dbo].[roles] ([role_id], [role_name]) VALUES (CAST(2 AS Numeric(18, 0)), N'Writer')
GO
INSERT [dbo].[roles] ([role_id], [role_name]) VALUES (CAST(3 AS Numeric(18, 0)), N'Viewer')
GO
SET IDENTITY_INSERT [dbo].[roles] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 
GO
INSERT [dbo].[users] ([user_id], [name], [username], [password], [email]) VALUES (CAST(1 AS Numeric(18, 0)), N'Hire Me', N'HireMe', N'dunno', N'hire@me.please')
GO
INSERT [dbo].[users] ([user_id], [name], [username], [password], [email]) VALUES (CAST(2 AS Numeric(18, 0)), N'Love Zemoga', N'LoveZemoga', N'dunno', N'love@zmg.com')
GO
INSERT [dbo].[users] ([user_id], [name], [username], [password], [email]) VALUES (CAST(3 AS Numeric(18, 0)), N'Viewer User', N'Viewer', N'dunno', N'viewer@user.co')
GO
SET IDENTITY_INSERT [dbo].[users] OFF
GO
SET IDENTITY_INSERT [dbo].[users_roles] ON 
GO
INSERT [dbo].[users_roles] ([users_roles_id], [user_id], [role_id]) VALUES (CAST(1 AS Numeric(18, 0)), CAST(1 AS Numeric(18, 0)), CAST(1 AS Numeric(18, 0)))
GO
INSERT [dbo].[users_roles] ([users_roles_id], [user_id], [role_id]) VALUES (CAST(2 AS Numeric(18, 0)), CAST(2 AS Numeric(18, 0)), CAST(2 AS Numeric(18, 0)))
GO
INSERT [dbo].[users_roles] ([users_roles_id], [user_id], [role_id]) VALUES (CAST(3 AS Numeric(18, 0)), CAST(3 AS Numeric(18, 0)), CAST(3 AS Numeric(18, 0)))
GO
SET IDENTITY_INSERT [dbo].[users_roles] OFF
GO
