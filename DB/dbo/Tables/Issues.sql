CREATE TABLE [dbo].[Issues] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Name]            VARCHAR (30)  NOT NULL,
    [Description]     VARCHAR (300) NOT NULL,
    [DeadLine]        DATETIME      NULL,
    [IssueStatus]     INT           NULL,
    [AssignedTo]      INT           NULL,
    [CreatedBy]       INT           NULL,
    [CreatedDate]     DATETIME      NOT NULL,
    [LastUpdatedDate] DATETIME      NOT NULL,
    [ClosedBy]        INT           NULL,
    [ClosedOn]        DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ClosedBy]) REFERENCES [dbo].[Employee] ([Id]),
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Employee] ([Id]),
    FOREIGN KEY ([IssueStatus]) REFERENCES [dbo].[IssueStatus] ([Id])
);

