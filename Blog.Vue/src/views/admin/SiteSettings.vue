<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { siteApi, siteUpdateApi } from '@/api/site'
  import { ElMessage } from 'element-plus'

  const activeTab = ref('site')
  const siteForm = ref({ title: '', logo: '', beian: '', enTitle: '', slogan: '' })
  const projectForm = ref({ title: '', icon: '', webPath: '' })
  const seoForm = ref({ keywords: '', description: '', author: '' })
  const aboutForm = ref({ siteDate: '', version: '', qq: '', wechat: '', github: '', gitee: '', bilibili: '' })
  const loginForm = ref({ qqLogin: false, usernamePwdLogin: true, emailLogin: false, captcha: false })
  const articleForm = ref({ noExamine: false, commentLine: 4 })
  const loading = ref(false)
  const saving = ref(false)

  async function load() {
    loading.value = true
    try {
      const res = await siteApi('site')
      if (res.code === 200) {
        const d = res.data as any
        siteForm.value = {
          title: d.siteInfo?.title || '',
          logo: d.siteInfo?.logo || '',
          beian: d.siteInfo?.beian || '',
          enTitle: d.enTitle || '',
          slogan: d.slogan || '',
        }
        projectForm.value = d.project || projectForm.value
        seoForm.value = d.seo || seoForm.value
        aboutForm.value = d.about || aboutForm.value
        loginForm.value = d.login || loginForm.value
        articleForm.value = d.article || articleForm.value
      }
    } finally {
      loading.value = false
    }
  }

  async function handleSave() {
    saving.value = true
    try {
      await siteUpdateApi('site', {
        siteInfo: siteForm.value,
        project: projectForm.value,
        seo: seoForm.value,
        about: aboutForm.value,
        login: loginForm.value,
        article: articleForm.value,
      })
      Message.success('保存成功')
    } catch {
      Message.error('保存失败')
    } finally {
      saving.value = false
    }
  }

  onMounted(load)
</script>

<template>
  <el-card v-loading="loading"
    ><template #header>站点配置</template>
    <el-form :model="siteForm" label-width="100px" style="max-width: 600px">
      <el-tabs v-model="activeTab">
        <el-tab-pane name="site" label="网站设置">
          <el-form-item label="站点名称"><el-input v-model="siteForm.title" /></el-form-item>
          <el-form-item label="英文名称"><el-input v-model="siteForm.enTitle" /></el-form-item>
          <el-form-item label="标语"><el-input v-model="siteForm.slogan" /></el-form-item>
          <el-form-item label="Logo URL"><el-input v-model="siteForm.logo" /></el-form-item>
          <el-form-item label="备案号"><el-input v-model="siteForm.beian" /></el-form-item>
        </el-tab-pane>
        <el-tab-pane name="project" label="项目信息">
          <el-form-item label="项目标题"><el-input v-model="projectForm.title" /></el-form-item>
          <el-form-item label="图标"><el-input v-model="projectForm.icon" /></el-form-item>
          <el-form-item label="前端地址"><el-input v-model="projectForm.webPath" /></el-form-item>
        </el-tab-pane>
        <el-tab-pane name="seo" label="SEO">
          <el-form-item label="关键词"><el-input v-model="seoForm.keywords" /></el-form-item>
          <el-form-item label="描述"><el-input type="textarea" v-model="seoForm.description" :rows="3" /></el-form-item>
          <el-form-item label="作者"><el-input v-model="seoForm.author" /></el-form-item>
        </el-tab-pane>
        <el-tab-pane name="about" label="关于">
          <el-form-item label="站点日期"><el-input v-model="aboutForm.siteDate" /></el-form-item>
          <el-form-item label="版本号"><el-input v-model="aboutForm.version" /></el-form-item>
          <el-form-item label="QQ"><el-input v-model="aboutForm.qq" /></el-form-item>
          <el-form-item label="微信"><el-input v-model="aboutForm.wechat" /></el-form-item>
          <el-form-item label="GitHub"><el-input v-model="aboutForm.github" /></el-form-item>
          <el-form-item label="Gitee"><el-input v-model="aboutForm.gitee" /></el-form-item>
          <el-form-item label="B站"><el-input v-model="aboutForm.bilibili" /></el-form-item>
        </el-tab-pane>
        <el-tab-pane name="login" label="登录设置">
          <el-form-item label="用户名密码登录"><el-switch v-model="loginForm.usernamePwdLogin" /></el-form-item>
          <el-form-item label="QQ登录"><el-switch v-model="loginForm.qqLogin" /></el-form-item>
          <el-form-item label="邮箱登录"><el-switch v-model="loginForm.emailLogin" /></el-form-item>
          <el-form-item label="验证码"><el-switch v-model="loginForm.captcha" /></el-form-item>
        </el-tab-pane>
        <el-tab-pane name="article" label="文章设置">
          <el-form-item label="免审核发布"><el-switch v-model="articleForm.noExamine" /></el-form-item>
          <el-form-item label="评论楼层数"><el-input-number v-model="articleForm.commentLine" /></el-form-item>
        </el-tab-pane>
      </el-tabs>
      <el-form-item style="margin-top: 20px">
        <el-button type="primary" :loading="saving" @click="handleSave">保存配置</el-button>
      </el-form-item>
    </el-form>
  </el-card>
</template>
