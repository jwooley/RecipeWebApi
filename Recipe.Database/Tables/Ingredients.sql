CREATE TABLE [dbo].[Ingredients] (
    [IngredientId]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [SortOrder]       INT           NULL,
    [Units]           NVARCHAR (50) NULL,
    [UnitType]        NVARCHAR (50) NULL,
    [Description]     NVARCHAR (50) NULL,
    [Recipe_RecipeId] BIGINT        NULL,
    CONSTRAINT [PK_dbo.Ingredients] PRIMARY KEY CLUSTERED ([IngredientId] ASC),
    CONSTRAINT [FK_dbo.Ingredients_dbo.Recipes_Recipe_RecipeId] FOREIGN KEY ([Recipe_RecipeId]) REFERENCES [dbo].[Recipes] ([RecipeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Recipe_RecipeId]
    ON [dbo].[Ingredients]([Recipe_RecipeId] ASC);


GO
CREATE NONCLUSTERED INDEX [_dta_index_Ingredients_5_661577395__K6_K1_2_3_4_5]
    ON [dbo].[Ingredients]([Recipe_RecipeId] ASC, [IngredientId] ASC)
    INCLUDE([SortOrder], [Units], [UnitType], [Description]);


GO
CREATE STATISTICS [_dta_stat_661577395_2_1]
    ON [dbo].[Ingredients]([SortOrder], [IngredientId]);


GO
CREATE STATISTICS [_dta_stat_661577395_6_2_1]
    ON [dbo].[Ingredients]([Recipe_RecipeId], [SortOrder], [IngredientId]);

