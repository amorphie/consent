using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeScopeColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Add a new temporary column with the desired type (string)
            migrationBuilder.AddColumn<string>(
                name: "ScopeTemp",
                table: "Consents",
                type: "text",
                nullable: true);

            //Copy the data from the old column to the new column
           migrationBuilder.Sql(
            @"
            UPDATE ""Consents""
            SET ""ScopeTemp"" = CAST(""Scope"" AS TEXT)
            WHERE ""Scope"" IS NOT NULL
            ");

            //Drop the old column
            migrationBuilder.DropColumn(
                name: "Scope",
                table: "Consents");

            //Rename the new column to the original column name
            migrationBuilder.RenameColumn(
                name: "ScopeTemp",
                table: "Consents",
                newName: "Scope");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          // Step 1: Add the old column back with its original type (long)
        migrationBuilder.AddColumn<long>(
            name: "ScopeTemp",
            table: "Consents",
            type: "bigint",
            nullable: true);

        // Step 2: Copy the data from the new column to the old column, converting the type
        migrationBuilder.Sql(
            @"
            UPDATE ""Consents""
            SET ""ScopeTemp"" = CAST(""Scope"" AS BIGINT)
            WHERE ""Scope"" IS NOT NULL
            ");

        // Step 3: Drop the new column
        migrationBuilder.DropColumn(
            name: "Scope",
            table: "Consents");

        // Step 4: Rename the old column back to its original name
        migrationBuilder.RenameColumn(
            name: "ScopeTemp",
            table: "Consents",
            newName: "ScopeTCKN");
        }
    }
}
