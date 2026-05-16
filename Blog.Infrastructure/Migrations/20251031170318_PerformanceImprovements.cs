using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PerformanceImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reactions_CommentId_UserId_Kind",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_PostId_UserId_Kind",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId_CreatedAt",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId_CreatedAt",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId_Kind",
                table: "Reactions",
                columns: new[] { "CommentId", "Kind" },
                filter: "[CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId_UserId",
                table: "Reactions",
                columns: new[] { "CommentId", "UserId" },
                unique: true,
                filter: "[CommentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId_Kind",
                table: "Reactions",
                columns: new[] { "PostId", "Kind" },
                filter: "[PostId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId_UserId",
                table: "Reactions",
                columns: new[] { "PostId", "UserId" },
                unique: true,
                filter: "[PostId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId_CreatedAt",
                table: "Posts",
                columns: new[] { "UserId", "CreatedAt" },
                descending: new[] { false, true })
                .Annotation("SqlServer:Include", new[] { "Content" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId_CreatedAt",
                table: "Comments",
                columns: new[] { "PostId", "CreatedAt" },
                descending: new[] { false, true },
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reactions_CommentId_Kind",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_CommentId_UserId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_PostId_Kind",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_PostId_UserId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId_CreatedAt",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId_CreatedAt",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId_UserId_Kind",
                table: "Reactions",
                columns: new[] { "CommentId", "UserId", "Kind" },
                unique: true,
                filter: "[CommentId] IS NOT NULL AND [UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId_UserId_Kind",
                table: "Reactions",
                columns: new[] { "PostId", "UserId", "Kind" },
                unique: true,
                filter: "[PostId] IS NOT NULL AND [UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId_CreatedAt",
                table: "Posts",
                columns: new[] { "UserId", "CreatedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId_CreatedAt",
                table: "Comments",
                columns: new[] { "PostId", "CreatedAt" });
        }
    }
}
