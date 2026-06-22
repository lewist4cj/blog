<template>
  <div class="not-found">
    <h1>404</h1>
    <p>页面不存在</p>
    <router-link to="/">返回首页</router-link>
  </div>
</template>

<style scoped>
  .not-found {
    text-align: center;
    padding: 100px 0;
  }
  .not-found h1 {
    font-size: 72px;
    color: #4080ff;
    margin-bottom: 16px;
  }
  .not-found p {
    font-size: 18px;
    color: #86909c;
    margin-bottom: 24px;
  }
</style>
