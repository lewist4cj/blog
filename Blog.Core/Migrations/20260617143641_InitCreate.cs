using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "article_digg_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    article_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_digg_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "article_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    title = table.Column<string>(type: "text", nullable: true),
                    desc = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    content_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    tag_list = table.Column<string>(type: "text", nullable: true),
                    cover = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    look_count = table.Column<long>(type: "bigint", nullable: true),
                    like_count = table.Column<long>(type: "bigint", nullable: true),
                    comment_count = table.Column<long>(type: "bigint", nullable: true),
                    collect_count = table.Column<long>(type: "bigint", nullable: true),
                    enable_comment = table.Column<bool>(type: "boolean", nullable: true),
                    status = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "banner_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    cover = table.Column<string>(type: "text", nullable: true),
                    href = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banner_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "collect_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    @abstract = table.Column<string>(name: "abstract", type: "character varying(256)", maxLength: 256, nullable: true),
                    cover = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    article_count = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collect_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "comment_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    content = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    article_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    parent_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    root_parent_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    digg_count = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "global_notications",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    content = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    href = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_global_notications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    log_type = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    ip = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    addr = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    is_read = table.Column<bool>(type: "boolean", nullable: true),
                    login_status = table.Column<bool>(type: "boolean", nullable: true),
                    user_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    pwd = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    login_type = table.Column<short>(type: "smallint", nullable: true),
                    service_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_article_collect_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    article_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    collect_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_article_collect_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_article_look_history_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    article_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_article_look_history_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_conf_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    user_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    like_tags = table.Column<string>(type: "text", nullable: true),
                    update_username_date = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    publish_collections = table.Column<bool>(type: "boolean", nullable: true),
                    publish_followings = table.Column<bool>(type: "boolean", nullable: true),
                    publish_fans = table.Column<bool>(type: "boolean", nullable: true),
                    theme_style_id = table.Column<decimal>(type: "numeric(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_conf_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_models",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    nickname = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    password = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    avatar = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    @abstract = table.Column<string>(name: "abstract", type: "character varying(256)", maxLength: 256, nullable: true),
                    register_src = table.Column<short>(type: "smallint", nullable: true),
                    code_age = table.Column<long>(type: "bigint", nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    open_id = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: true),
                    role = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_models", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "article_digg_models",
                columns: new[] { "user_id", "article_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_name1",
                table: "user_article_collect_models",
                columns: new[] { "user_id", "article_id", "collect_id" },
                unique: true);

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
                name: "article_models");

            migrationBuilder.DropTable(
                name: "banner_models");

            migrationBuilder.DropTable(
                name: "category_models");

            migrationBuilder.DropTable(
                name: "collect_models");

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
                name: "user_models");
        }
    }
}
