<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { siteMsgListApi, siteMsgReadApi, type SiteMsgType } from '@/api/message'
  import { useUserStore } from '@/stores/user'
  import { ElMessage } from 'element-plus'

  const store = useUserStore()
  const activeTab = ref<'comment' | 'digg' | 'system'>('comment')
  const messages = ref<SiteMsgType[]>([])
  const loading = ref(false)

  const tabMap = { comment: 1, digg: 2, system: 3 }

  async function load() {
    loading.value = true
    try {
      const res = await siteMsgListApi({ t: tabMap[activeTab.value] as 1 | 2 | 3, page: 1, limit: 50 })
      if (res.code === 200) messages.value = res.data.list
    } finally {
      loading.value = false
    }
  }

  async function handleRead(item: SiteMsgType) {
    if (item.isRead) return
    await siteMsgReadApi({ id: item.id, t: tabMap[activeTab.value] as 1 | 2 | 3 })
    item.isRead = true
    store.loadUnreadMsg()
  }

  onMounted(load)
  watch(activeTab, load)

  import { watch } from 'vue'
</script>

<template>
  <div class="msg-page">
    <el-card>
      <template #header><span>消息通知</span></template>
      <el-tabs v-model="activeTab" @tab-click="load">
        <el-tab-pane
          name="comment"
          :label="`评论和回复${store.unreadMsg.commentMsgCount > 0 ? `(${store.unreadMsg.commentMsgCount})` : ''}`"
        />
        <el-tab-pane
          name="digg"
          :label="`赞和收藏${store.unreadMsg.diggMsgCount > 0 ? `(${store.unreadMsg.diggMsgCount})` : ''}`"
        />
        <el-tab-pane
          name="system"
          :label="`系统消息${store.unreadMsg.systemMsgCount > 0 ? `(${store.unreadMsg.systemMsgCount})` : ''}`"
        />
      </el-tabs>
      <div v-loading="loading">
        <div
          v-for="item in messages"
          :key="item.id"
          class="msg-item"
          :class="{ unread: !item.isRead }"
          @click="handleRead(item)"
        >
          <div class="msg-avatar" v-if="item.actionUserAvatar">
            <el-avatar :size="40"><img :src="item.actionUserAvatar" /></el-avatar>
          </div>
          <div class="msg-body">
            <div class="msg-title">{{ item.title }}</div>
            <div class="msg-desc">{{ item.content }}</div>
          </div>
          <div class="msg-time">{{ item.createdAt?.slice(0, 16) }}</div>
        </div>
        <el-empty v-if="!loading && messages.length === 0" description="暂无消息" />
      </div>
    </el-card>
  </div>
</template>

<style scoped lang="scss">
  .msg-page {
    max-width: 800px;
    margin: 0 auto;
  }
  .msg-item {
    display: flex;
    align-items: center;
    padding: 14px 0;
    border-bottom: 1px solid #f0f0f0;
    cursor: pointer;
    gap: 12px;
  }
  .msg-item.unread {
    background: #f7f8fa;
    font-weight: 500;
    margin: 0 -16px;
    padding: 14px 16px;
  }
  .msg-body {
    flex: 1;
  }
  .msg-title {
    font-size: 14px;
    margin-bottom: 4px;
  }
  .msg-desc {
    font-size: 12px;
    color: #909399;
  }
  .msg-time {
    font-size: 12px;
    color: #c0c4cc;
    white-space: nowrap;
  }
</style>
