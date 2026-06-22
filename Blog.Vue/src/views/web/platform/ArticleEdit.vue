<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { articleDetailApi, articleCreateApi, articleEditApi } from '@/api/article'
  import { ElMessage } from 'element-plus'

  const route = useRoute()
  const router = useRouter()
  const isEdit = !!route.params.id
  const title = ref('')
  const content = ref('')
  const abstract = ref('')
  const categoryId = ref<number>()
  const tagList = ref('')
  const loading = ref(false)

  onMounted(async () => {
    if (isEdit) {
      const id = Number(route.params.id)
      const res = await articleDetailApi(id)
      if (res.code === 200) {
        title.value = res.data.title
        content.value = res.data.content
        abstract.value = res.data.abstract || ''
        categoryId.value = res.data.categoryId
        tagList.value = (res.data.tagList || []).join(',')
      }
    }
  })

  async function handleSave(status: number) {
    if (!title.value) {
      Message.warning('请输入标题')
      return
    }
    loading.value = true
    try {
      const data = {
        title: title.value,
        content: content.value,
        abstract: abstract.value,
        categoryId: categoryId.value,
        tagList: tagList.value,
        status,
      }
      if (isEdit) {
        await articleEditApi({ ...data, id: Number(route.params.id) })
        Message.success('更新成功')
      } else {
        await articleCreateApi(data)
        Message.success('创建成功')
      }
      router.push({ name: 'platformArticle' })
    } finally {
      loading.value = false
    }
  }
</script>

<template>
  <el-card
    ><template #header>{{ isEdit ? '编辑文章' : '写文章' }}</template>
    <el-form :model="{ title, content, abstract, categoryId, tagList }" label-width="60px">
      <el-form-item label="标题">
        <el-input v-model="title" placeholder="文章标题" />
      </el-form-item>
      <el-form-item label="摘要">
        <el-input type="textarea" v-model="abstract" placeholder="文章摘要" :rows="3" />
      </el-form-item>
      <el-form-item label="标签">
        <el-input v-model="tagList" placeholder="标签，用逗号分隔" />
      </el-form-item>
      <el-form-item label="内容">
        <el-input type="textarea" v-model="content" placeholder="文章内容（支持 HTML）" :rows="15" />
      </el-form-item>
      <el-form-item>
        <el-space>
          <el-button type="primary" :loading="loading" @click="handleSave(3)">发布</el-button>
          <el-button :loading="loading" @click="handleSave(1)">存草稿</el-button>
          <el-button @click="router.back()">取消</el-button>
        </el-space>
      </el-form-item>
    </el-form>
  </el-card>
</template>
