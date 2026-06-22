<script setup lang="ts">
  import { useUserStore } from '@/stores/user'
  import { useRouter } from 'vue-router'

  const store = useUserStore()
  const router = useRouter()

  function goHome() {
    router.push('/')
  }
</script>

<template>
  <div class="web-layout">
    <el-container>
      <el-header class="web-header">
        <div class="header-content">
          <div class="logo" @click="goHome">
            <img v-if="store.siteInfo?.siteInfo.logo" :src="store.siteInfo.siteInfo.logo" alt="logo" />
            <span v-else>{{ store.siteInfo?.siteInfo.title || 'Blog' }}</span>
          </div>
          <div class="header-menu">
            <el-menu mode="horizontal" :default-active="router.currentRoute.value.name as string">
              <el-menu-item index="home">
                <router-link to="/">首页</router-link>
              </el-menu-item>
              <el-menu-item index="platform">
                <router-link to="/platform/article">文章管理</router-link>
              </el-menu-item>
              <el-menu-item index="messages" v-if="store.isLogin">
                <router-link to="/messages">消息</router-link>
              </el-menu-item>
              <el-menu-item index="admin">
                <router-link to="/admin">管理后台</router-link>
              </el-menu-item>
            </el-menu>
          </div>
          <div class="header-actions">
            <template v-if="store.isLogin">
              <el-dropdown trigger="click">
                <span class="dropdown-trigger">
                  <el-avatar :size="32">
                    <img v-if="store.userInfo.avatar" :src="store.userInfo.avatar" alt="avatar" />
                    <span v-else>{{ store.userInfo.nickname?.charAt(0) }}</span>
                  </el-avatar>
                </span>
                <template #dropdown>
                  <el-dropdown-item>
                    <router-link :to="{ name: 'userInfo' }">用户中心</router-link>
                  </el-dropdown-item>
                  <el-dropdown-item @click="store.logout()">退出登录</el-dropdown-item>
                </template>
              </el-dropdown>
            </template>
            <template v-else>
              <el-button type="primary" @click="router.push('/login')">登录</el-button>
            </template>
          </div>
        </div>
      </el-header>
      <el-main class="web-content">
        <router-view />
      </el-main>
      <el-footer class="web-footer">
        <span v-if="store.siteInfo?.about">
          {{ store.siteInfo.about.siteDate }} {{ store.siteInfo.siteInfo.title }}
          <a v-if="store.siteInfo.siteInfo.beian" :href="'https://beian.miit.gov.cn/'">{{
            store.siteInfo.siteInfo.beian
          }}</a>
        </span>
      </el-footer>
    </el-container>
  </div>
</template>

<style scoped lang="scss">
  .web-layout {
    min-height: 100vh;
  }
  .web-header {
    position: sticky;
    top: 0;
    z-index: 100;
    background: #fff;
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);
  }
  .header-content {
    display: flex;
    align-items: center;
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 20px;
    height: 60px;
  }
  .logo {
    font-size: 20px;
    font-weight: 700;
    cursor: pointer;
    margin-right: 40px;
    display: flex;
    align-items: center;
    gap: 8px;
  }
  .logo img {
    height: 32px;
  }
  .header-menu {
    flex: 1;
  }
  .header-actions {
    display: flex;
    align-items: center;
    gap: 12px;
  }
  .web-content {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
    min-height: calc(100vh - 120px);
  }
  .web-footer {
    text-align: center;
    padding: 20px;
    color: #86909c;
    font-size: 13px;
  }
</style>
