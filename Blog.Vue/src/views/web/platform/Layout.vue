<template>
  <div class="platform-layout">
    <el-menu mode="horizontal" :default-active="router.currentRoute.value.name as string">
      <el-menu-item index="platformArticle"
        ><router-link :to="{ name: 'platformArticle' }">文章列表</router-link></el-menu-item
      >
      <el-menu-item index="platformArticleAdd"
        ><router-link :to="{ name: 'platformArticleAdd' }">写文章</router-link></el-menu-item
      >
    </el-menu>
    <div class="platform-content"><router-view /></div>
  </div>
</template>

<script setup lang="ts">
  import { useRouter } from 'vue-router'
  const router = useRouter()
</script>

<style scoped>
  .platform-layout {
    max-width: 1000px;
    margin: 0 auto;
  }
  .platform-content {
    margin-top: 20px;
  }
</style>
