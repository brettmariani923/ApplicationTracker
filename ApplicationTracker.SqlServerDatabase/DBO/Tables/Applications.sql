CREATE TABLE [dbo].[Applications]
(
  ApplicationId INT IDENTITY(1,1) PRIMARY KEY,
  CompanyName   NVARCHAR(200) NOT NULL,
  JobTitle      NVARCHAR(200) NULL,
  ResumeId      NVARCHAR(50)  NULL,
  CreatedUtc    DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
);