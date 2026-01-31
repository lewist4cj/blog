using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "banner_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cover = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    href = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banner_models", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "global_notications",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    icon = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    href = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_global_notications", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nickname = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    @abstract = table.Column<string>(name: "abstract", type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    register_src = table.Column<sbyte>(type: "tinyint", nullable: true),
                    code_age = table.Column<long>(type: "bigint", nullable: true),
                    email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    open_id = table.Column<string>(type: "varchar(126)", maxLength: 126, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<sbyte>(type: "tinyint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_models", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "article_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    desc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content_id = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    tag_list = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cover = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    look_count = table.Column<long>(type: "bigint", nullable: true),
                    like_count = table.Column<long>(type: "bigint", nullable: true),
                    comment_count = table.Column<long>(type: "bigint", nullable: true),
                    collect_count = table.Column<long>(type: "bigint", nullable: true),
                    enable_comment = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_article_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "category_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "collect_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    @abstract = table.Column<string>(name: "abstract", type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cover = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    article_count = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collect_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_collect_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "log_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    log_type = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    level = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    ip = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    addr = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_read = table.Column<int>(type: "int", nullable: true),
                    login_status = table.Column<int>(type: "int", nullable: true),
                    user_name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pwd = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    login_type = table.Column<int>(type: "int", nullable: true),
                    service_name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_log_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_conf_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    like_tags = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_username_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    publish_collections = table.Column<int>(type: "int", nullable: false),
                    publish_followings = table.Column<int>(type: "int", nullable: false),
                    publish_fans = table.Column<int>(type: "int", nullable: false),
                    theme_style_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_conf_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_conf_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "article_digg_models",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_digg_models", x => new { x.user_id, x.article_id });
                    table.ForeignKey(
                        name: "FK_article_digg_models_article_models_article_id",
                        column: x => x.article_id,
                        principalTable: "article_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_article_digg_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comment_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    article_id = table.Column<long>(type: "bigint", nullable: true),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    root_parent_id = table.Column<long>(type: "bigint", nullable: true),
                    digg_count = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_comment_models_article_models_article_id",
                        column: x => x.article_id,
                        principalTable: "article_models",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_comment_models_comment_models_parent_id",
                        column: x => x.parent_id,
                        principalTable: "comment_models",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_comment_models_comment_models_root_parent_id",
                        column: x => x.root_parent_id,
                        principalTable: "comment_models",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_comment_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_article_look_history_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    article_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_article_look_history_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_article_look_history_models_article_models_article_id",
                        column: x => x.article_id,
                        principalTable: "article_models",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_article_look_history_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_article_collect_models",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    article_id = table.Column<long>(type: "bigint", nullable: false),
                    collect_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_article_collect_models", x => new { x.user_id, x.article_id, x.collect_id });
                    table.ForeignKey(
                        name: "FK_user_article_collect_models_article_models_article_id",
                        column: x => x.article_id,
                        principalTable: "article_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_article_collect_models_collect_models_collect_id",
                        column: x => x.collect_id,
                        principalTable: "collect_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_article_collect_models_user_models_user_id",
                        column: x => x.user_id,
                        principalTable: "user_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "article_digg_models",
                columns: new[] { "user_id", "article_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_article_digg_models_article_id",
                table: "article_digg_models",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_article_models_user_id",
                table: "article_models",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_category_models_user_id",
                table: "category_models",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_collect_models_user_id",
                table: "collect_models",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_models_article_id",
                table: "comment_models",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_models_parent_id",
                table: "comment_models",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_models_root_parent_id",
                table: "comment_models",
                column: "root_parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_models_user_id",
                table: "comment_models",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_log_models_user_id",
                table: "log_models",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "user_article_collect_models",
                columns: new[] { "user_id", "article_id", "collect_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_article_collect_models_article_id",
                table: "user_article_collect_models",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_article_collect_models_collect_id",
                table: "user_article_collect_models",
                column: "collect_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_article_look_history_models_article_id",
                table: "user_article_look_history_models",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_article_look_history_models_user_id",
                table: "user_article_look_history_models",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uni_user_conf_models_user_id",
                table: "user_conf_models",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article_digg_models");

            migrationBuilder.DropTable(
                name: "banner_models");

            migrationBuilder.DropTable(
                name: "category_models");

            migrationBuilder.DropTable(
                name: "comment_models");

            migrationBuilder.DropTable(
                name: "global_notications");

            migrationBuilder.DropTable(
                name: "log_models");

            migrationBuilder.DropTable(
                name: "user_article_collect_models");

            migrationBuilder.DropTable(
                name: "user_article_look_history_models");

            migrationBuilder.DropTable(
                name: "user_conf_models");

            migrationBuilder.DropTable(
                name: "collect_models");

            migrationBuilder.DropTable(
                name: "article_models");

            migrationBuilder.DropTable(
                name: "user_models");
        }
    }
}
