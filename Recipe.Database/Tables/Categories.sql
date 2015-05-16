CREATE TABLE [dbo].[Categories] (
    [CategoryId]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (50) NULL,
    CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

