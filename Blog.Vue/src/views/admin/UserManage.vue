<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { userListApi, type UserListType } from '@/api/user'

  const users = ref<UserListType[]>([])
  const loading = ref(false)

  onMounted(async () => {
    loading.value = true
    try {
      const res = await userListApi({ page: 1, limit: 20 })
      if (res.code === 200) users.value = res.data.list
    } finally {
      loading.value = false
    }
  })
</script>

<template>
  <el-card
    ><template #header>用户管理</template>
    <el-table :data="users" v-loading="loading" style="width: 100%">
      <el-table-column prop="id" label="ID" width="80" />
      <el-table-column prop="username" label="用户名" />
      <el-table-column prop="nickname" label="昵称" />
      <el-table-column prop="ip" label="IP" />
      <el-table-column prop="createdAt" label="注册时间" width="180" />
    </el-table>
  </el-card>
</template>
