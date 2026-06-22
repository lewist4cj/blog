<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { articleListApi, articleExamineApi, type ArticleListType } from '@/api/article'
  import { ElMessage } from 'element-plus'

  const articles = ref<ArticleListType[]>([])
  const loading = ref(false)

  async function load() {
    loading.value = true
    try {
      const res = await articleListApi({ page: 1, limit: 20 })
      if (res.code === 200) articles.value = res.data.list
    } finally {
      loading.value = false
    }
  }

  async function handleExamine(id: number, status: number) {
    try {
      await articleExamineApi({ articleID: id, status, msg: '' })
      Message.success(status === 3 ? '已通过' : '已拒绝')
      load()
    } catch {
      Message.error('操作失败')
    }
  }

  onMounted(load)
</script>

<template>
  <el-card
    ><template #header>文章管理</template>
    <el-table :data="articles" v-loading="loading" style="width: 100%">
      <el-table-column prop="title" label="标题" />
      <el-table-column prop="userNickname" label="作者" width="120" />
      <el-table-column label="状态" width="100">
        <template #default="{ row }">
          <el-tag :type="row.status === 3 ? 'success' : row.status === 2 ? 'warning' : 'info'">
            {{ { 1: '草稿', 2: '审核中', 3: '已发布' }[row.status as number] || '未知' }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="createdAt" label="时间" width="180" />
      <el-table-column label="操作" width="160">
        <template #default="{ row }">
          <el-button v-if="row.status === 2" type="success" link size="small" @click="handleExamine(row.id, 3)"
            >通过</el-button
          >
          <el-button v-if="row.status === 2" type="danger" link size="small" @click="handleExamine(row.id, 4)"
            >拒绝</el-button
          >
        </template>
      </el-table-column>
    </el-table>
  </el-card>
</template>
