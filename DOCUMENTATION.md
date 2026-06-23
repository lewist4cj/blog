# Blog Server - 项目文档

> 基于 **.NET 10** + **Vue 3** 的全栈个人博客系统

---

## 📋 目录

- [项目概述](#项目概述)
- [技术栈](#技术栈)
- [项目结构](#项目结构)
- [快速开始](#快速开始)
- [API 接口](#api-接口)
- [配置说明](#配置说明)
- [架构设计](#架构设计)
- [Native AOT](#native-aot)
- [数据库](#数据库)
- [日志系统](#日志系统)
- [项目优化记录](#项目优化记录)

---

## 项目概述

基于 **ASP.NET Core 10.0** + **Vue 3** 构建的全栈个人博客系统，提供文章管理、分类管理、用户认证、评论互动、收藏管理、站点配置、日志记录、文件上传等完整的博客功能 API。

---

## 技术栈

| 技术 | 用途 |
|------|------|
| **.NET 10** | 运行时框架 |
| **ASP.NET Core Minimal API** | Web API 框架 |
| **SqlSugarCore 5.1.4** | ORM 数据访问 |
| **PostgreSQL (Npgsql)** | 数据库 |
| **Redis (StackExchange)** | 缓存/黑名单 |
| **JWT Bearer** | 身份认证 |
| **Serilog** | 结构化日志 |
| **HtmlAgilityPack** | HTML 解析 |
| **IP2Region** | IP 地理位置查询 |

### 前端技术栈

| 技术 | 用途 |
|------|------|
| **Vue 3.5** | 前端框架 |
| **TypeScript 6** | 类型检查 |
| **Element Plus 2.14** | UI 组件库 |
| **Pinia 3** | 状态管理 |
| **Vue Router 5** | 路由管理 |
| **Vite 8** | 构建工具 |
| **ECharts 5.5** | 数据可视化 |
| **Axios** | HTTP 请求 |
| **SCSS (Sass)** | CSS 预处理器 |

---

## 项目结构

```
Blog.sln
├── Blog.Api/                  # API 层 - Minimal API 端点/启动配置
│   ├── Endpoints/
│   │   ├── ArticleEndpoints.cs     # 文章接口
│   │   ├── CategoryEndpoints.cs    # 分类接口
│   │   ├── UserEndpoints.cs        # 用户认证接口
│   │   ├── CommentEndpoints.cs     # 评论接口
│   │   ├── CollectEndpoints.cs     # 收藏接口
│   │   ├── SiteEndpoints.cs        # 站点配置接口
│   │   ├── LogEndpoints.cs         # 日志管理接口
│   │   └── UploadEndpoints.cs      # 文件上传接口
│   ├── Middleware/
│   │   ├── GlobalExceptionMiddleware.cs  # 全局异常处理
│   │   └── LogMiddleware.cs              # 请求日志中间件
│   ├── Filters/
│   │   └── RoleAuthorizationFilter.cs    # 角色授权过滤器
│   ├── Infrastructure/
│   │   ├── CollectServiceExtension.cs    # DI 自动注册
│   │   ├── WebApplicationBuilderExt.cs   # 服务配置
│   │   └── WebApplicationExt.cs          # 管道配置
│   ├── JsonContext/
│   │   └── AppJsonContext.cs             # AOT JSON 序列化上下文
│   ├── Program.cs                        # 应用入口
│   └── appsettings*.json                 # 配置文件
│
├── Blog.Common/               # 公共层 - 工具/基础设施
│   ├── AppSettings.cs                    # 配置管理
│   ├── ITag.cs                           # DI 标记接口
│   ├── ErrorModule/                      # 错误码定义
│   ├── MD5Module/                        # MD5 密码工具
│   ├── RedisModule/                      # Redis 客户端封装
│   ├── RespModule/                       # 统一 API 响应
│   ├── TokenModule/                      # JWT Token 工具
│   ├── Utils/                            # 通用工具类
│   └── Validation/                       # 配置验证扩展
│
├── Blog.Domain/               # 领域层 - 实体/枚举/DTO
│   ├── BaseEntity.cs                     # 实体基类
│   ├── Models/                           # 数据模型
│   ├── Dtos/                             # 数据传输对象
│   ├── Config/
│   │   ├── SiteMgr.cs                    # 站点配置模型
│   │   ├── OtherMgr.cs                   # 其他配置模型
│   │   └── Settings.cs                   # 系统设置
│   ├── JsonContext/                      # AOT JSON 序列化上下文
│   ├── enums/                            # 枚举定义
│   └── PageModel/                        # 分页模型
│
├── Blog.Core/                 # 核心层 - 数据访问
│   ├── SqlSugar/
│   │   ├── SugarContext.cs               # SqlSugar 数据上下文
│   │   └── SugarRepository.cs            # 仓储实现
│   ├── Repository/                       # 仓储接口
│   ├── Sync/                             # 数据同步服务
│   └── UnitOfWork/                       # 工作单元
│       └── UnitOfWork.cs                 # 事务管理
│
├── Blog.Services/             # 服务层 - 业务逻辑
│   ├── ArticleApp/                       # 文章服务
│   ├── ConfigMgrApp/                     # 配置管理服务
│   ├── LogMgrApp/                        # 日志查询服务
│   └── UserApp/                          # 用户服务
│
├── Blog.Vue/                  # 前端 - Vue 3 应用
│   ├── src/
│   │   ├── api/                          # API 请求层
│   │   ├── stores/                       # Pinia 状态管理
│   │   ├── router/                       # Vue Router 路由
│   │   ├── views/                        # 页面组件
│   │   ├── components/                   # 公共组件
│   │   ├── assets/                       # 静态资源
│   │   └── styles/                       # 全局样式
│   ├── public/                           # 公共资源
│   ├── index.html                        # 入口 HTML
│   ├── vite.config.ts                    # Vite 配置
│   └── package.json                      # 依赖管理
│
└── Docs/                      # 项目文档
    ├── AOT-Publish-Guide.md              # AOT 发布指南
    └── AOT-Size-Optimization.md          # AOT 体积优化
```

---

## 快速开始

### 前置条件

- .NET 10 SDK
- PostgreSQL 14+
- Redis（可选，用于 Token 黑名单）
- Node.js 20+（前端开发）

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

# 运行后端（SqlSugar 自动创建表结构）
cd Blog.Api
dotnet run
```

默认监听地址：`http://localhost:5051`

### 前端启动

```bash
# 进入前端目录
cd Blog.Vue

# 安装依赖
pnpm install

# 启动开发服务器
pnpm dev
```

前端默认监听地址：`http://localhost:5173`（Vite 代理 API 请求到后端）

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

### 文章模块

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| GET | `/api/article/list` | 获取文章列表（分页） | ❌ |
| GET | `/api/article/{id}` | 获取文章详情 | ❌ |
| POST | `/api/article` | 创建文章 | ✅ Bearer |
| PUT | `/api/article` | 更新文章 | ✅ Bearer |
| DELETE | `/api/article/{id}` | 删除文章 | ✅ Bearer |
| PUT | `/api/article/top/{id}` | 置顶/取消置顶文章 | ✅ Bearer |
| PUT | `/api/article/visible/{id}` | 公开/隐藏文章 | ✅ Bearer |

### 分类模块

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| GET | `/api/category` | 获取全部分类 | ❌ |
| POST | `/api/category` | 创建分类 | ✅ Bearer |
| PUT | `/api/category` | 更新分类 | ✅ Bearer |
| DELETE | `/api/category/{id}` | 删除分类 | ✅ Bearer |

### 用户模块

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| POST | `/api/user/login` | 用户登录 | ❌ |
| GET | `/api/user/unregister` | 注销登录（Token 加入黑名单） | ❌ |

### 评论模块

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| GET | `/api/comment/{articleId}` | 获取文章评论 | ❌ |
| POST | `/api/comment` | 发表评论 | ❌ |
| DELETE | `/api/comment/{id}` | 删除评论 | ✅ Bearer |

### 收藏模块

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| GET | `/api/collect/list` | 获取收藏列表 | ✅ Bearer |
| POST | `/api/collect` | 添加收藏 | ✅ Bearer |
| DELETE | `/api/collect/{id}` | 取消收藏 | ✅ Bearer |

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
| POST | `/api/upload` | 文件上传 | ❌ |

### 数据统计

| 方法 | 路径 | 说明 | 认证 |
|------|------|------|------|
| GET | `/api/article/statistics` | 文章数据统计 | ❌ |

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
| 2008 | 数据库异常 |
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
| 1 | **EF Core → SqlSugar** | 从 Entity Framework Core 迁移到 SqlSugarCore 5.1.4，消除 EF Core 版本冲突，简化数据访问 |
| 2 | **Controller → Minimal API** | 从传统的 Controller 基类模式迁移到 ASP.NET Core Minimal API，减少样板代码 |
| 3 | **Native AOT 支持** | 项目配置为支持 Native AOT 发布，减小部署体积并提升启动速度 |
| 4 | **AOT JSON 序列化** | 使用 `JsonSerializerContext` 源码生成器，避免 AOT 下反射序列化问题 |
| 5 | **AOT 配置绑定** | 配置绑定使用源码生成器，确保 AOT 下配置正确加载 |
| 6 | **Vue 3 + Element Plus 前端** | 使用 Vite 8 + Vue 3.5 + Element Plus 2.14 重构前端 |
| 7 | **MySQL → PostgreSQL** | 从 MySQL 迁移到 PostgreSQL，使用 Npgsql 驱动 |
| 8 | **Vite 代理配置** | 开发环境通过 Vite proxy 代理 API 请求，解决跨域问题 |
| 9 | **运行时配置持久化** | 配置支持运行时修改并持久化，部分配置项可热更新 |
| 10 | **`.sln` → `.slnx` 格式** | 转换到新的 XML 解决方案格式 |
| 11 | **`Code.MySqlAccessDenied` → `Code.DbAccessDenied`** | 全局异常处理更新为 PostgreSQL |
| 12 | **修复配置空 Key** | `appsettings.json` 中 `""` 修复为 `"OtherSiteMgr"` |
| 13 | **新增 CORS 配置** | 服务启动时注册了默认允许所有源的 CORS 策略 |
| 14 | **修复 `Serilogs` 拼写错误** | 配置键 `Serilogs` 修正为 `Serilog` |
| 15 | **`Encoding.Default` → `Encoding.UTF8`** | MD5Helper 使用确定性的 UTF-8 编码，消除平台差异 |

### 待优化项

| # | 建议 | 说明 |
|---|------|------|
| 1 | **MD5 → BCrypt 密码哈希** | 当前使用 MD5（不安全），建议使用 `BCrypt.Net-Next` |
| 2 | **日志批量写入** | 当前每次请求写入日志，高并发下建议改为批量/异步队列写入 |
| 3 | **工作单元模式完善** | UnitOfWork 已初步实现，需完善事务提交和回滚机制 |


