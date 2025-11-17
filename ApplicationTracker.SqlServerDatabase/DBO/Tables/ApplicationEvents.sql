CREATE TABLE [dbo].[ApplicationEvents]
(
  EventId       BIGINT IDENTITY(1,1) PRIMARY KEY,
  ApplicationId INT NOT NULL,
  StageId       INT NOT NULL,
  CONSTRAINT FK_ApplicationEvents_Applications
      FOREIGN KEY (ApplicationId)
      REFERENCES dbo.Applications(ApplicationId)
      ON DELETE CASCADE,
  CONSTRAINT FK_ApplicationEvents_Stages
      FOREIGN KEY (StageId)
      REFERENCES dbo.Stages(StageId)
);
