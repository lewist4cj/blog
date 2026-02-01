# 博客系统 EF Core 模型关系图

## 🏗️ 核心模型结构

### 基础模型
```
BaseModel (抽象类)
├── Id (long, 主键)
├── CreatedAt (DateTime, 创建时间)
└── UpdatedAt (DateTime, 更新时间)
```

### 用户相关模型
```
UserModel (用户表)
├── 继承: BaseModel
├── 属性: Username, Nickname, Password, Email 等
├── 关系:
│   ├── 一对多: Articles (文章)
│   ├── 一对多: Comments (评论)
│   ├── 一对多: Logs (日志)
│   └── 一对一: UserConfig (用户配置)
└── 索引: idx_users_username (用户名唯一索引)

UserConfModel (用户配置表)
├── 继承: BaseModel
├── 属性: UserId (外键), LikeTags, PublishSettings 等
├── 关系:
│   └── 多对一: User (所属用户)
└── 约束: uni_user_conf_models_user_id (UserId 唯一)
```

### 内容相关模型
```
CategoryModel (分类表)
├── 继承: BaseModel
├── 属性: Title, Abstract, Cover, ArticleCount 等
├── 关系:
│   ├── 多对一: User (创建者)
│   └── 一对多: Articles (分类下的文章)
└── 索引: idx_category_user_id

ArticleModel (文章表)
├── 继承: BaseModel
├── 属性: Title, Content, UserId, Status 等
├── 关系:
│   ├── 多对一: User (作者)
│   ├── 多对一: Category (所属分类)
│   ├── 一对多: Comments (评论)
│   ├── 一对多: ArticleDiggs (点赞)
│   ├── 一对多: UserArticleCollects (收藏)
│   └── 一对多: UserArticleLookHistories (浏览记录)
└── 索引: idx_articles_user_id
```

### 互动相关模型
```
CommentModel (评论表)
├── 继承: BaseModel
├── 属性: Content, UserId, ArticleId, ParentId 等
├── 关系:
│   ├── 多对一: User (评论者)
│   ├── 多对一: Article (所属文章)
│   ├── 多对一: Parent (父评论)
│   └── 一对多: Replies (回复)
└── 索引: idx_comment_user_id, idx_comment_article_id

ArticleDiggModel (文章点赞表)
├── 继承: BaseModel
├── 属性: UserId, ArticleId
├── 关系:
│   ├── 多对一: User (点赞用户)
│   └── 多对一: Article (被点赞文章)
└── 约束: uni_user_article_digg (用户对文章点赞唯一)

UserArticleCollectModel (用户文章收藏表)
├── 继承: BaseModel
├── 属性: UserId, ArticleId, CollectId
├── 关系:
│   ├── 多对一: User (收藏用户)
│   ├── 多对一: Article (被收藏文章)
│   └── 多对一: Collect (收藏夹)
└── 约束: uni_user_article_collect (用户在收藏夹中收藏文章唯一)
```

### 系统相关模型
```
LogModel (日志表)
├── 继承: BaseModel
├── 属性: LogType, Title, Content, Level 等
├── 关系:
│   └── 多对一: User (相关用户)
└── 无特殊约束

GlobalNotificationModel (全局通知表)
├── 继承: BaseModel
├── 属性: Title, Icon, Content, Href
└── 无关系

BannerModel (横幅广告表)
├── 继承: BaseModel
├── 属性: Cover, Href
└── 无关系

UserArticleLookHistoryModel (用户文章浏览历史表)
├── 继承: BaseModel
├── 属性: UserId, ArticleId
├── 关系:
│   ├── 多对一: User (浏览用户)
│   └── 多对一: Article (浏览文章)
└── 无特殊约束
```

## 🔗 模型关系总结

### 主要关系类型：
1. **继承关系**: 所有业务模型继承 BaseModel
2. **一对多关系**: User → Articles/Comments/Logs
3. **多对一关系**: Article → User/Category
4. **自引用关系**: Comment → Parent/Replies
5. **多对多关系**: 通过中间表实现 (ArticleDigg, UserArticleCollect)

### 性能优化配置：
- ✅ 禁用外键约束（按项目要求）
- ✅ 保留必要业务索引
- ✅ 配置唯一约束防止重复数据
- ✅ 合理的查询跟踪策略

### 命名规范：
- 表名: snake_case (如 user_models)
- 字段名: snake_case (如 user_id)
- 模型名: PascalCase (如 UserModel)
- 导航属性: PascalCase (如 User, Articles)