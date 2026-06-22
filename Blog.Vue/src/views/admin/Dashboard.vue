<script setup lang="ts">
  import { ref, onMounted, onUnmounted, nextTick } from 'vue'
  import { dataSumApi, dataArticleGrowthApi, type DataSumType, type DataGrowthType } from '@/api/data'
  import * as echarts from 'echarts'

  const sum = ref<DataSumType | null>(null)
  const growth = ref<DataGrowthType | null>(null)
  const loading = ref(true)
  let chart: echarts.ECharts | null = null

  onMounted(async () => {
    try {
      const [sumRes, growthRes] = await Promise.all([dataSumApi(), dataArticleGrowthApi()])
      if (sumRes.code === 200) sum.value = sumRes.data
      if (growthRes.code === 200) growth.value = growthRes.data
      await nextTick()
      renderChart()
    } finally {
      loading.value = false
    }
  })

  onUnmounted(() => chart?.dispose())

  function renderChart() {
    const el = document.getElementById('growth-chart')
    if (!el || !growth.value) return
    chart = echarts.init(el)
    chart.setOption({
      tooltip: { trigger: 'axis' },
      xAxis: { type: 'category', data: growth.value.dateList },
      yAxis: { type: 'value' },
      series: [
        {
          type: 'line',
          smooth: true,
          data: growth.value.countList,
          areaStyle: { opacity: 0.3 },
          itemStyle: { color: '#4080ff' },
        },
      ],
      grid: { left: 60, right: 20, bottom: 40 },
    })
  }
</script>

<template>
  <div>
    <el-row :gutter="16" style="margin-bottom: 20px">
      <el-col :span="6"
        ><el-card v-loading="loading"
          ><div style="text-align: center; padding: 10px">
            <div style="font-size: 13px; color: #909399">文章总数</div>
            <div style="font-size: 28px; font-weight: 700; color: #409eff; margin-top: 8px">
              {{ sum?.articleCount || 0 }}
            </div>
          </div></el-card
        ></el-col
      >
      <el-col :span="6"
        ><el-card v-loading="loading"
          ><div style="text-align: center; padding: 10px">
            <div style="font-size: 13px; color: #909399">用户总数</div>
            <div style="font-size: 28px; font-weight: 700; color: #67c23a; margin-top: 8px">
              {{ sum?.userCount || 0 }}
            </div>
          </div></el-card
        ></el-col
      >
      <el-col :span="6"
        ><el-card v-loading="loading"
          ><div style="text-align: center; padding: 10px">
            <div style="font-size: 13px; color: #909399">评论总数</div>
            <div style="font-size: 28px; font-weight: 700; color: #e6a23c; margin-top: 8px">
              {{ sum?.commentCount || 0 }}
            </div>
          </div></el-card
        ></el-col
      >
      <el-col :span="6"
        ><el-card v-loading="loading"
          ><div style="text-align: center; padding: 10px">
            <div style="font-size: 13px; color: #909399">今日访问</div>
            <div style="font-size: 28px; font-weight: 700; color: #f56c6c; margin-top: 8px">
              {{ sum?.flowCount || 0 }}
            </div>
          </div></el-card
        ></el-col
      >
    </el-row>
    <el-row :gutter="16">
      <el-col :span="12">
        <el-card v-loading="loading"
          ><template #header>文章增长趋势</template>
          <div id="growth-chart" style="height: 350px"></div>
        </el-card>
      </el-col>
      <el-col :span="12">
        <el-card v-loading="loading"
          ><template #header>快速操作</template>
          <el-space direction="vertical" style="width: 100%" fill>
            <el-button type="primary" style="width: 100%" @click="$router.push('/admin/articles')">文章管理</el-button>
            <el-button style="width: 100%" @click="$router.push('/admin/users')">用户管理</el-button>
            <el-button style="width: 100%" @click="$router.push('/admin/settings')">站点配置</el-button>
          </el-space>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>
