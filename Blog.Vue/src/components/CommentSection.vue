<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { commentListApi, commentCreateApi, commentDeleteApi } from '@/api/comment'
  import { useUserStore } from '@/stores/user'
  import { ElMessage, ElMessageBox } from 'element-plus'

  const props = defineProps<{ articleId: number }>()
  const store = useUserStore()

  const comments = ref<any[]>([])
  const loading = ref(false)
  const newComment = ref('')

  async function loadComments() {
    loading.value = true
    try {
      const res = await commentListApi({ page: 1, limit: 50, articleId: props.articleId })
      if (res.code === 200) comments.value = res.data.list
    } finally {
      loading.value = false
    }
  }

  async function submitComment() {
    if (!newComment.value.trim()) return
    if (!store.isLogin) {
      Message.warning('请先登录')
      return
    }
    try {
      const res = await commentCreateApi({ content: newComment.value, articleId: props.articleId })
      if (res.code === 200) {
        Message.success('评论成功')
        newComment.value = ''
        loadComments()
      }
    } catch {
      Message.error('评论失败')
    }
  }

  function handleDelete(id: number) {
    Modal.confirm({
      title: '删除评论',
      content: '确定要删除这条评论吗？',
      onOk: async () => {
        await commentDeleteApi(id)
        Message.success('已删除')
        loadComments()
      },
    })
  }

  onMounted(loadComments)
</script>

<template>
  <div class="comment-section">
    <h3>评论 ({{ comments.length }})</h3>
    <div class="comment-input" v-if="store.isLogin">
      <el-input type="textarea" v-model="newComment" placeholder="写下你的评论..." :rows="3" />
      <el-button type="primary" style="margin-top: 12px" @click="submitComment">发表评论</el-button>
    </div>
    <div v-else class="login-hint"><router-link to="/login">登录</router-link>后发表评论</div>
    <div v-loading="loading" style="margin-top: 20px">
      <div v-for="item in comments" :key="item.id" class="comment-item">
        <div class="comment-header">
          <el-avatar :size="36">{{ item.nickname?.charAt(0) }}</el-avatar>
          <span class="comment-author">{{ item.nickname }}</span>
          <span class="comment-time">{{ item.createdAt?.slice(0, 16) }}</span>
        </div>
        <div class="comment-content">{{ item.content }}</div>
        <div class="comment-actions">
          <el-button v-if="item.isMe" type="danger" link size="small" @click="handleDelete(item.id)">删除</el-button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
  .comment-section {
    margin-top: 40px;
  }
  .comment-input {
    margin-top: 16px;
  }
  .login-hint {
    margin-top: 16px;
    color: #86909c;
  }
  .login-hint a {
    color: #409eff;
  }
  .comment-item {
    padding: 16px 0;
    border-bottom: 1px solid #f0f0f0;
  }
  .comment-header {
    display: flex;
    align-items: center;
    gap: 8px;
    margin-bottom: 8px;
  }
  .comment-author {
    font-weight: 600;
    font-size: 14px;
  }
  .comment-time {
    font-size: 12px;
    color: #999;
    margin-left: auto;
  }
  .comment-content {
    font-size: 14px;
    line-height: 1.6;
    margin-left: 44px;
  }
  .comment-actions {
    margin-left: 44px;
    margin-top: 4px;
  }
</style>
