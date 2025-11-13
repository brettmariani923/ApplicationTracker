CREATE TABLE [dbo].[ApplicationEvents]
(
  EventId       BIGINT IDENTITY(1,1) PRIMARY KEY,
  ApplicationId INT NOT NULL FOREIGN KEY REFERENCES Applications(ApplicationId) ON DELETE CASCADE,
  -- "ON DELETE CASCADE" is so that if a record in the parent table is deleted, then all the related records in the child table will also be deleted
  StageId       INT NOT NULL  FOREIGN KEY REFERENCES Stages(StageId),
);
CREATE TABLE [dbo].[Applications]
(
  ApplicationId INT IDENTITY(1,1) PRIMARY KEY,
  CompanyName   NVARCHAR(200) NOT NULL,
  JobTitle      NVARCHAR(200) NULL,
);

CREATE TABLE [dbo].[Stages]
(
  StageId     INT IDENTITY(1,1) PRIMARY KEY,
  StageKey    VARCHAR(40) UNIQUE NOT NULL,  -- APPLIED, INT1, INT2, INT3_5, OFFER, HIRED, REJECTED, NO_RESPONSE
  DisplayName VARCHAR(100) NOT NULL,
  SortOrder   INT NOT NULL
);