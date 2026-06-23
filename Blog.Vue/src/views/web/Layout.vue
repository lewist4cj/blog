<script setup lang="ts">
  import { onMounted } from 'vue'
  import { useUserStore } from '@/stores/user'
  import { useRouter } from 'vue-router'

  const store = useUserStore()
  const router = useRouter()

  onMounted(() => {
    store.loadSiteInfo()
  })

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
            <span>{{ store.siteInfo?.siteSettings?.title || '即鹿無虞' }}</span>
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
        <span v-if="store.siteInfo?.aboutSettings">
          {{ store.siteInfo.aboutSettings.siteDate }}
          <a v-if="store.siteInfo.aboutSettings.gitee" :href="store.siteInfo.aboutSettings.gitee">{{ store.siteInfo.siteSettings?.title || '即鹿無虞' }}</a>
          <template v-else>{{ store.siteInfo.siteSettings?.title || '即鹿無虞' }}</template>
          <a v-if="store.siteInfo.siteSettings?.beiAn" :href="'https://beian.miit.gov.cn/'" target="_blank">
            {{ store.siteInfo.siteSettings.beiAn }}
          </a>
        </span>
        <span v-else>{{ store.siteInfo?.siteSettings?.title || '即鹿無虞' }}</span>
      </el-footer>
    </el-container>
  </div>
</template>

<style scoped lang="scss">
  .web-layout {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
  }
  .web-layout :deep(.el-container) {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
  }
  .web-header {
    position: sticky;
    top: 0;
    z-index: 100;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(12px);
    -webkit-backdrop-filter: blur(12px);
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.06);
  }
  .header-content {
    display: flex;
    align-items: center;
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 24px;
    height: var(--blog-header-height);
  }
  .logo {
    font-size: 22px;
    font-weight: 800;
    cursor: pointer;
    margin-right: 48px;
    display: flex;
    align-items: center;
    gap: 10px;
    color: #1d2129;
    letter-spacing: -0.5px;
  }

  .header-menu {
    flex: 1;
  }
  .header-menu :deep(.el-menu-item) {
    font-size: 14px;
    padding: 0 16px;
    height: 60px;
    line-height: 60px;
  }
  .header-menu :deep(.el-menu-item a) {
    display: block;
    color: inherit;
  }
  .header-actions {
    display: flex;
    align-items: center;
    gap: 12px;
  }
  .dropdown-trigger {
    cursor: pointer;
    display: flex;
    align-items: center;
  }
  .web-content {
    max-width: 1200px;
    margin: 0 auto;
    padding: 28px 24px;
    width: 100%;
    flex: 1;
    box-sizing: border-box;
  }
  .web-footer {
    text-align: center;
    padding: 24px 20px;
    color: #86909c;
    font-size: 13px;
    border-top: 1px solid #f0f0f0;
    background: #fff;
  }
  .web-footer a {
    color: #86909c;
    margin-left: 4px;
  }
  .web-footer a:hover {
    color: var(--el-color-primary);
  }
</style>
