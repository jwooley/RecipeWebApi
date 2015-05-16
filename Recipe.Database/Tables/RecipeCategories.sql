CREATE TABLE [dbo].[RecipeCategories] (
    [Recipe_RecipeId]     BIGINT NOT NULL,
    [Category_CategoryId] BIGINT NOT NULL,
    CONSTRAINT [PK_dbo.RecipeCategories] PRIMARY KEY CLUSTERED ([Recipe_RecipeId] ASC, [Category_CategoryId] ASC),
    CONSTRAINT [FK_dbo.RecipeCategories_dbo.Categories_Category_CategoryId] FOREIGN KEY ([Category_CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.RecipeCategories_dbo.Recipes_Recipe_RecipeId] FOREIGN KEY ([Recipe_RecipeId]) REFERENCES [dbo].[Recipes] ([RecipeId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Recipe_RecipeId]
    ON [dbo].[RecipeCategories]([Recipe_RecipeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Category_CategoryId]
    ON [dbo].[RecipeCategories]([Category_CategoryId] ASC);

