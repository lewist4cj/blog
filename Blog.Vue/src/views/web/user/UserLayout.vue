<template>
  <div class="user-layout">
    <el-card><template #header>用户中心</template></el-card>
    <el-menu mode="horizontal" :default-active="router.currentRoute.value.name as string">
      <el-menu-item index="userInfo"><router-link :to="{ name: 'userInfo' }">个人信息</router-link></el-menu-item>
      <el-menu-item index="userAccount"><router-link :to="{ name: 'userAccount' }">账号设置</router-link></el-menu-item>
      <el-menu-item index="userHistory"><router-link :to="{ name: 'userHistory' }">浏览历史</router-link></el-menu-item>
    </el-menu>
    <div class="user-content"><router-view /></div>
  </div>
</template>

<script setup lang="ts">
  import { useRouter } from 'vue-router'
  const router = useRouter()
</script>

<style scoped>
  .user-layout {
    max-width: 900px;
    margin: 0 auto;
  }
  .user-content {
    margin-top: 20px;
  }
</style>
