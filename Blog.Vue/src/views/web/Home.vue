<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { articleListApi, articleRecommendApi, type ArticleListType } from '@/api/article'
  import { useRouter } from 'vue-router'

  const router = useRouter()
  const articles = ref<ArticleListType[]>([])
  const loading = ref(false)
  const total = ref(0)
  const page = ref(1)
  const limit = 10

  async function loadArticles() {
    loading.value = true
    try {
      const res = await articleListApi({ page: page.value, limit })
      if (res.code === 200) {
        articles.value = res.data.list
        total.value = res.data.totalCount || 0
      }
    } finally {
      loading.value = false
    }
  }

  function goDetail(id: number) {
    router.push({ name: 'articleDetail', params: { id } })
  }

  onMounted(loadArticles)
</script>

<template>
  <div class="home-page">
    <div class="article-list" v-loading="loading">
      <div v-for="item in articles" :key="item.id" class="article-item-wrapper">
        <el-card shadow="hover" class="article-card" @click="goDetail(item.id)">
          <div class="article-item">
            <div class="article-info">
              <div class="article-title">{{ item.title }}</div>
              <div class="article-abstract">{{ item.abstract }}</div>
              <div class="article-meta">
                <el-space size="medium">
                  <span>{{ item.userNickname }}</span>
                  <span>{{ item.createdAt?.slice(0, 10) }}</span>
                  <span>👁 {{ item.lookCount }}</span>
                  <span>👍 {{ item.diggCount }}</span>
                  <span>💬 {{ item.commentCount }}</span>
                </el-space>
              </div>
            </div>
            <div v-if="item.cover" class="article-cover">
              <img :src="item.cover" alt="cover" />
            </div>
          </div>
        </el-card>
      </div>
      <el-pagination
        v-if="total > limit"
        :total="total"
        v-model:current-page="page"
        :page-size="limit"
        @current-change="loadArticles"
        layout="prev,pager,next"
        style="margin-top: 20px; justify-content: center"
      />
    </div>
  </div>
</template>

<style scoped lang="scss">
  .home-page {
    max-width: 900px;
    margin: 0 auto;
  }
  .article-card {
    margin-bottom: 16px;
    cursor: pointer;
  }
  .article-item {
    display: flex;
    gap: 20px;
  }
  .article-info {
    flex: 1;
  }
  .article-title {
    font-size: 18px;
    font-weight: 600;
    margin-bottom: 8px;
    color: #1d2129;
  }
  .article-abstract {
    font-size: 14px;
    color: #86909c;
    margin-bottom: 12px;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
  .article-meta {
    font-size: 12px;
    color: #c9cdd4;
  }
  .article-cover {
    width: 200px;
    flex-shrink: 0;
  }
  .article-cover img {
    width: 100%;
    height: 120px;
    object-fit: cover;
    border-radius: 4px;
  }
</style>
