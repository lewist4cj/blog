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

<style scoped lang="scss">
  .login-page {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  }
  .login-card {
    width: 420px;
    padding: 48px 40px 40px;
    background: #fff;
    border-radius: 16px;
    box-shadow: 0 20px 60px rgba(0, 0, 0, 0.15);
  }
  .login-title {
    text-align: center;
    margin-bottom: 36px;
    font-size: 28px;
    font-weight: 700;
    color: #1d2129;
    letter-spacing: -0.5px;
  }
  .login-footer {
    text-align: center;
    margin-top: 20px;
  }
  .login-footer a {
    color: #909399;
    font-size: 13px;
  }
  .login-footer a:hover {
    color: var(--el-color-primary);
  }
</style>
