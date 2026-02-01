# EF Core 反向工程模型整合指南

## 生成的模型位置
`Blog.Models/GeneratedModels/` 目录下包含所有自动生成的模型文件

## 整合步骤

### 1. 主键类型统一
将所有生成模型中的 `ulong Id` 修改为 `long Id` 以匹配 BaseModel

### 2. 继承 BaseModel
让所有实体模型继承 BaseModel，移除重复的时间戳字段

### 3. 命名空间调整
将 `Blog.Models.GeneratedModels` 修改为 `blog.Models`

### 4. 枚举类型处理
将数字字段转换为相应的枚举类型：
- RegisterSrc → RegisterSrcEnum
- Role → RoleEnum
- LogLevel → LogLevelEnum 等

## 需要手动处理的模型

### UserModel (优先级：高)
- 继承 BaseModel
- 统一主键类型为 long
- 保持现有验证属性

### ArticleModel (优先级：高)
- 继承 BaseModel  
- 处理 content_id 的 ulong 类型问题
- 添加导航属性

### 其他模型 (优先级：中)
- CommentModel, CollectModel, LogModel 等
- 按需添加导航属性
- 统一数据类型

## 注意事项

1. **备份现有模型**：在替换前做好备份
2. **逐步替换**：建议一次只替换一个模型进行测试
3. **测试验证**：每个模型替换后都要进行功能测试
4. **迁移更新**：可能需要更新数据库迁移

## 推荐的替换顺序

1. BaseModel 相关模型 (UserModel, ArticleModel)
2. 日志相关模型 (LogModel)
3. 关系模型 (CommentModel, CollectModel)
4. 配置模型 (UserConfModel)
5. 其他辅助模型

生成的模型文件可以作为参考，但需要根据项目实际情况进行适当调整。