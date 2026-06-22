<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { articleHistoryListApi, type ArticleListType } from '@/api/article'
  import { useRouter } from 'vue-router'

  const router = useRouter()
  const items = ref<ArticleListType[]>([])
  const loading = ref(false)

  onMounted(async () => {
    loading.value = true
    try {
      const res = await articleHistoryListApi({ page: 1, limit: 20 })
      if (res.code === 200) items.value = res.data.list
    } finally {
      loading.value = false
    }
  })
</script>

<template>
  <el-card v-loading="loading">
    <template #header>浏览历史</template>
    <div v-if="items.length > 0">
      <div
        v-for="item in items"
        :key="item.id"
        class="history-item"
        @click="router.push({ name: 'articleDetail', params: { id: item.id } })"
      >
        <div class="history-title">{{ item.title }}</div>
        <div class="history-time">{{ item.createdAt?.slice(0, 16) }}</div>
      </div>
    </div>
    <el-empty v-else description="暂无浏览记录" />
  </el-card>
</template>
