CREATE TABLE [dbo].[Directions] (
    [DirectionId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [LineNumber]      BIGINT         NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [Recipe_RecipeId] BIGINT         NULL,
    CONSTRAINT [PK_dbo.Directions] PRIMARY KEY NONCLUSTERED ([DirectionId] ASC),
	CONSTRAINT [FK_dbo.Directions_dbo.Recipes_Recipe_RecipeId] FOREIGN KEY ([Recipe_RecipeId]) REFERENCES [dbo].[Recipes] ([RecipeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Recipe_RecipeId]
    ON [dbo].[Directions]([Recipe_RecipeId] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_Directions_5_709577566__K4_K1_2_3]
    ON [dbo].[Directions]([Recipe_RecipeId] ASC, [DirectionId] ASC)
    INCLUDE([LineNumber], [Description]);


GO
CREATE STATISTICS [_dta_stat_709577566_2]
    ON [dbo].[Directions]([LineNumber]);


GO
CREATE STATISTICS [_dta_stat_709577566_4_2]
    ON [dbo].[Directions]([Recipe_RecipeId], [LineNumber]);


GO

CREATE CLUSTERED INDEX [IX_Directions_RecipeLineNumber] ON [dbo].[Directions] ([Recipe_RecipeId] , LineNumber)
