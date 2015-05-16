CREATE TABLE [dbo].[Recipes] (
    [RecipeId]        BIGINT          NOT NULL,
    [Title]           NVARCHAR (MAX)  NULL,
    [ServingQuantity] DECIMAL (18, 2) NULL,
    [ServingMeasure]  NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Recipes] PRIMARY KEY CLUSTERED ([RecipeId] ASC)
);


GO
CREATE STATISTICS [_dta_stat_517576882_1_3]
    ON [dbo].[Recipes]([RecipeId], [ServingQuantity]);

