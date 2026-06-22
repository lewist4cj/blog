<script setup lang="ts">
  import { ref } from 'vue'
  import { useRouter, useRoute } from 'vue-router'
  import { useUserStore } from '@/stores/user'
  import { pwdLoginApi } from '@/api/user'
  import { ElMessage } from 'element-plus'

  const router = useRouter()
  const route = useRoute()
  const store = useUserStore()

  const username = ref('')
  const password = ref('')
  const loading = ref(false)

  async function handleLogin() {
    if (!username.value || !password.value) {
      Message.warning('请输入用户名和密码')
      return
    }
    loading.value = true
    try {
      const res = await pwdLoginApi({ username: username.value, password: password.value })
      if (res.code === 200) {
        store.setToken(res.data)
        Message.success('登录成功')
        const redirect = (route.query.redirect as string) || '/'
        router.push(redirect)
      } else {
        Message.error(res.message || '登录失败')
      }
    } catch (err) {
      Message.error('登录失败: ' + (err as Error).message)
    } finally {
      loading.value = false
    }
  }
</script>

<template>
  <div class="login-page">
    <div class="login-card">
      <h2 class="login-title">登录</h2>
      <el-form :model="{ username, password }" label-width="60px" @submit.prevent="handleLogin">
        <el-form-item label="用户名">
          <el-input v-model="username" placeholder="请输入用户名" />
        </el-form-item>
        <el-form-item label="密码">
          <el-input type="password" v-model="password" placeholder="请输入密码" show-password />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" native-type="submit" :loading="loading" style="width: 100%">
            {{ loading ? '登录中...' : '登录' }}
          </el-button>
        </el-form-item>
      </el-form>
      <div class="login-footer">
        <router-link to="/">返回首页</router-link>
      </div>
    </div>
  </div>
</template>

<style scoped>
  .login-page {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: #f2f3f5;
  }
  .login-card {
    width: 400px;
    padding: 40px;
    background: #fff;
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  }
  .login-title {
    text-align: center;
    margin-bottom: 30px;
    font-size: 24px;
  }
  .login-footer {
    text-align: center;
    margin-top: 16px;
  }
  .login-footer a {
    color: #4080ff;
  }
</style>
