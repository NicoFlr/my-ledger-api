USE [MyLedgerDB]
GO
INSERT [dbo].[Category] ([Id], [Name]) VALUES (N'85ab9918-007b-4804-aeef-0e7f99d54a4c', N'Transport')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (N'0beb373e-c5dc-4420-b0e0-152c70b90661', N'Food')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (N'799b89f8-bf63-48ae-9221-2252ce4b54cb', N'Work')
GO
INSERT [dbo].[Transaction] ([Id], [Money], [DateTime], [IsBill], [CategoryId]) VALUES (N'ce41629a-bd68-48d5-8238-3989f6b6e3c8', CAST(55.0000 AS Decimal(19, 4)), CAST(N'2025-01-20T10:04:52.2870000-04:00' AS DateTimeOffset), 0, N'799b89f8-bf63-48ae-9221-2252ce4b54cb')
INSERT [dbo].[Transaction] ([Id], [Money], [DateTime], [IsBill], [CategoryId]) VALUES (N'10c15097-d8d5-4c2d-a544-c163fc785b86', CAST(0.5000 AS Decimal(19, 4)), CAST(N'2025-01-22T11:36:41.4680000-04:00' AS DateTimeOffset), 1, N'85ab9918-007b-4804-aeef-0e7f99d54a4c')
GO
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'b7a7c3d9-755e-4018-921e-1da37fa173ec', N'User')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2', N'Administrator')
GO
INSERT [dbo].[User] ([Id], [FirstName], [LastName], [Email], [Password], [RoleId], [IsDeleted]) VALUES (N'42581618-6f25-45d9-8d5b-40af9be6278b', N'Julio Nicolas', N'Flores Rojas', N'nico_fr1110@hotmail.com', N'774D22A764BF3D71122A0739CCD99E6D6F79C335D96D19C7127614E9C8FDDE83FB239268F30F1BB751D00EAAFF7886AC37F0004397100DA79EB19415A1B4C419|281BB3D14B86445D6980B9E0D24B56E037E3BB485FF3C553E3E7EF5039CDFED2EFFE054881D8E30126DEF5B4B5AAB100ECE6AD417F4F44AFDA609CC8476FD931', N'e0b5d470-e47e-4c7e-aaa8-b7c8cbebc4a2', 0)
GO
INSERT [dbo].[UserTransactions] ([UserId], [TransactionId]) VALUES (N'42581618-6f25-45d9-8d5b-40af9be6278b', N'ce41629a-bd68-48d5-8238-3989f6b6e3c8')
INSERT [dbo].[UserTransactions] ([UserId], [TransactionId]) VALUES (N'42581618-6f25-45d9-8d5b-40af9be6278b', N'10c15097-d8d5-4c2d-a544-c163fc785b86')
GO
