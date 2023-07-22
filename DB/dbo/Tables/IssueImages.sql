CREATE TABLE [dbo].[IssueImages] (
    [Id]        INT              IDENTITY (1, 1) NOT NULL,
    [ImagePath] VARCHAR (300)    NULL,
    [IssueId]   INT              NULL,
    [ImageGuid] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IssueId]) REFERENCES [dbo].[Issues] ([Id])
);

