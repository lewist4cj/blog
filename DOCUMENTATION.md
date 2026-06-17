# Blog Server - 项目文档

> 一个基于 .NET 8 的个人博客系统后端 API

---

## 📋 目录

- [项目概述](#项目概述)
- [技术栈](#技术栈)
- [项目结构](#项目结构)
- [快速开始](#快速开始)
- [API 接口](#api-接口)
- [配置说明](#配置说明)
- [项目优化记录](#项目优化记录)

---

## 项目概述

基于 **ASP.NET Core 8.0** 构建的个人博客系统后端，提供文章管理、用户认证、评论互动、日志记录等完整的博客功能 API。

---

## 技术栈

| 技术 | 用途 |
|------|------|
| **.NET 8** | 运行时框架 |
| **ASP.NET Core** | Web API 框架 |
| **Entity Framework Core 8** | ORM 数据访问 |
| **PostgreSQL (Npgsql)** | 数据库（原 MySQL → PostgreSQL 迁移） |
| **Redis (StackExchange)** | 缓存/黑名单 |
| **Elasticsearch (可选)** | 全文搜索（通过配置启用） |
| **Canal CDC (可选)** | 数据库变更同步（通过配置启用） |
| **JWT Bearer** | 身份认证 |
| **AutoMapper** | 对象映射 |
| **Serilog** | 结构化日志 |
| **HtmlAgilityPack** | HTML 解析 |
| **IP2Region** | IP 地理位置查询 |

---

## 项目结构

```
Blog.sln
├── Blog.Api/              # API 层 - 控制器/启动配置
│   ├── Controllers/
│   │   ├── BaseController.cs       # 控制器基类
│   │   ├── UserController.cs       # 用户认证接口
│   │   ├── SiteController.cs       # 站点配置接口
│   │   ├── LogController.cs        # 日志管理接口
│   │   └── UploadController.cs     # 文件上传接口
│   ├── Program.cs                  # 应用入口
│   └── appsettings*.json           # 配置文件
│
├── Blog.Common/           # 公共层 - 工具/基础设施
│   ├── AppSettings.cs              # 配置管理
│   ├── ITag.cs                     # DI 标记接口
│   ├── ErrorModule/                # 错误码定义
│   ├── MD5Module/                  # MD5 密码工具
│   ├── RedisModule/                # Redis 客户端封装
│   ├── RespModule/                 # 统一 API 响应
│   ├── TokenModule/                # JWT Token 工具
│   └── Validation/                 # 配置验证扩展
│
├── Blog.Domain/           # 领域层 - 实体/枚举
│   ├── BaseEntity.cs               # 实体基类
│   ├── Models/                     # 数据模型
│   └── enums/                      # 枚举定义
│
├── Blog.Core/             # 核心层 - 数据访问
│   ├── DbContext/
│   │   └── BlogDbContext.cs         # EF Core 数据上下文
│   ├── Repository/
│   │   ├── IRepository.cs          # 仓储接口
│   │   └── Repository.cs           # 仓储实现
│   └── Migrations/                  # 数据库迁移
│
├── Blog.Services/         # 服务层 - 业务逻辑
│   ├── BlogProfile.cs              # AutoMapper 配置
│   ├── UserApp/                    # 用户服务
│   ├── LogMgrApp/                  # 日志查询服务
│   └── Log/                        # 日志记录服务
│       ├── ActionLogService.cs     # 操作日志
│       ├── RuntimeLogService.cs    # 运行时日志
│       └── LoginLogService.cs      # 登录日志
│
├── Blog.Extensions/       # 扩展层 - 中间件/配置
│   ├── CollectServiceExtension.cs   # DI 自动注册
│   ├── Extensions/
│   │   ├── WebApplicationBuilderExt.cs  # 服务配置
│   │   ├── WebApplicationExt.cs         # 管道配置
│   │   └── ModelBindExt.cs              # 模型绑定扩展
│   ├── Middleware/
│   │   ├── GlobalExceptionMiddleware.cs # 全局异常处理
│   │   └── LogMiddleware.cs             # 请求日志中间件
│   ├── Filter/
│   │   ├── AuthorizationFilterAttribute.cs  # 角色授权
│   │   └── ValidateModelFilter.cs           # 模型验证
│   ├── Config/                    # 配置模型
│   └── Validation/                # 配置验证
│
└── Blog.Repository/       # (预留) 仓储层
```

---

## 快速开始

### 前置条件

- .NET 8 SDK
- PostgreSQL 14+
- Redis（可选，用于 Token 黑名单）

### 配置

1. 修改 `Blog.Api/appsettings.json` 中的数据库连接字符串：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=blog;Username=postgres;Password=your_password;"
  }
}
```

2. 配置 Redis（可选）：

```json
{
  "Redis": {
    "Host": "127.0.0.1",
    "Port": 6379,
    "Password": "your_redis_password"
  }
}
```

### 运行

```bash
# 还原依赖
dotnet restore

# 应用数据库迁移
cd Blog.Api
dotnet ef database update

# 运行
dotnet run
```

默认监听地址：`http://localhost:5051`

### 数据库架构说明

所有高级功能（读写分离、ES同步、Canal CDC）**默认全部禁用**，确保普通单服务器即可部署运行：

```json
{
  "Database": {
    "EnableReadWriteSplitting": false,   // ← 默认禁用
    "EnableElasticsearchSync": false,    // ← 默认禁用
    "EnableCanalSync": false             // ← 默认禁用
  }
}
```

当需要扩展时，只需修改配置对应的开关为 `true` 并填写相应的连接信息即可。

---

## API 接口

### 用户模块

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| POST | `/api/user/login` | 用户登录 | ❌ |
| GET | `/api/user/unregister` | 注销登录（Token 加入黑名单） | ❌ |

### 站点管理

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| GET | `/api/site/info?name=site` | 获取站点配置 | ✅ Bearer |
| GET | `/api/site/info?name=email` | 获取邮箱配置 | ✅ SuperAdmin |
| GET | `/api/site/info?name=qq` | 获取 QQ 登录配置 | ✅ SuperAdmin |
| GET | `/api/site/redirection` | 获取 QQ 登录跳转 URL | ✅ Bearer |
| PUT | `/api/site/info?name=site` | 更新站点配置 | ✅ Bearer |
| PUT | `/api/site/update` | 更新前端 HTML 元数据 | ✅ Bearer |

### 日志管理

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| GET | `/api/log/list?pageIndex=1&pageSize=10` | 获取日志列表 | ✅ SuperAdmin |

### 文件上传

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| POST | `/api/Uploads/upload` | 文件上传 | ❌ |

---

## 配置说明

### 核心配置

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "PostgreSQL 连接字符串"
  },
  "Database": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=blog;Username=postgres;Password=postgres;",
    "EnableReadWriteSplitting": false,       // 是否启用读写分离
    "ReadConnection": "",                    // 只读副本连接（读写分离启用时）
    "EnableElasticsearchSync": false,        // 是否启用 ES 同步
    "Elasticsearch": {
      "Urls": "http://localhost:9200",
      "IndexPrefix": "blog"
    },
    "EnableCanalSync": false,                // 是否启用 Canal CDC
    "Canal": {
      "Host": "127.0.0.1",
      "Port": 11111,
      "Instance": "example"
    }
  },
  "Jwt": {
    "Issuer": "JWT 签发者",
    "Audience": "JWT 受众",
    "Expire": 10,              // 过期时间（小时）
    "Security": "签名密钥"
  },
  "Redis": {
    "Host": "Redis 主机地址",
    "Port": "Redis 端口",
    "Password": "Redis 密码"
  },
  "Serilog": {
    "Path": "日志文件路径"
  }
}
```

### 站点配置

| 配置节 | 说明 |
|--------|------|
| `SiteMgr` | 站点基本信息、SEO、关于、组件设置 |
| `OtherSiteMgr` | 邮箱、QQ 登录、AI、七牛云配置 |

### 角色权限

| 角色 | 值 | 说明 |
|------|-----|------|
| `Normal` | 1 | 普通用户 |
| `Admin` | 2 | 管理员 |
| `SuperAdmin` | 3 | 超级管理员 |

---

## 统一响应格式

所有 API 返回统一格式：

```json
{
  "code": 200,
  "message": "Success",
  "data": {}
}
```

### 错误码

| 码 | 说明 |
|----|------|
| 200 | 成功 |
| 400 | 请求参数错误 |
| 401 | 未授权/Token 过期 |
| 403 | 禁止访问 |
| 404 | 资源未找到 |
| 500 | 服务器内部错误 |
| 1001 | 用户已存在 |
| 1002 | 用户名或密码错误 |
| 2003 | 数据验证失败 |
| 2004 | 用户账户已关闭 |
| 2008 | MySQL 异常 |
| 2009 | Redis 异常 |

---

## 日志系统

系统使用 **Serilog** 进行文件日志记录，同时提供以下业务日志：

| 日志类型 | 说明 |
|----------|------|
| **操作日志** (`ActionLog`) | 通过中间件自动记录每次请求的 Request/Response |
| **运行时日志** (`RuntimeLog`) | 开发人员手动记录的业务运行时信息 |
| **登录日志** (`LoginLog`) | 用户登录行为记录 |

---

## 项目优化记录

### 已完成优化

| # | 优化项 | 说明 |
|---|--------|------|
| 1 | **消除 `BuildServiceProvider()` 反模式** | JWT 事件中使用 `HttpContext.RequestServices` 替代 `services.BuildServiceProvider()`，避免每次请求创建 DI 容器导致内存泄漏 |
| 2 | **`Func<>` → `Expression<>` 修复** | Repository 的 `Get`/`GetList` 同步方法改用 `Expression<Func<TEntity, bool>>`，避免 EF Core 在客户端（内存）中执行过滤 |
| 3 | **修复分页总记录数** | 新增 `GetCountAsync` 方法，`LogService` 分页查询现在返回真实总记录数 |
| 4 | **修复配置文件空 Key** | `appsettings.json` 中 `""` 修复为 `"OtherSiteMgr"`，使邮箱、QQ、AI、七牛云配置可正常读取 |
| 5 | **修复 SiteController Bug** | `UpdateSiteSetting` 中 `qq/qiNiu` 分支的 `resp` 变量引用错误已修正；`UpdateSiteInfo` 移除无效的 `File.Exists` URL 检查；`metaDesc` 写入 `Keywords` 已修正为 `Description` |
| 6 | **新增 CORS 配置** | 服务启动时注册了默认允许所有源的 CORS 策略 |
| 7 | **修复密码日志泄露** | `UserController.CheckLogin` 中移除了密码明文记录 |
| 8 | **`Encoding.Default` → `Encoding.UTF8`** | MD5Helper 使用确定性的 UTF-8 编码，消除平台差异 |
| 9 | **修复 `Serilogs` 拼写错误** | 配置键 `Serilogs` 修正为 `Serilog` |
| 10 | **修复 `JwtTokenModel` 字段映射** | 添加 `[JsonPropertyName("Expire")]` 使配置 `Expire` 正确映射到属性 |
| 11 | **`OtherSiteMgr` 字段改为属性** | 将 `readonly` 字段改为自动属性，使 `BindTo` 绑定生效 |
| 12 | **实现 `UploadController`** | 添加文件类型校验、大小限制、GUID 重命名等完整上传逻辑 |
| 13 | **修复 AutoMapper 版本冲突** | 降级 AutoMapper 至 12.0.1 匹配 DI 扩展版本 |
| 14 | **修复 `RedisCore` 可为 null 警告** | `Db` 属性添加 `null!` 抑制警告 |
| 15 | **`.sln` → `.slnx` 格式** | 转换到新的 XML 解决方案格式 |
| 16 | **MySQL → PostgreSQL 迁移** | Pomelo.EntityFrameworkCore.MySql → Npgsql.EntityFrameworkCore.PostgreSQL |
| 17 | **读写分离配置化** | 通过 `Database.EnableReadWriteSplitting` 控制，默认禁用 |
| 18 | **Elasticsearch 同步配置化** | 通过 `Database.EnableElasticsearchSync` 控制，默认禁用 |
| 19 | **Canal CDC 配置化** | 通过 `Database.EnableCanalSync` 控制，默认禁用 |
| 20 | **数据库配置集中管理** | 新增 `DatabaseSettings` 配置模型和 `DbContextFactory` |
| 21 | **`Code.MySqlAccessDenied` → `Code.DbAccessDenied`** | 全局异常处理更新为 PostgreSQL |

### 待优化项

| # | 建议 | 说明 |
|---|------|------|
| 1 | **MD5 → BCrypt 密码哈希** | 当前使用 MD5（不安全），建议使用 `BCrypt.Net-Next` 或 `Identity.Password.Hasher` |
| 2 | **日志批量写入** | 当前每次请求 `SaveChanges`，高并发下建议改为批量/异步队列写入 |
| 3 | **工作单元模式** | Repository 每次操作立即 `SaveChanges`，建议引入 UnitOfWork 批量提交事务 |
| 4 | **日志 HTML 拼接** | `ActionLogService` 手工拼接 HTML，建议改为结构化日志或模板渲染 |
| 5 | **`AppSettings` 静态类** | 建议逐步替换为标准的 `IOptions<T>` / `IConfiguration` DI 注入 |
| 6 | **EF Core Relational 版本冲突** | Pomelo 依赖的 EF Core 8.0.13 与项目 8.0.23 版本不一致，可升级 Pomelo |

---

## 数据库架构

### 读写分离

系统支持可选的读写分离，通过配置文件 `Database.EnableReadWriteSplitting` 控制：

```
┌─────────────┐    写入     ┌──────────┐
│  写入操作    │ ──────────→│  主库     │
│ (Insert/    │            │ (Write)   │
│  Update/    │            └──────────┘
│  Delete)    │
└─────────────┘
┌─────────────┐    读取     ┌──────────┐
│  查询操作    │ ──────────→│  从库     │
│ (Select/    │            │ (Read)    │
│  Get/List)  │            └──────────┘
└─────────────┘
```

- **禁用时**（默认）：读写均使用 `DefaultConnection`
- **启用时**：读操作路由到 `ReadConnection`，写操作路由到 `DefaultConnection`
- 通过 `ReadOnlyDbContext` 包装器实现，Repository 查询方法可选择使用只读上下文

### Elasticsearch 同步（可选）

- 通过 `Database.EnableElasticsearchSync` 控制
- 禁用时所有 ES 操作为空操作（Noop），不影响业务逻辑
- 启用时自动将文章/分类数据同步到 ES 索引
- 搜索操作优先走 ES，降级回数据库查询

### Canal CDC 同步（可选）

- 通过 `Database.EnableCanalSync` 控制
- 禁用时 Canal 后台服务不执行任何操作
- 启用时连接 Canal Server 监听数据库变更事件
- 适用于需要实时同步到 ES 或清理缓存的场景

## 依赖注入架构

### 自动注册机制

系统通过 `ITag` 标记接口和配置文件实现服务的自动注册：

1. 服务类实现 `ITag` 标记接口
2. `appsettings.json` 中 `IocTags.list` 配置要扫描的程序集
3. `CollectServiceExtension.AddServiceRegister()` 通过反射自动注册

### 已注册服务

| 服务 | 生命周期 | 说明 |
|------|----------|------|
| `AppSettings` | Singleton | 配置管理 |
| `RedisCore` | Singleton | Redis 连接 |
| `BlogDbContext` | Scoped | EF Core 数据上下文（主库） |
| `ReadOnlyDbContext` | Scoped | 只读数据上下文（从库） |
| `DbContextFactory` | Scoped | DbContext 工厂 |
| `DatabaseSettings` | Singleton | 数据库配置 |
| `IRepository<T>` | Transient | 泛型仓储 |
| `IElasticsearchSyncService` | Singleton | ES 同步（可配置空操作） |
| `CanalSyncService` | Singleton | Canal CDC 后台服务（可配置跳过） |
| `ActionLogService` | Scoped | 操作日志 |
| `RuntimeLogService` | Scoped | 运行时日志 |
| `LocalService` | Singleton | IP 地理位置 |

---

## 中间件管道

请求处理顺序（`WebApplicationExt.UseEntry`）：

```
1. GlobalExceptionMiddleware  (全局异常捕获)
2. UseCors                   (跨域)
3. UseAuthentication         (JWT 认证)
4. UseAuthorization          (授权)
5. LogMiddleware             (请求/响应日志)
6. MapControllers            (路由到控制器)
7. UseStaticFiles            (静态文件)
```
