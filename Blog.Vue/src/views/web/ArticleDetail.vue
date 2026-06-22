<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import { articleDetailApi, type ArticleDetailType } from '@/api/article'
  import { articleRecordHistoryApi } from '@/api/article'
  import CommentSection from '@/components/CommentSection.vue'

  const route = useRoute()
  const article = ref<ArticleDetailType | null>(null)
  const loading = ref(true)

  onMounted(async () => {
    try {
      const id = Number(route.params.id)
      const res = await articleDetailApi(id)
      if (res.code === 200) {
        article.value = res.data
        // 记录浏览历史
        articleRecordHistoryApi({ articleId: id }).catch(() => {})
      }
    } finally {
      loading.value = false
    }
  })
</script>

<template>
  <div class="article-detail" v-if="loading"><el-skeleton animated :loading="loading" /></div>
  <div class="article-detail" v-else-if="article">
    <h1 class="detail-title">{{ article.title }}</h1>
    <div class="detail-meta">
      <el-space>
        <el-avatar :size="24">{{ article.nickname?.charAt(0) }}</el-avatar>
        <span>{{ article.nickname }}</span>
        <span>{{ article.createdAt?.slice(0, 10) }}</span>
        <span>👁 {{ article.lookCount }}</span>
        <span>👍 {{ article.diggCount }}</span>
        <span>💬 {{ article.commentCount }}</span>
      </el-space>
    </div>
    <div v-if="article.cover" class="detail-cover">
      <img :src="article.cover" alt="cover" />
    </div>
    <div class="detail-content" v-html="article.content"></div>
    <!-- 评论区 -->
    <CommentSection :articleId="article.id" />
  </div>
  <div v-else class="not-found">文章不存在</div>
</template>

<style scoped>
  .article-detail {
    max-width: 800px;
    margin: 0 auto;
  }
  .detail-title {
    font-size: 28px;
    font-weight: 700;
    margin-bottom: 16px;
  }
  .detail-meta {
    margin-bottom: 24px;
    color: #86909c;
    font-size: 14px;
  }
  .detail-cover {
    margin-bottom: 24px;
  }
  .detail-cover img {
    width: 100%;
    max-height: 400px;
    object-fit: cover;
    border-radius: 8px;
  }
  .detail-content {
    line-height: 1.8;
    font-size: 16px;
  }
  .not-found {
    text-align: center;
    padding: 100px 0;
    font-size: 18px;
    color: #86909c;
  }
</style>
