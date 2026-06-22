<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useUserStore } from '@/stores/user'
  import { userInfoApi } from '@/api/user'

  const store = useUserStore()
  const userDetail = ref<Record<string, unknown>>({})
  const loading = ref(true)

  onMounted(async () => {
    if (store.userInfo.id) {
      try {
        const res = await userInfoApi(store.userInfo.id)
        if (res.code === 200) userDetail.value = res.data as unknown as Record<string, unknown>
      } catch {
        /* ignore */
      } finally {
        loading.value = false
      }
    } else loading.value = false
  })
</script>

<template>
  <el-card v-loading="loading">
    <template #header>个人信息</template>
    <div style="text-align: center; margin-bottom: 20px">
      <el-avatar :size="80">
        <img v-if="store.userInfo.avatar" :src="store.userInfo.avatar" alt="avatar" />
        <span v-else>{{ store.userInfo.nickname?.charAt(0) }}</span>
      </el-avatar>
    </div>
    <el-descriptions :column="2" border>
      <el-descriptions-item label="昵称">{{ store.userInfo.nickname }}</el-descriptions-item>
      <el-descriptions-item label="用户名">{{ store.userInfo.username }}</el-descriptions-item>
      <el-descriptions-item label="个人简介">{{ (userDetail as any).abstract || '未设置' }}</el-descriptions-item>
      <el-descriptions-item label="文章数">{{ store.userInfo.articleCount }}</el-descriptions-item>
      <el-descriptions-item label="粉丝数">{{ store.userInfo.fansCount }}</el-descriptions-item>
      <el-descriptions-item label="关注数">{{ store.userInfo.followCount }}</el-descriptions-item>
      <el-descriptions-item label="总浏览量">{{ store.userInfo.lookCount }}</el-descriptions-item>
      <el-descriptions-item label="角色">{{
        store.userInfo.role === 1 ? '管理员' : store.userInfo.role === 2 ? '用户' : '超级管理员'
      }}</el-descriptions-item>
    </el-descriptions>
  </el-card>
</template>
