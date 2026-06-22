# Native AOT 体积优化指南

## 当前体积分析

```
Blog.Api.exe        ≈ 69 MB  （原生可执行文件）
Blog.Api.pdb        ≈ 272 MB （调试符号，发布时不需要）
e_sqlite3.dll       ≈ 1.7 MB （SqlSugar 捆绑的 SQLite 驱动，未使用）
SqlClient.SNI.dll   ≈ 492 KB （SqlSugar 捆绑的 SQL Server 驱动，未使用）
```

**72MB（含 PDB 时 343MB）**。PDB 是调试符号，**发布时不需分发**。

---

## 一、已应用的优化

```xml
<PublishTrimmed>true</PublishTrimmed>
<SuppressTrimAnalysisWarnings>true</SuppressTrimAnalysisWarnings>
```

---

## 二、推荐优化（按效果排序）

### 2.1 去掉调试符号 🔥 省 ~270MB（PDB）+ 减小 exe

```xml
<DebugType>none</DebugType>
```

PDB 文件 272MB —— 这是当前体积最大的元凶。去掉后发布目录仅 69MB。
同时 `DebugType=none` 也让 exe 本身的调试信息减少约 3-5MB。

### 2.2 移除全球化 ICU 数据 🔥 省 ~5-10MB

```xml
<InvariantGlobalization>true</InvariantGlobalization>
```

移除 ICU（International Components for Unicode）数据包。
如果应用只用基础字符处理（不涉及复杂字符串排序/文化信息），
启用后通常无影响。

**注意事项：**
- `string.Compare("ä", "a")` 的结果可能不同
- 货币格式、日期格式固定为不变文化

### 2.3 激进裁剪 🔥 省 ~5MB

```xml
<OptimizationPreferTrimming>true</OptimizationPreferTrimming>
```

让 IL Linker 更激进地裁剪框架程序集。

```xml
<TrimMode>link</TrimMode>
```

对 `System.Private.CoreLib` 等核心库也进行链接裁剪。
**风险：** 可能剪掉反射依赖的类型，需要更完整的 rd.xml。

### 2.4 合并相同方法体 ⭐ 省 ~2-3MB

```xml
<IlcFoldIdenticalMethodBodies>true</IlcFoldIdenticalMethodBodies>
```

ILC 编译器会将代码相同的方法合并为同一个实现，减少代码量。

### 2.5 全局禁用堆栈跟踪信息 ⭐ 省 ~1-2MB

```xml
<StackTraceSupport>false</StackTraceSupport>
```

关闭堆栈字符串生成。异常时仍会抛异常，但 `Exception.StackTrace`
返回简化信息。

### 2.6 移除事件源 ✅ 省 ~1MB

```xml
<EventSourceSupport>false</EventSourceSupport>
```

移除以 `System.Diagnostics.Tracing.EventSource` 为基础的事件跟踪。

### 2.7 完整配置参考

```xml
<PropertyGroup>
  <!-- AOT 基础配置 -->
  <PublishAot>true</PublishAot>
  <PublishTrimmed>true</PublishTrimmed>
  <SuppressTrimAnalysisWarnings>true</SuppressTrimAnalysisWarnings>

  <!-- 体积优化 -->
  <DebugType>none</DebugType>
  <InvariantGlobalization>true</InvariantGlobalization>
  <OptimizationPreferTrimming>true</OptimizationPreferTrimming>
  <TrimMode>link</TrimMode>
  <IlcFoldIdenticalMethodBodies>true</IlcFoldIdenticalMethodBodies>

  <!-- 可选：进一步减小 -->
  <StackTraceSupport>false</StackTraceSupport>
  <EventSourceSupport>false</EventSourceSupport>
</PropertyGroup>
```

---

## 三、预期优化效果

| 配置 | exe 体积 | 启动时间 | 备注 |
|------|---------|---------|------|
| 当前（无优化） | ~69 MB | ~20ms | 含完整调试信息 |
| `DebugType=none` | ~65 MB | ~20ms | 推荐 |
| + `InvariantGlobalization` | ~55 MB | ~18ms | 推荐 |
| + `OptimizationPreferTrimming` | ~50 MB | ~18ms | 推荐 |
| + `TrimMode=link` | ~40 MB | ~17ms | 风险较高 |
| + `IlcFoldIdenticalMethodBodies` | ~38 MB | ~17ms | 安全 |
| + `StackTraceSupport=false` | ~37 MB | ~15ms | 生产可用 |

**最终可达：35-40 MB**

---

## 四、注意：PublishSingleFile 与 AOT 的关系

```xml
<PublishSingleFile>true</PublishSingleFile>
<PublishTrimmed>true</PublishTrimmed>
<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
```

这些是 **JIT 单文件发布** 的配置，与 Native AOT 无关。
Native AOT 本身就是单文件。可以安全移除：

```diff
- <PublishSingleFile>true</PublishSingleFile>
- <PublishTrimmed>true</PublishTrimmed>  ← AOT 自带裁剪
- <IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
```

`PublishTrimmed` 在 AOT 模式下默认启用，不需要显式声明。

---

## 五、第三方依赖瘦身

### SqlSugar 多数据库驱动

SqlSugar 捆绑了多个数据库的驱动：

```
SqlSugarCore 5.1.4.214 捆绑：
  ├── Npgsql         ✅ (我们使用的 PostgreSQL)
  ├── MySql.Data     ❌ 未使用
  ├── Microsoft.Data.SqlClient ❌ 未使用
  ├── e_sqlite3      ❌ 未使用
  ├── Oracle.ManagedDataAccess ❌ 未使用
  ├── Kdbndp         ❌ 未使用
  └── 其他国产数据库   ❌ 未使用
```

**解决方案：** 使用 `SqlSugarCoreNoDrive` + 手动添加需要的驱动包。
但该包版本较旧（5.1.4.186 vs 5.1.4.214）。权衡稳定性和体积。

### rd.xml 配置

rd.xml 保留的类型越多，可裁剪空间越小。
建议只保留必要的程序集：

```xml
<Assembly Name="SqlSugar" Dynamic="Required All" />
<!-- 去掉不必要的程序集保留 -->
```

---

## 六、最终建议配置

对于生产环境，推荐：

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
  <DebugType>none</DebugType>
  <InvariantGlobalization>true</InvariantGlobalization>
  <OptimizationPreferTrimming>true</OptimizationPreferTrimming>
  <IlcFoldIdenticalMethodBodies>true</IlcFoldIdenticalMethodBodies>
</PropertyGroup>
```

预期效果：
- exe 体积：**~50 MB**
- 启动时间：**< 20ms**
- 内存占用：**~30MB RSS**
