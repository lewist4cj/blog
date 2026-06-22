<script setup lang="ts">
  import { ref } from 'vue'
  import { useUserStore } from '@/stores/user'
  import { userUpdateApi } from '@/api/user'
  import { ElMessage } from 'element-plus'

  const store = useUserStore()
  const nickname = ref(store.userInfo.nickname)
  const avatar = ref(store.userInfo.avatar)
  const loading = ref(false)

  async function handleSave() {
    loading.value = true
    try {
      const res = await userUpdateApi({ id: store.userInfo.id, nickname: nickname.value, avatar: avatar.value })
      if (res.code === 200) {
        store.setUserInfo({ nickname: nickname.value, avatar: avatar.value })
        ElMessage.success('保存成功')
      }
    } finally {
      loading.value = false
    }
  }
</script>

<template>
  <el-card>
    <template #header>账号设置</template>
    <el-form :model="{ nickname, avatar }" label-width="80px" style="max-width: 500px">
      <el-form-item label="昵称">
        <el-input v-model="nickname" placeholder="请输入昵称" />
      </el-form-item>
      <el-form-item label="头像URL">
        <el-input v-model="avatar" placeholder="头像图片地址" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" :loading="loading" @click="handleSave">保存</el-button>
      </el-form-item>
    </el-form>
  </el-card>
</template>
