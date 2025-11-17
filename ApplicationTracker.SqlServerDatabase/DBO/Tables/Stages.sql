CREATE TABLE dbo.Stages
(
    StageId INT IDENTITY(1,1) PRIMARY KEY,
    StageKey NVARCHAR(50) NOT NULL UNIQUE,
    DisplayName NVARCHAR(100) NOT NULL,
    SortOrder INT NOT NULL
);

INSERT INTO dbo.Stages (StageKey, DisplayName, SortOrder)
VALUES
    ('APPLIED',             'Applied',              1),
    ('PHONE_SCREEN',        'Phone Screen',         2),
    ('TECHNICAL_INTERVIEW', 'Technical Interview',  3),
    ('ON_SITE',             'On-site',              4),
    ('OFFER',               'Offer',                5),
    ('ACCEPTED',            'Accepted',             6),
    ('REJECTED_OFFER',      'Rejected Offer',       7),
    ('NO_RESPONSE',         'No response',          8);
