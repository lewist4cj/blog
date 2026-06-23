# 即鹿無虞 - 个人博客系统

> 基于 **.NET 10** + **Vue 3** 的现代个人博客系统，支持 Native AOT 发布。

| 模块 | 技术栈 |
|------|--------|
| 后端 | .NET 10, ASP.NET Core Minimal API, SqlSugar 5.1, Npgsql |
| 前端 | Vue 3.5, TypeScript 6, Element Plus, Pinia, Vue Router, ECharts |
| 数据库 | PostgreSQL (主), Redis (缓存/黑名单) |
| 构建 | Vite 8, Native AOT, Prettier |

---

## 📋 目录

- [项目概述](#项目概述)
- [技术栈详情](#技术栈详情)
- [项目结构](#项目结构)
- [快速开始](#快速开始)
- [前端开发](#前端开发)
- [AOT 发布](#aot-发布)
- [API 概览](#api-概览)
- [配置说明](#配置说明)
- [相关文档](#相关文档)

---

## 项目概述

全栈个人博客系统，包含文章管理、用户认证、评论互动、站点配置、数据统计等完整功能。后端支持 **Native AOT** 发布（零依赖部署），前端基于 Vue 3 + Element Plus 构建。

---

## 技术栈详情

### 后端 (Blog.Api)

| 技术 | 用途 |
|------|------|
| .NET 10 | 运行时框架 |
| ASP.NET Core Minimal API | API 端点 |
| SqlSugarCore 5.1.4 | ORM 数据访问 |
| Npgsql | PostgreSQL 驱动 |
| StackExchange.Redis | Redis 缓存 |
| JWT Bearer | 身份认证 |
| Serilog | 结构化日志 |
| HtmlAgilityPack | HTML 解析 |
| IP2Region | IP 地理位置查询 |

### 前端 (Blog.Vue)

| 技术 | 用途 |
|------|------|
| Vue 3.5 | UI 框架 |
| TypeScript 6 | 类型安全 |
| Element Plus 2.14 | UI 组件库 |
| Pinia 3 | 状态管理 |
| Vue Router 5 | 路由 |
| ECharts 5.5 | 数据可视化 |
| Axios | HTTP 客户端 |
| Vite 8 | 构建工具 |
| SCSS (Sass) | 样式预处理 |
| Prettier | 代码格式化 |

---

## 项目结构

```
Blog.slnx
├── Blog.Api/                  # API 层 - Minimal API 端点
│   ├── Endpoints/             # 路由端点
│   │   ├── ArticleEndpoints.cs
│   │   ├── CategoryEndpoints.cs
│   │   ├── CommentEndpoints.cs
│   │   ├── UserEndpoints.cs
│   │   ├── SiteEndpoints.cs
│   │   ├── UploadEndpoints.cs
│   │   ├── LogEndpoints.cs
│   │   └── CollectEndpoints.cs
│   ├── Middleware/             # 中间件
│   │   ├── GlobalExceptionMiddleware.cs
│   │   └── LogMiddleware.cs
│   ├── Filters/               # 授权过滤器
│   ├── Infrastructure/        # 启动配置
│   ├── JsonContext/           # AOT JSON 序列化上下文
│   └── Program.cs             # 应用入口
│
├── Blog.Common/               # 公共工具层
│   ├── AppSettings.cs
│   ├── Database/              # 数据库配置
│   ├── RedisModule/           # Redis 封装
│   ├── TokenModule/           # JWT Token
│   ├── RespModule/            # 统一响应
│   └── Utils/                 # 工具类
│
├── Blog.Domain/               # 领域层
│   ├── Models/                # 数据模型
│   ├── Config/                # 配置模型
│   ├── Dtos/                  # 数据传输对象
│   ├── enums/                 # 枚举
│   ├── PageModel/             # 分页模型
│   └── JsonContext/           # AOT JSON 上下文
│
├── Blog.Core/                 # 数据访问层
│   ├── Repository/            # 仓储实现
│   ├── SqlSugar/              # SqlSugar 配置
│   ├── UnitOfWork/            # 工作单元
│   └── Sync/                  # 数据同步
│
├── Blog.Services/             # 业务逻辑层
│   ├── ArticleApp/            # 文章服务
│   ├── UserApp/               # 用户服务
│   ├── ConfigMgrApp/          # 配置管理
│   └── LogMgrApp/             # 日志管理
│
├── Blog.Vue/                  # 前端应用
│   └── src/
│       ├── api/               # API 请求层
│       ├── stores/            # Pinia 状态
│       ├── router/            # 路由配置
│       ├── views/             # 页面组件
│       │   ├── web/           # 前台页面
│       │   ├── admin/         # 后台管理
│       │   └── login/         # 登录
│       ├── components/        # 公共组件
│       └── assets/            # 样式/资源
│
└── Docs/                      # 技术文档
    ├── AOT-Publish-Guide.md
    └── AOT-Size-Optimization.md
```

---

## 快速开始

### 前置条件

- .NET 10 SDK
- PostgreSQL 14+
- Redis（可选，用于 Token 黑名单）
- Node.js 24+

### 后端配置与运行

1. 修改数据库连接字符串 `Blog.Api/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=blog;Username=postgres;Password=your_password;"
  }
}
```

2. 运行后端：

```bash
cd Blog.Api
dotnet run --launch-profile http
```

默认监听：`http://localhost:5051`

### 前端配置与运行

```bash
cd Blog.Vue
pnpm install
pnpm dev
```

默认访问：`http://localhost:80`

前端开发服务器已配置代理，`/api` 和 `/uploads` 请求自动转发到后端 `localhost:5051`。

---

## 前端开发

### 常用命令

| 命令 | 说明 |
|------|------|
| `pnpm dev` | 启动开发服务器 |
| `pnpm build` | 类型检查 + 生产构建 |
| `pnpm format` | 代码格式化 (Prettier) |
| `pnpm format:check` | 格式检查 |

### 编码规范

- 使用 Prettier 格式化（无分号、单引号、尾逗号、120 列宽）
- 组件名使用 PascalCase
- API 调用统一通过 `src/api/` 目录
- 状态管理通过 Pinia store

---

## AOT 发布

本项目支持 **Native AOT** 发布，生成单文件独立可执行文件，无需目标机器安装 .NET 运行时。

```bash
cd Blog.Api
dotnet publish -c Release
```

详细指南请参考：
- [AOT 发布指南](Docs/AOT-Publish-Guide.md)
- [AOT 体积优化](Docs/AOT-Size-Optimization.md)

---

## API 概览

### 文章模块

| 方法 | 路径 | 说明 |
|------|------|------|
| GET | `/api/article?page=&limit=&type=` | 文章列表（分页） |
| GET | `/api/article/{id}` | 文章详情 |
| POST | `/api/article` | 创建文章 |
| PUT | `/api/article` | 更新文章 |
| DELETE | `/api/article/{id}` | 删除文章 |

### 用户模块

| 方法 | 路径 | 说明 |
|------|------|------|
| POST | `/api/user/login` | 登录 |
| POST | `/api/user/logout` | 退出 |
| GET | `/api/user/info` | 用户信息 |
| POST | `/api/user/register` | 注册 |

### 站点配置

| 方法 | 路径 | 说明 |
|------|------|------|
| GET | `/api/site/info?name=site` | 获取站点配置 |
| PUT | `/api/site/info` | 更新站点配置 |

更多 API 请参考 [DOCUMENTATION.md](DOCUMENTATION.md)。

---

## 配置说明

关键配置项在 `Blog.Api/appsettings.json`：

| 配置节 | 说明 |
|--------|------|
| `ConnectionStrings.DefaultConnection` | PostgreSQL 连接字符串 |
| `Jwt` | JWT 认证配置（Issuer, Audience, Expire, Security） |
| `Redis` | Redis 连接（Host, Port, Password） |
| `SiteMgr` | 站点基本信息、SEO、关于、组件、登录设置 |
| `OtherSiteMgr` | 邮箱、QQ 登录、AI、七牛云配置 |
| `Serilog.Path` | 日志文件路径 |

### 站点配置运行时修改

站点配置首次从 `appsettings.json` 加载，之后同步到数据库。管理员可通过后台界面修改配置，修改后持久化到数据库，重启不丢失。

---

## 相关文档

| 文档 | 说明 |
|------|------|
| [DOCUMENTATION.md](DOCUMENTATION.md) | 完整 API 文档与系统架构 |
| [Docs/AOT-Publish-Guide.md](Docs/AOT-Publish-Guide.md) | Native AOT 发布指南 |
| [Docs/AOT-Size-Optimization.md](Docs/AOT-Size-Optimization.md) | AOT 体积优化方案 |
| [Redis.md](Redis.md) | Redis 使用说明 |
