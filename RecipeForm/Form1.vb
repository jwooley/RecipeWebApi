Imports RecipeDal
Imports Recipe.Utils

Public Class Form1
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim dc = RecipeContext.ContextFactory()
        Dim recipes = From r In dc.Recipes
                      Where r.Title.Contains(txtSearch.Text) OrElse
                         r.Ingredients.Any(Function(i) i.Description.Contains(txtSearch.Text))
                      Select r
        RecipeList.DataSource = recipes.ToList()
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles dgSearchResults.SelectionChanged
        Console.WriteLine(dgSearchResults.SelectedRows)
    End Sub

    Private Sub RecipeList_CurrentItemChanged(sender As Object, e As EventArgs) Handles RecipeList.CurrentItemChanged
        If RecipeList.Current Is Nothing Then
            Return
        End If
        Dim found = DirectCast(RecipeList.Current, RecipeDal.Recipe)

        Dim dc = RecipeContext.ContextFactory()
        Dim Recipe =
            (From r In dc.Recipes
             Where r.Id = found.Id
             Select New With
            {
                r.Title,
                 r.ServingQuantity,
                 r.ServingMeasure,
                .Category = r.Categories.Select(Function(c) c.Description),
                .Ingredients = r.Ingredients.OrderBy(Function(i) i.SortOrder).Distinct(),
                .Directions = r.Directions.OrderBy(Function(d) d.LineNumber).Select(Function(d) d.Description)
            }
                 ).FirstOrDefault()
        If Recipe Is Nothing Then
            Return
        End If

        txtName.Text = Recipe.Title
        txtQuantity.Text = Recipe.ServingQuantity.GetValueOrDefault().ToFractionString()
        txtMeasure.Text = Recipe.ServingMeasure
        txtCategories.Text = String.Join(",", Recipe.Category.ToArray())
        txtDirections.Text = String.Join(Environment.NewLine, Recipe.Directions.ToArray())
        dgIngredient.DataSource = (
            From i In Recipe.Ingredients
            Select New IngredientVm() With
                {
                    .Description = i.Description,
                    .UnitType = i.UnitType,
                    .Units = i.Units.ToFractionString()
                }).ToList()
    End Sub
End Class
