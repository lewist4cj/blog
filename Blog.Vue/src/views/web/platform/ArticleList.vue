<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { articleListApi, articleDeleteApi, type ArticleListType } from '@/api/article'
  import { useRouter } from 'vue-router'
  import { ElMessage, ElMessageBox } from 'element-plus'

  const router = useRouter()
  const articles = ref<ArticleListType[]>([])
  const loading = ref(false)
  const page = ref(1)
  const limit = 10

  async function load() {
    loading.value = true
    try {
      const res = await articleListApi({ page: page.value, limit, status: 3 })
      if (res.code === 200) articles.value = res.data.list
    } finally {
      loading.value = false
    }
  }

  function goEdit(id: number) {
    router.push({ name: 'platformArticleEdit', params: { id } })
  }

  function handleDelete(id: number) {
    Modal.confirm({
      title: '确认删除',
      content: '确定要删除这篇文章吗？',
      onOk: async () => {
        await articleDeleteApi(id)
        Message.success('删除成功')
        load()
      },
    })
  }

  onMounted(load)
</script>

<template>
  <el-card
    ><template #header>文章管理</template>
    <el-button type="primary" style="margin-bottom: 16px" @click="router.push({ name: 'platformArticleAdd' })"
      >写文章</el-button
    >
    <el-table :data="articles" v-loading="loading" style="width: 100%">
      <el-table-column prop="title" label="标题" />
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
          <el-button type="primary" link size="small" @click="goEdit(row.id)">编辑</el-button>
          <el-button type="danger" link size="small" @click="handleDelete(row.id)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-card>
</template>
