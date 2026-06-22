<template>
  <div class="profile-layout">
    <el-menu mode="horizontal">
      <el-menu-item index="userArticle"><router-link :to="{ name: 'userArticle' }">文章</router-link></el-menu-item>
      <el-menu-item index="userCollect"><router-link :to="{ name: 'userCollect' }">收藏</router-link></el-menu-item>
    </el-menu>
    <div class="profile-content"><router-view /></div>
  </div>
</template>

<style scoped>
  .profile-layout {
    max-width: 900px;
    margin: 0 auto;
  }
  .profile-content {
    margin-top: 20px;
  }
</style>
