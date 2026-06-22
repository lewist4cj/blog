#!/bin/bash
set -e

echo "========================================"
echo "  Blog.Server - Linux AOT 发布"
echo "========================================"
echo ""

# 安装 AOT 依赖（首次运行需要）
if ! command -v clang &> /dev/null; then
    echo "安装 AOT 编译依赖..."
    sudo apt-get update && sudo apt-get install -y clang zlib1g-dev
fi

# 清理旧产物
rm -rf publish

# 发布 AOT
dotnet publish Blog.Api/Blog.Api.csproj \
  -c Release -r linux-x64 \
  --self-contained

# 删除调试符号
rm -f publish/*.pdb 2>/dev/null

echo ""
echo "========================================"
echo "  ✅ 发布完成！"
echo "  产物: publish/Blog.Api"
echo "  运行: ./publish/Blog.Api"
echo "========================================"
