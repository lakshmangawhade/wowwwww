-- Run against your database. Adjust column types/names if your existing tables differ.
-- Existing tables assumed: TerritoryMaster, DealerMaster, UserDetails

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PFAOnboardingRequests')
BEGIN
    CREATE TABLE dbo.PFAOnboardingRequests
    (
        RequestId              BIGINT IDENTITY(1,1) NOT NULL,
        Name                   NVARCHAR(200)        NOT NULL,
        Mobile                 NVARCHAR(15)         NOT NULL,
        EmailId                NVARCHAR(256)        NOT NULL,
        PanNo                  NVARCHAR(10)         NOT NULL,
        AadhaarNumber          NVARCHAR(12)         NOT NULL,
        UanNumber              NVARCHAR(12)         NULL,
        TerritoryId            INT                  NOT NULL,
        UsedExistingUserDetails BIT                 NOT NULL CONSTRAINT DF_PFAOnboardingRequests_UsedExisting DEFAULT (0),
        UserDetailsId          INT                  NULL,
        CreatedAtUtc           DATETIME2(3)         NOT NULL CONSTRAINT DF_PFAOnboardingRequests_CreatedAt DEFAULT (SYSUTCDATETIME()),
        Status                 NVARCHAR(20)         NOT NULL CONSTRAINT DF_PFAOnboardingRequests_Status DEFAULT ('Pending'),

        CONSTRAINT PK_PFAOnboardingRequests PRIMARY KEY CLUSTERED (RequestId),
        CONSTRAINT FK_PFAOnboardingRequests_Territory
            FOREIGN KEY (TerritoryId) REFERENCES dbo.TerritoryMaster (TerritoryId),
        CONSTRAINT FK_PFAOnboardingRequests_UserDetails
            FOREIGN KEY (UserDetailsId) REFERENCES dbo.UserDetails (UserId)
    );

    CREATE NONCLUSTERED INDEX IX_PFAOnboardingRequests_Mobile
        ON dbo.PFAOnboardingRequests (Mobile);

    CREATE NONCLUSTERED INDEX IX_PFAOnboardingRequests_TerritoryId
        ON dbo.PFAOnboardingRequests (TerritoryId);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PFAOnboardingRequestDistributors')
BEGIN
    CREATE TABLE dbo.PFAOnboardingRequestDistributors
    (
        Id            BIGINT IDENTITY(1,1) NOT NULL,
        RequestId     BIGINT               NOT NULL,
        DealerId      INT                  NOT NULL,
        CreatedAtUtc  DATETIME2(3)         NOT NULL CONSTRAINT DF_PFAOnboardingRequestDistributors_CreatedAt DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_PFAOnboardingRequestDistributors PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT FK_PFAOnboardingRequestDistributors_Request
            FOREIGN KEY (RequestId) REFERENCES dbo.PFAOnboardingRequests (RequestId) ON DELETE CASCADE,
        CONSTRAINT FK_PFAOnboardingRequestDistributors_Dealer
            FOREIGN KEY (DealerId) REFERENCES dbo.DealerMaster (DealerId),
        CONSTRAINT UQ_PFAOnboardingRequestDistributors_Request_Dealer
            UNIQUE (RequestId, DealerId)
    );

    CREATE NONCLUSTERED INDEX IX_PFAOnboardingRequestDistributors_RequestId
        ON dbo.PFAOnboardingRequestDistributors (RequestId);
END
GO
