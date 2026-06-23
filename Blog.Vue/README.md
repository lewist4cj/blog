# Blog.Vue — 前端应用

> 即鹿無虞个人博客系统前端，基于 Vue 3 + Element Plus

## 技术栈

| 技术 | 版本 |
|------|------|
| Vue | 3.5 |
| TypeScript | 6.0 |
| Element Plus | 2.14 |
| Pinia | 3.0 |
| Vue Router | 5.1 |
| Vite | 8.0 |
| ECharts | 5.5 |
| Axios | 1.6 |
| SCSS (Sass) | 1.77 |
| Prettier | 3.8 |

## 项目结构

```
src/
├── api/           # API 请求层 (axios)
│   ├── index.ts        # axios 实例 & 拦截器
│   ├── article.ts      # 文章 API
│   ├── user.ts         # 用户 API
│   ├── category.ts     # 分类 API
│   ├── site.ts         # 站点配置 API
│   ├── comment.ts      # 评论 API
│   ├── collect.ts      # 收藏 API
│   ├── message.ts      # 消息 API
│   └── data.ts         # 数据统计 API
├── stores/        # Pinia 状态管理
│   └── user.ts         # 用户/站点信息 store
├── router/        # Vue Router 配置
│   └── index.ts
├── views/         # 页面组件
│   ├── web/            # 前台页面
│   │   ├── Layout.vue         # 前台布局
│   │   ├── Home.vue           # 首页（文章列表）
│   │   ├── ArticleDetail.vue  # 文章详情
│   │   ├── Messages.vue       # 消息中心
│   │   ├── platform/          # 平台相关
│   │   └── user/              # 用户中心
│   ├── admin/           # 后台管理
│   │   ├── Layout.vue         # 后台布局
│   │   ├── Dashboard.vue      # 仪表盘
│   │   ├── ArticleManage.vue  # 文章管理
│   │   ├── UserManage.vue     # 用户管理
│   │   └── SiteSettings.vue   # 站点设置
│   └── login/           # 登录页
│       └── Login.vue
├── components/    # 公共组件
│   └── CommentSection.vue  # 评论组件
└── assets/        # 样式/资源
    └── global.scss        # 全局样式
```

## 快速开始

### 前置条件

- Node.js 24+ / pnpm

### 安装与运行

```sh
pnpm install
pnpm dev
```

默认访问：`http://localhost:80`

开发服务器已配置 Vite 代理，`/api` 和 `/uploads` 请求自动转发到后端 `localhost:5051`。

### 常用命令

| 命令 | 说明 |
|------|------|
| `pnpm dev` | 启动开发服务器 |
| `pnpm build` | 类型检查 + 生产构建 |
| `pnpm preview` | 预览生产构建 |
| `pnpm format` | 代码格式化 (Prettier) |
| `pnpm format:check` | 格式检查 |

## 编码规范

- 使用 Prettier 格式化：无分号、单引号、尾逗号、120 列宽
- 组件名使用 PascalCase
- API 调用统一通过 `src/api/` 目录
- 状态管理通过 Pinia store

## 相关文档

- [项目总览 README](../README.md)
- [后端 API 文档](../DOCUMENTATION.md)
