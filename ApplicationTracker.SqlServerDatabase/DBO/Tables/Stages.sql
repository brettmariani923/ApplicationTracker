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
    ('NO_RESPONSE',         'No response',          2),
    ('PHONE_SCREEN',        'Phone Screen',         3),
    ('TECHNICAL_INTERVIEW', 'Technical Interview',  4),
    ('ON_SITE',             'On-site',              5),
    ('OFFER',               'Offer',                6),
    ('ACCEPTED',            'Accepted',             7),
    ('REJECTED_OFFER',      'Rejected Offer',       8);
