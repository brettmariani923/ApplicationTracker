CREATE TABLE [dbo].[ApplicationEvents]
(
  EventId       BIGINT IDENTITY(1,1) PRIMARY KEY,
  ApplicationId INT NOT NULL FOREIGN KEY REFERENCES Applications(ApplicationId) ON DELETE CASCADE,
  -- "ON DELETE CASCADE" is so that if a record in the parent table is deleted, then all the related records in the child table will also be deleted
  StageId       INT NOT NULL  FOREIGN KEY REFERENCES Stages(StageId),
  OccurredUtc   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
);