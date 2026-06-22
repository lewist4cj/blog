# .NET Native AOT 发布指南

## 一、什么是 Native AOT

Native AOT（Ahead-Of-Time）将 .NET 代码直接编译为原生机器码，**不需要 JIT 编译器** 和 **运行时（Runtime）**。产物是一个独立的可执行文件，目标机器不需要安装 .NET SDK 或运行时。

### 优势

- **零依赖部署** — 单个 exe/elf 文件，直接运行
- **更快的启动速度** — 无 JIT 编译，冷启动毫秒级
- **更低的内存** — 无运行时元数据占用
- **更难逆向** — 原生机器码

### 代价

- **较大的文件体积** — 运行时静态链接导致体积增大（50-80MB）
- **有限反射** — 不支持运行时动态代码生成
- **兼容性限制** — 不是所有 .NET API 都可用

---

## 二、环境要求

### 操作系统要求

| 平台 | 编译工具 | 备注 |
|------|---------|------|
| **Windows** | Visual Studio C++ 工具链 | `link.exe` MSVC 链接器 |
| **Linux** | `clang` + `zlib1g-dev` | Ubuntu/Debian |
| **macOS** | Xcode CLI Tools | `xcode-select --install` |

**注意：不支持跨 OS 交叉编译。** 编译 Windows AOT 必须在 Windows 上，
编译 Linux AOT 必须在 Linux（或 WSL）上。

### .NET SDK

需要 .NET 8+ 的 SDK。本项目的 AOT 配置基于 .NET 10。

```bash
dotnet --version  # 验证 SDK 版本
```

---

## 三、项目配置

### 3.1 基础 AOT 配置 (csproj)

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>  <!-- 或 linux-x64 -->
  <SelfContained>true</SelfContained>
  <PublishTrimmed>true</PublishTrimmed>
</PropertyGroup>
```

### 3.2 本体项目的完整配置

参考 `Blog.Api/Blog.Api.csproj`：

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
  <PublishTrimmed>true</PublishTrimmed>
  <SuppressTrimAnalysisWarnings>true</SuppressTrimAnalysisWarnings>
  <DebugType>none</DebugType>                    <!-- 去掉调试符号，大幅减体积 -->
  <InvariantGlobalization>true</InvariantGlobalization>  <!-- 移除 ICU 数据 -->
  <!-- 可选：进一步减小体积 -->
  <!-- <OptimizationPreferTrimming>true</OptimizationPreferTrimming> -->
</PropertyGroup>
```

### 3.3 AOT 反射保留 (rd.xml)

Native AOT 会裁剪未使用的代码。通过 `rd.xml` 文件保留
SqlSugar 等框架所需的类型：

```xml
<Directives>
  <Application>
    <Assembly Name="SqlSugar" Dynamic="Required All" />
    <Assembly Name="Blog.Api" Dynamic="Required All" />
    <!-- 所有项目程序集 -->
  </Application>
</Directives>
```

在 csproj 中引用：

```xml
<ItemGroup>
  <RdXmlFile Include="rd.xml" />
</ItemGroup>
```

### 3.4 MVC → Minimal API 改造

**MVC Controller 在 AOT 中存在问题：**

- `ModelMetadata.IsConvertibleType` 在 AOT 下不可用
- `InferParameterBindingInfoConvention` 依赖反射推断参数绑定

**必须启用的配置：**

```csharp
builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.TypeInfoResolver = DomainJsonContext.Default;
});

// 禁用参数绑定推断（AOT 必需）
services.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
});
```

**或直接升级为 Minimal API（推荐）：**

```csharp
// 替代 [ApiController] + [Route("api/[controller]")]
var api = app.MapGroup("/api");
api.MapGroup("/user").MapUserEndpoints();
// ...
```

---

## 四、JSON 序列化（AOT 关键点）

### 4.1 源码生成上下文

所有在 API 中序列化的类型必须在 `JsonSerializerContext` 中注册：

```csharp
[JsonSerializable(typeof(ApiResult))]
[JsonSerializable(typeof(SiteMgr))]
[JsonSerializable(typeof(UserModelLoginDto))]
internal partial class DomainJsonContext : JsonSerializerContext { }
```

### 4.2 设置全局解析器

```csharp
services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.TypeInfoResolver = DomainJsonContext.Default;
});
```

### 4.3 不在上下文中的类型

如果返回类型未被注册，AOT 运行时会抛出：

```
JsonTypeInfo metadata for type 'Xxx' was not provided by TypeInfoResolver
```

**解决方法：** 将缺失的类型添加到 `DomainJsonContext`。

---

## 五、项目特定的 AOT 适配

### 5.1 EF Core → SqlSugar

AOT 不支持 EF Core 的运行时表达式编译。本项目替换为
SqlSugar 5.1.4.214 （原生支持 AOT）：

```csharp
StaticConfig.EnableAot = true;

var db = new SqlSugarClient(new ConnectionConfig
{
    DbType = DbType.PostgreSQL,
    ConnectionString = connectionString,
    // ...
});
```

### 5.2 AutoMapper

AutoMapper 依赖运行时表达式生成，在 AOT 中不可用。
解决方案：删除 AutoMapper（本项目中发现 AutoMapper 仅注册但未使用）。

### 5.3 程序集扫描

`Assembly.GetTypes()` 在 AOT 中返回空集合。
替换为显式注册：

```csharp
// 错误：运行时扫描
services.AddServiceRegister(configuration);  // 内部调用 Assembly.Load

// 正确：显式注册
services.AddScoped<IUserService, UserService>();
services.AddScoped<ILogService, LogService>();
```

### 5.4 配置绑定

`configuration.Get<T>()` 和 `configuration.Bind(obj)` 在 AOT 中
需要 `[RequiresDynamicCode]` 抑制或 `IOptions<T>` 模式。

---

## 六、发布命令

### Windows

```bash
dotnet publish -c Release -r win-x64 Blog.Api/Blog.Api.csproj
产物：Blog.Api/bin/Release/net10.0/win-x64/publish/
```

### Linux

```bash
# 需要 Linux 环境（WSL / 服务器 / Docker）
sudo apt-get install -y clang zlib1g-dev
dotnet publish -c Release -r linux-x64 Blog.Api/Blog.Api.csproj
产物：Blog.Api/bin/Release/net10.0/linux-x64/publish/
```

---

## 七、用 Docker 构建 Linux AOT

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
RUN apt-get update && apt-get install -y clang zlib1g-dev
WORKDIR /src
COPY . .
RUN dotnet publish Blog.Api/Blog.Api.csproj \
    -c Release -r linux-x64 --self-contained -o /out

FROM scratch
COPY --from=build /out/Blog.Api /Blog.Api
COPY --from=build /out/appsettings*.json /
EXPOSE 5000
ENTRYPOINT ["/Blog.Api"]
```

---

## 八、常见问题

### 8.1 `IsConvertibleType is not initialized`

**原因：** MVC 模型元数据在 AOT 中未初始化。
**解决：** a) 启用 `SuppressInferBindingSourcesForParameters = true`
或 b) 改用 Minimal API。


### 8.2 `JsonTypeInfo metadata for type 'Xxx' was not provided`

**原因：** `TypeInfoResolver` 未设置或类型未注册。
**解决：** 将类型加入 `[JsonSerializable]` 上下文。

### 8.3 `Generic implementation type has 'new()' constraint`

**原因：** 接口约束（无 `new()`）与实现约束（有 `new()`）不匹配。
**解决：** 保持接口与实现的泛型约束一致。

### 8.4 链接器错误 `vswhere.exe` / `link.exe`

**原因：** 缺少 Visual Studio C++ 工具链。
**解决：** 安装 "使用 C++ 的桌面开发" 工作负载。

---

## 九、性能与体积

| 指标 | .NET 8 (JIT) | .NET 10 AOT | 备注 |
|------|-------------|-------------|------|
| 启动时间 | ~500ms | ~20ms | 冷启动 |
| 内存占用 | ~80MB | ~30MB | RSS |
| 文件体积 | ~30MB* | 69MB | AOT 包含运行时 |
| 部署方式 | 需运行时 | 单文件 | |

*JIT 发布为单文件 + 运行时依赖

> 更多体积优化见 `Docs/AOT-Size-Optimization.md`
