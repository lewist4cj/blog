using blog.Models.enums.Log;

namespace blog.Models;

public class LogModel : BaseModel
{
    public LogTypeEnum LogType { get; set; }

    public string Title { get; set; } = string.Empty;   

    public string Content { get; set; } = string.Empty;

    public LogLevelEnum Level { get; set; }

    public long UserId { get; set; }

    public string Ip   { get; set; } = string.Empty;

    public string Addr { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public bool LoginStatus { get; set; }
    
    public string UserName { get; set; } = string.Empty;

    public string Pwd { get; set; } = string.Empty;

    public short LoginType { get; set; }

    public string ServiceName { get; set; } = string.Empty;
}

// CREATE TABLE `log_models` (
//     `id` bigint unsigned NOT NULL AUTO_INCREMENT,
//     `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
//     `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
//     `log_type` tinyint DEFAULT NULL,
//     `title` varchar(32) DEFAULT NULL,
//     `content` longtext,
//     `level` tinyint DEFAULT NULL,
//     `user_id` bigint unsigned DEFAULT NULL,
//     `ip` varchar(32) DEFAULT NULL,
//     `addr` varchar(64) DEFAULT NULL,
//     `is_read` tinyint(1) DEFAULT NULL,
//     `login_status` tinyint(1) DEFAULT NULL,
//     `user_name` varchar(32) DEFAULT NULL,
//     `pwd` varchar(32) DEFAULT NULL,
//     `login_type` tinyint DEFAULT NULL,
//     `service_name` varchar(32) DEFAULT NULL,
//     PRIMARY KEY (`id`)
//     ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;