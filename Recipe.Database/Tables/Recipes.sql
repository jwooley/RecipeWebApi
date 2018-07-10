CREATE TABLE [dbo].[Recipes] (
    [RecipeId]        BIGINT          NOT NULL,
    [Title]           NVARCHAR (1024)  NULL,
    [ServingQuantity] DECIMAL (18, 2) NULL,
    [ServingMeasure]  NVARCHAR (50)  NULL,
    CONSTRAINT [PK_dbo.Recipes] PRIMARY KEY CLUSTERED ([RecipeId] ASC),
);


GO

CREATE INDEX [IX_Recipes_Title] ON [dbo].[Recipes] ([Title])
