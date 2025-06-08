<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Label1 = New Label()
        txtSearch = New TextBox()
        btnSearch = New Button()
        dgSearchResults = New DataGridView()
        IdDataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
        TitleDataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
        RecipeList = New BindingSource(components)
        SplitContainer1 = New SplitContainer()
        SplitContainer2 = New SplitContainer()
        dgIngredient = New DataGridView()
        UnitsDataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
        UnitTypeDataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
        DescriptionDataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
        bsIngredient = New BindingSource(components)
        Label6 = New Label()
        txtDirections = New TextBox()
        Label7 = New Label()
        txtCategories = New TextBox()
        Label5 = New Label()
        txtMeasure = New TextBox()
        Label4 = New Label()
        txtQuantity = New TextBox()
        Label3 = New Label()
        txtName = New TextBox()
        Label2 = New Label()
        CType(dgSearchResults, ComponentModel.ISupportInitialize).BeginInit()
        CType(RecipeList, ComponentModel.ISupportInitialize).BeginInit()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer2.Panel1.SuspendLayout()
        SplitContainer2.Panel2.SuspendLayout()
        SplitContainer2.SuspendLayout()
        CType(dgIngredient, ComponentModel.ISupportInitialize).BeginInit()
        CType(bsIngredient, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(14, 17)
        Label1.Margin = New Padding(2, 0, 2, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(53, 20)
        Label1.TabIndex = 0
        Label1.Text = "Search"
        ' 
        ' txtSearch
        ' 
        txtSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtSearch.Location = New Point(78, 12)
        txtSearch.Margin = New Padding(2, 3, 2, 3)
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(1284, 27)
        txtSearch.TabIndex = 1
        ' 
        ' btnSearch
        ' 
        btnSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSearch.Location = New Point(1368, 9)
        btnSearch.Margin = New Padding(2, 3, 2, 3)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(33, 32)
        btnSearch.TabIndex = 2
        btnSearch.Text = "Button1"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' dgSearchResults
        ' 
        dgSearchResults.AllowUserToAddRows = False
        dgSearchResults.AllowUserToDeleteRows = False
        dgSearchResults.AutoGenerateColumns = False
        dgSearchResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgSearchResults.ColumnHeadersVisible = False
        dgSearchResults.Columns.AddRange(New DataGridViewColumn() {IdDataGridViewTextBoxColumn, TitleDataGridViewTextBoxColumn})
        dgSearchResults.DataSource = RecipeList
        dgSearchResults.Dock = DockStyle.Fill
        dgSearchResults.Location = New Point(0, 0)
        dgSearchResults.Margin = New Padding(2, 3, 2, 3)
        dgSearchResults.Name = "dgSearchResults"
        dgSearchResults.ReadOnly = True
        dgSearchResults.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgSearchResults.RowHeadersVisible = False
        dgSearchResults.RowHeadersWidth = 51
        dgSearchResults.RowTemplate.Height = 24
        dgSearchResults.Size = New Size(464, 723)
        dgSearchResults.TabIndex = 3
        ' 
        ' IdDataGridViewTextBoxColumn
        ' 
        IdDataGridViewTextBoxColumn.DataPropertyName = "Id"
        IdDataGridViewTextBoxColumn.HeaderText = "Id"
        IdDataGridViewTextBoxColumn.MinimumWidth = 6
        IdDataGridViewTextBoxColumn.Name = "IdDataGridViewTextBoxColumn"
        IdDataGridViewTextBoxColumn.ReadOnly = True
        IdDataGridViewTextBoxColumn.Visible = False
        IdDataGridViewTextBoxColumn.Width = 125
        ' 
        ' TitleDataGridViewTextBoxColumn
        ' 
        TitleDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        TitleDataGridViewTextBoxColumn.DataPropertyName = "Title"
        TitleDataGridViewTextBoxColumn.HeaderText = "Title"
        TitleDataGridViewTextBoxColumn.MinimumWidth = 6
        TitleDataGridViewTextBoxColumn.Name = "TitleDataGridViewTextBoxColumn"
        TitleDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' RecipeList
        ' 
        RecipeList.DataSource = GetType(RecipeDal.Recipe)
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        SplitContainer1.Location = New Point(2, 61)
        SplitContainer1.Margin = New Padding(2, 3, 2, 3)
        SplitContainer1.Name = "SplitContainer1"
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(dgSearchResults)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(SplitContainer2)
        SplitContainer1.Panel2.Controls.Add(txtCategories)
        SplitContainer1.Panel2.Controls.Add(Label5)
        SplitContainer1.Panel2.Controls.Add(txtMeasure)
        SplitContainer1.Panel2.Controls.Add(Label4)
        SplitContainer1.Panel2.Controls.Add(txtQuantity)
        SplitContainer1.Panel2.Controls.Add(Label3)
        SplitContainer1.Panel2.Controls.Add(txtName)
        SplitContainer1.Panel2.Controls.Add(Label2)
        SplitContainer1.Size = New Size(1399, 723)
        SplitContainer1.SplitterDistance = 464
        SplitContainer1.SplitterWidth = 5
        SplitContainer1.TabIndex = 4
        ' 
        ' SplitContainer2
        ' 
        SplitContainer2.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        SplitContainer2.Location = New Point(2, 140)
        SplitContainer2.Margin = New Padding(2, 3, 2, 3)
        SplitContainer2.Name = "SplitContainer2"
        ' 
        ' SplitContainer2.Panel1
        ' 
        SplitContainer2.Panel1.Controls.Add(dgIngredient)
        SplitContainer2.Panel1.Controls.Add(Label6)
        ' 
        ' SplitContainer2.Panel2
        ' 
        SplitContainer2.Panel2.Controls.Add(txtDirections)
        SplitContainer2.Panel2.Controls.Add(Label7)
        SplitContainer2.Size = New Size(929, 581)
        SplitContainer2.SplitterDistance = 380
        SplitContainer2.SplitterWidth = 5
        SplitContainer2.TabIndex = 8
        ' 
        ' dgIngredient
        ' 
        dgIngredient.AllowUserToAddRows = False
        dgIngredient.AllowUserToDeleteRows = False
        dgIngredient.AutoGenerateColumns = False
        dgIngredient.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgIngredient.ColumnHeadersVisible = False
        dgIngredient.Columns.AddRange(New DataGridViewColumn() {UnitsDataGridViewTextBoxColumn, UnitTypeDataGridViewTextBoxColumn, DescriptionDataGridViewTextBoxColumn})
        dgIngredient.DataSource = bsIngredient
        dgIngredient.Location = New Point(7, 41)
        dgIngredient.Margin = New Padding(5, 4, 5, 4)
        dgIngredient.Name = "dgIngredient"
        dgIngredient.ReadOnly = True
        dgIngredient.RowHeadersVisible = False
        dgIngredient.RowHeadersWidth = 51
        dgIngredient.ShowEditingIcon = False
        dgIngredient.Size = New Size(368, 532)
        dgIngredient.TabIndex = 11
        ' 
        ' UnitsDataGridViewTextBoxColumn
        ' 
        UnitsDataGridViewTextBoxColumn.DataPropertyName = "Units"
        UnitsDataGridViewTextBoxColumn.HeaderText = "Units"
        UnitsDataGridViewTextBoxColumn.MinimumWidth = 6
        UnitsDataGridViewTextBoxColumn.Name = "UnitsDataGridViewTextBoxColumn"
        UnitsDataGridViewTextBoxColumn.ReadOnly = True
        UnitsDataGridViewTextBoxColumn.Width = 50
        ' 
        ' UnitTypeDataGridViewTextBoxColumn
        ' 
        UnitTypeDataGridViewTextBoxColumn.DataPropertyName = "UnitType"
        UnitTypeDataGridViewTextBoxColumn.HeaderText = "UnitType"
        UnitTypeDataGridViewTextBoxColumn.MinimumWidth = 6
        UnitTypeDataGridViewTextBoxColumn.Name = "UnitTypeDataGridViewTextBoxColumn"
        UnitTypeDataGridViewTextBoxColumn.ReadOnly = True
        UnitTypeDataGridViewTextBoxColumn.Width = 50
        ' 
        ' DescriptionDataGridViewTextBoxColumn
        ' 
        DescriptionDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DescriptionDataGridViewTextBoxColumn.DataPropertyName = "Description"
        DescriptionDataGridViewTextBoxColumn.HeaderText = "Description"
        DescriptionDataGridViewTextBoxColumn.MinimumWidth = 6
        DescriptionDataGridViewTextBoxColumn.Name = "DescriptionDataGridViewTextBoxColumn"
        DescriptionDataGridViewTextBoxColumn.ReadOnly = True
        ' 
        ' bsIngredient
        ' 
        bsIngredient.AllowNew = False
        bsIngredient.DataSource = GetType(IngredientVm)
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(15, 13)
        Label6.Margin = New Padding(2, 0, 2, 0)
        Label6.Name = "Label6"
        Label6.Size = New Size(83, 20)
        Label6.TabIndex = 9
        Label6.Text = "Ingredients"
        ' 
        ' txtDirections
        ' 
        txtDirections.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtDirections.Location = New Point(7, 42)
        txtDirections.Margin = New Padding(2, 3, 2, 3)
        txtDirections.Multiline = True
        txtDirections.Name = "txtDirections"
        txtDirections.ScrollBars = ScrollBars.Both
        txtDirections.Size = New Size(536, 532)
        txtDirections.TabIndex = 11
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(14, 13)
        Label7.Margin = New Padding(2, 0, 2, 0)
        Label7.Name = "Label7"
        Label7.Size = New Size(76, 20)
        Label7.TabIndex = 10
        Label7.Text = "Directions"
        ' 
        ' txtCategories
        ' 
        txtCategories.Location = New Point(98, 91)
        txtCategories.Margin = New Padding(2, 3, 2, 3)
        txtCategories.Name = "txtCategories"
        txtCategories.Size = New Size(501, 27)
        txtCategories.TabIndex = 7
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(17, 96)
        Label5.Margin = New Padding(2, 0, 2, 0)
        Label5.Name = "Label5"
        Label5.Size = New Size(80, 20)
        Label5.TabIndex = 6
        Label5.Text = "Categories"
        ' 
        ' txtMeasure
        ' 
        txtMeasure.Location = New Point(392, 53)
        txtMeasure.Margin = New Padding(2, 3, 2, 3)
        txtMeasure.Name = "txtMeasure"
        txtMeasure.Size = New Size(207, 27)
        txtMeasure.TabIndex = 5
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(322, 60)
        Label4.Margin = New Padding(2, 0, 2, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(65, 20)
        Label4.TabIndex = 4
        Label4.Text = "Measure"
        ' 
        ' txtQuantity
        ' 
        txtQuantity.Location = New Point(98, 53)
        txtQuantity.Margin = New Padding(2, 3, 2, 3)
        txtQuantity.Name = "txtQuantity"
        txtQuantity.Size = New Size(207, 27)
        txtQuantity.TabIndex = 3
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(17, 60)
        Label3.Margin = New Padding(2, 0, 2, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(65, 20)
        Label3.TabIndex = 2
        Label3.Text = "Quantity"
        ' 
        ' txtName
        ' 
        txtName.Location = New Point(98, 21)
        txtName.Margin = New Padding(2, 3, 2, 3)
        txtName.Name = "txtName"
        txtName.Size = New Size(821, 27)
        txtName.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(17, 27)
        Label2.Margin = New Padding(2, 0, 2, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(49, 20)
        Label2.TabIndex = 0
        Label2.Text = "Name"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1407, 787)
        Controls.Add(SplitContainer1)
        Controls.Add(btnSearch)
        Controls.Add(txtSearch)
        Controls.Add(Label1)
        Margin = New Padding(2, 3, 2, 3)
        Name = "Form1"
        Text = "Recipe"
        CType(dgSearchResults, ComponentModel.ISupportInitialize).EndInit()
        CType(RecipeList, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        SplitContainer1.Panel2.PerformLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        SplitContainer2.Panel1.ResumeLayout(False)
        SplitContainer2.Panel1.PerformLayout()
        SplitContainer2.Panel2.ResumeLayout(False)
        SplitContainer2.Panel2.PerformLayout()
        CType(SplitContainer2, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer2.ResumeLayout(False)
        CType(dgIngredient, ComponentModel.ISupportInitialize).EndInit()
        CType(bsIngredient, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents dgSearchResults As DataGridView
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents txtCategories As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtMeasure As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDirections As TextBox
    Friend WithEvents RecipeList As BindingSource
    Friend WithEvents dgIngredient As DataGridView
    Friend WithEvents IdDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents TitleDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents bsIngredient As BindingSource
    Friend WithEvents UnitsDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents UnitTypeDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DescriptionDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
End Class
