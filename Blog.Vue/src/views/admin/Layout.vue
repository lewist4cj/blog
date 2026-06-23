<script setup lang="ts">
  import { useRouter } from 'vue-router'
  import { useUserStore } from '@/stores/user'

  const router = useRouter()
  const store = useUserStore()
</script>

<template>
  <el-container class="admin-layout">
    <el-aside :width="220">
      <div class="admin-logo" @click="router.push('/admin')">{{ store.siteInfo?.siteSettings?.title || '管理后台' }}</div>
      <el-menu :default-active="router.currentRoute.value.name as string">
        <el-menu-item index="dashboard">
          <router-link to="/admin">📊 数据统计</router-link>
        </el-menu-item>
        <el-menu-item index="userManage">
          <router-link to="/admin/users">👤 用户管理</router-link>
        </el-menu-item>
        <el-menu-item index="articleManage">
          <router-link to="/admin/articles">📄 文章管理</router-link>
        </el-menu-item>
        <el-menu-item index="siteSettings">
          <router-link to="/admin/settings">⚙️ 站点配置</router-link>
        </el-menu-item>
      </el-menu>
      <div class="admin-footer">
        <el-button style="width: 100%" @click="router.push('/')">返回前台</el-button>
      </div>
    </el-aside>
    <el-container>
      <el-header class="admin-header">
        <el-space>
          <el-avatar :size="32">{{ store.userInfo.nickname?.charAt(0) }}</el-avatar>
          <span>{{ store.userInfo.nickname }}</span>
          <el-button type="primary" link @click="store.logout()">退出</el-button>
        </el-space>
      </el-header>
      <el-main class="admin-content">
        <router-view />
      </el-main>
    </el-container>
  </el-container>
</template>

<style scoped lang="scss">
  .admin-layout {
    min-height: 100vh;
  }
  .admin-logo {
    height: 64px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 18px;
    font-weight: 700;
    cursor: pointer;
    color: #fff;
    background: #409eff;
  }
  .admin-header {
    display: flex;
    align-items: center;
    justify-content: flex-end;
    padding: 0 20px;
    height: 60px;
    background: #fff;
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);
  }
  .admin-content {
    margin: 20px;
  }
  .admin-footer {
    padding: 16px;
    border-top: 1px solid #e4e7ed;
  }
</style>
